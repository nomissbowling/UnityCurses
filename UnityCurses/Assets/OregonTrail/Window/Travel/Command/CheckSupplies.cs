﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Control;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;
using Assets.OregonTrail.Entity;

namespace Assets.OregonTrail.Window.Travel.Command
{
    /// <summary>
    ///     Shows all the players supplies that they currently have in their vehicle inventory, along with the amount of money
    ///     they have. This screen is not for looking at group stats, only items which are normally not shown unlike the travel
    ///     menu that shows basic party stats at all times.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public class CheckSupplies : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CheckSupplies" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public CheckSupplies(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Tick the people, but not the trail or the day.
            OregonTrailApp.Instance.TakeTurn(true);

            // Build up representation of supplies once in constructor and then reference when asked for render.
            var supplies = new StringBuilder();
            supplies.AppendLine(string.Format("{0}Your Supplies{1}", Environment.NewLine, Environment.NewLine));

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
                        suppliesList.Add(new SuppyItem("money left", item.Value.TotalValue.ToString("C")));
                        break;
                    case Entities.Vehicle:
                    case Entities.Person:
                    case Entities.Location:
                        throw new ArgumentOutOfRangeException();
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
            supplies.AppendLine(supplyTable);

            return supplies.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = null;
            ClearForm();
        }
    }
}