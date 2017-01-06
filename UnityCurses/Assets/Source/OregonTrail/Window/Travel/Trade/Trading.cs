﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using System.Text;
using Assets.Source.Engine;
using Assets.Source.Engine.Utility;
using Assets.Source.Engine.Window;
using Assets.Source.Engine.Window.Control;
using Assets.Source.Engine.Window.Form;
using Assets.Source.Engine.Window.Form.Input;
using Assets.Source.OregonTrail.Entity;
using Assets.Source.OregonTrail.Entity.Vehicle;
using Assets.Source.OregonTrail.Window.Travel.Command;

namespace Assets.Source.OregonTrail.Window.Travel.Trade
{
    /// <summary>
    ///     Handles the interaction of the player party and another AI controlled party that offers up items for trading which
    ///     the player can choose to accept or not.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class Trading : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Representation of text that shows current supplies and the trade offer if one exists.
        /// </summary>
        private static StringBuilder supplyPrompt;

        /// <summary>
        ///     Determines if the player is able to make the current trade offer with the supplies they have available in their
        ///     vehicles inventory.
        /// </summary>
        private bool playerCanTrade;

        /// <summary>
        ///     Index of the item which we are going to use as the trade the player is going to make and be offered, if it exists.
        /// </summary>
        private int tradeIndex;

        /// <summary>
        ///     References all of the possible trades this location will be able to offer the player. If the list is empty that
        ///     means nobody wants to trade with the player at this time.
        /// </summary>
        private List<TradeOffer> trades;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Trading" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public Trading(IWindow window) : base(window)
        {
            supplyPrompt = new StringBuilder();
        }

        /// <summary>
        ///     Builds up representation of supplies once in constructor and then reference when asked for render.
        /// </summary>
        /// <returns>Formatted text table that shows vehicle current supplies.</returns>
        private static string SupplyTextTable
        {
            get
            {
                var tradeTable = new StringBuilder();
                tradeTable.AppendLine(string.Format("{0}Your Supplies{1}", Environment.NewLine, Environment.NewLine));

                // Build up a list with tuple in it to hold our data about supplies.
                var suppliesList = new List<SuppyItem>();

                // Loop through every inventory item in the vehicle.
                foreach (var item in OregonTrailApp.Instance.Vehicle.Inventory)
                {
                    // Apply number formatting to quantities so they have thousand separators.
                    var itemFormattedQuantity = item.Value.Quantity.ToString("N0");

                    // Change up how we print out various items in the vehicle inventory.
                    switch (item.Key)
                    {
                        case Entities.Animal:
                            suppliesList.Add(new SuppyItem("oxen", itemFormattedQuantity));
                            break;
                        case Entities.Clothes:
                            suppliesList.Add(new SuppyItem("sets of clothing", itemFormattedQuantity));
                            break;
                        case Entities.Ammo:
                            suppliesList.Add(new SuppyItem("bullets", itemFormattedQuantity));
                            break;
                        case Entities.Wheel:
                            suppliesList.Add(new SuppyItem("wagon wheels", itemFormattedQuantity));
                            break;
                        case Entities.Axle:
                            suppliesList.Add(new SuppyItem("wagon axles", itemFormattedQuantity));
                            break;
                        case Entities.Tongue:
                            suppliesList.Add(new SuppyItem("wagon tongues", itemFormattedQuantity));
                            break;
                        case Entities.Food:
                            suppliesList.Add(new SuppyItem("pounds of food",
                                item.Value.TotalWeight.ToString("N0")));
                            break;
                        case Entities.Cash:
                        case Entities.Vehicle:
                        case Entities.Person:
                        case Entities.Location:
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // Generate the formatted table of supplies we will show to user.
                var supplyTable = suppliesList.ToStringTable(
                    new[] {"Item Name", "Amount"},
                    u => u.Name,
                    u => u.AmountPretty);

                // Add the table to the text user interface.
                tradeTable.AppendLine(supplyTable);
                return tradeTable.ToString();
            }
        }

        /// <summary>
        ///     Defines what type of dialog this will act like depending on this enumeration value. Up to implementation to define
        ///     desired behavior.
        /// </summary>
        protected override DialogType DialogType
        {
            get
            {
                // Dialog type is determined by players ability to trade against the generated offer.
                if (trades != null && trades.Count > 0 && playerCanTrade)
                    return DialogType.YesNo;

                return DialogType.Prompt;
            }
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The dialog prompt text.<see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            UpdateTrade();

            // Returns the completed table of supplies and selected trade offer.
            return supplyPrompt.ToString();
        }

        /// <summary>
        ///     Fired when the window is activated and or refocused after another window was removed from being on-top of it.
        ///     Useful for re-initializing form data after something like a random event runs which might kill people or alter the
        ///     vehicle inventory.
        /// </summary>
        public override void OnFormActivate()
        {
            base.OnFormActivate();

            UpdateTrade();
        }

        /// <summary>
        ///     Creates a new possible trade for the player, or not.
        /// </summary>
        private void UpdateTrade()
        {
            // Tick the people, but not the trail or the day.
            OregonTrailApp.Instance.TakeTurn(false);

            // Grabs all the data for the player current vehicle inventory.
            supplyPrompt = new StringBuilder();
            supplyPrompt.Append(SupplyTextTable);

            // Trades are randomly generated when ticking the location.
            GenerateTrades();

            // Generate a random number based on trade count and what our trade will be.
            tradeIndex = EngineApp.Random.Next(trades.Count);

            // Check if the player has the item in question the trader wants.
            playerCanTrade = trades.Count > 0 &&
                             OregonTrailApp.Instance.Vehicle.ContainsItem(trades[tradeIndex].WantedItem);

            // Select one of the trades to use, or say there are none if none generated.
            if (trades.Count > 0)
            {
                // Generates the default prompt for trading that is shown if you have items to trade back or not.
                var wrapText =
                    string.Format("You meet another emigrant who wants {0:N0} {1}. ",
                        trades[tradeIndex].WantedItem.Quantity, trades[tradeIndex].WantedItem.Name.ToLowerInvariant()) +
                    string.Format("He will trade you {0:N0} {1}.", trades[tradeIndex].OfferedItem.Quantity,
                        trades[tradeIndex].OfferedItem.Name.ToLowerInvariant());

                // Depending if the player has enough of what the trader wants we change up last part of message.
                supplyPrompt.Append(playerCanTrade
                    ? string.Format("{0}{1}Are you willing to trade? Y/N", wrapText.WordWrap(32), Environment.NewLine)
                    : string.Format("{0}{1}You don't have this.{2}{3}", wrapText.WordWrap(32), Environment.NewLine,
                        Environment.NewLine, Environment.NewLine));
            }
            else
            {
                // Prompt is not shown if we have no traders generated.
                supplyPrompt.AppendLine(string.Format("Nobody wants to trade with you.{0}", Environment.NewLine));
            }
        }

        /// <summary>
        ///     Creates some possible trades the player can have selected at random.
        /// </summary>
        private void GenerateTrades()
        {
            // Creates new list of trade offers.
            trades = new List<TradeOffer>();

            // Figure out how many trades, if any we will have this time the player checks.
            var totalTrades = EngineApp.Random.Next(0, EngineApp.Random.Next(1, 100));

            // Check if we just stop here.
            if (totalTrades <= 0)
                return;

            // Creates as many trade offers as generator says we should.
            for (var i = 0; i < totalTrades; i++)
                trades.Add(new TradeOffer());

            // Cleanup the generated trades.
            var copyTrades = new List<TradeOffer>(trades);
            foreach (var trade in copyTrades)
            {
                // Remove trades that are null on either side.
                if (trade.WantedItem != null && trade.OfferedItem != null &&
                    trade.WantedItem.Category != trade.OfferedItem.Category)
                    continue;

                // Remove any trades that are the same item twice.
                trades.Remove(trade);
            }
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            switch (reponse)
            {
                case DialogResponse.Yes:
                {
                    // Remove the quantity of item from the vehicle inventory the trader wants.
                    OregonTrailApp.Instance.Vehicle.Inventory[trades[tradeIndex].WantedItem.Category].ReduceQuantity(
                        trades[tradeIndex].WantedItem.Quantity);

                    // Give the vehicle the item the trade said he would.
                    OregonTrailApp.Instance.Vehicle.Inventory[trades[tradeIndex].OfferedItem.Category].AddQuantity(
                        trades[tradeIndex].OfferedItem.Quantity);

                    // Checks if the player has animals to pull their vehicle.
                    OregonTrailApp.Instance.Vehicle.Status =
                        OregonTrailApp.Instance.Vehicle.Inventory[Entities.Animal].Quantity <= 0
                            ? VehicleStatus.Disabled
                            : VehicleStatus.Moving;

                    // Return to the travel menu.
                    ClearForm();
                    return;
                }
                case DialogResponse.Custom:
                case DialogResponse.No:
                {
                    // Return to the travel menu.
                    ClearForm();
                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException("reponse", reponse, null);
            }
        }
    }
}