// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Source.Engine.Window;
using Assets.Source.Engine.Window.Form;
using Assets.Source.Engine.Window.Form.Input;
using Assets.Source.OregonTrail.Entity;
using Assets.Source.OregonTrail.Entity.Location.Point;
using Assets.Source.OregonTrail.Window.Travel.Dialog;

namespace Assets.Source.OregonTrail.Window.Travel.Toll
{
    /// <summary>
    ///     Prompts the user with a question about the toll road location they are attempting to progress to. Depending on
    ///     player cash reserves they might not be able to afford the toll road, in this case the message will only explain
    ///     this to the user and return to the fork in the road or travel menu. If the player can afford it they are asked if
    ///     they would like to proceed, if YES then they will have the monies subtracted from their vehicle inventory and
    ///     proceed to toll road location.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class TollRoadQuestion : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Figures out of the vehicle has enough cash to use the toll road, this is generally used as a check for the dialog.
        /// </summary>
        private bool canAffordToll;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InputForm{T}" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public TollRoadQuestion(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Determines if user input is currently allowed to be typed and filled into the input buffer.
        /// </summary>
        /// <remarks>Default is FALSE. Setting to TRUE allows characters and input buffer to be read when submitted.</remarks>
        public override bool InputFillsBuffer
        {
            get { return canAffordToll; }
        }

        /// <summary>
        ///     Defines what type of dialog this will act like depending on this enumeration value. Up to implementation to define
        ///     desired behavior.
        /// </summary>
        protected override DialogType DialogType
        {
            get { return canAffordToll ? DialogType.YesNo : DialogType.Prompt; }
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The dialog prompt text.<see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            var tollPrompt = new StringBuilder();

            // Grab instance of the game simulation.
            var game = OregonTrailApp.Instance;

            canAffordToll = game.Vehicle.Inventory[Entities.Cash].TotalValue >= UserData.Toll.Cost;

            // First portion of the message changes based on varying conditions.
            if (UserData.Toll.Road != null)
            {
                // Fork in the road sent us here.
                tollPrompt.AppendLine(
                    string.Format("{0}You must pay {1:C0} to travel the", Environment.NewLine, UserData.Toll.Cost));
                tollPrompt.AppendLine(string.Format("{0}.", UserData.Toll.Road.Name));
            }
            else if (game.Trail.CurrentLocation != null)
            {
                // Toll road was placed in-line on the trail, and will block player if they are broke.
                tollPrompt.AppendLine(
                    string.Format("{0}You must pay {1} to travel the", Environment.NewLine, UserData.Toll.Cost));
                tollPrompt.AppendLine(string.Format("{0}.", game.Trail.CurrentLocation.Name));
            }
            else if (game.Trail.NextLocation != null)
            {
                tollPrompt.AppendLine(
                    string.Format("{0}You must pay {1} to travel the", Environment.NewLine, UserData.Toll.Cost));
                tollPrompt.AppendLine(string.Format("{0}.", game.Trail.NextLocation.Name));
            }
            else
            {
                tollPrompt.AppendLine(
                    string.Format("{0}You must pay {1} to travel the", Environment.NewLine, UserData.Toll.Cost));
                tollPrompt.AppendLine("indefinable road.");
            }

            // Check if the player has enough money to pay for the toll road.
            if (game.Vehicle.Inventory[Entities.Cash].TotalValue >= UserData.Toll.Cost)
            {
                tollPrompt.AppendLine(string.Format("{0}Are you willing", Environment.NewLine));
                tollPrompt.Append("to do this? Y/N");
            }
            else
            {
                tollPrompt.AppendLine(string.Format("{0}You don't have enough", Environment.NewLine));
                tollPrompt.Append("cash for the toll road.");
            }

            return tollPrompt.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Check if the player has enough monies to pay for the toll road.
            if (!canAffordToll)
            {
                SetForm(typeof(LocationFork));
                return;
            }

            // Depending on player response we will subtract money or continue on trail.
            switch (reponse)
            {
                case DialogResponse.Yes:
                    // Remove monies for the cost of the trip on toll road.
                    OregonTrailApp.Instance.Vehicle.Inventory[Entities.Cash].ReduceQuantity(UserData.Toll.Cost);

                    // Only insert the location if there is one to actually insert.
                    if (UserData.Toll.Road != null)
                        OregonTrailApp.Instance.Trail.InsertLocation(UserData.Toll.Road);

                    // Shutdown the toll road data now that we are done with it.
                    UserData.DestroyToll();

                    // Onward to the next location!
                    SetForm(typeof(LocationDepart));
                    break;
                case DialogResponse.No:
                case DialogResponse.Custom:
                    if (OregonTrailApp.Instance.Trail.CurrentLocation is ForkInRoad)
                        SetForm(typeof(LocationFork));
                    else
                        ClearForm();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reponse", reponse, null);
            }
        }
    }
}