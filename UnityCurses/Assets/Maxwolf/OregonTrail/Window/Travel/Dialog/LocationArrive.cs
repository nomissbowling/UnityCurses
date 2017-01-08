// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.OregonTrail.Entity.Vehicle;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.Travel.Dialog
{
    /// <summary>
    ///     State that is attached when the event is fired for reaching a new point of interest on the trail. Default action is
    ///     to ask the player if they would like to look around, but there is a chance for this behavior to be overridden in
    ///     the constructor there is a default boolean value to skip the question asking part and force a look around event to
    ///     occur without player consent.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class LocationArrive : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Defines what type of dialog this will act like depending on this enumeration value. Up to implementation to define
        ///     desired behavior.
        /// </summary>
        protected override DialogType DialogType
        {
            get
            {
                // First location we only tell the user they are going back in time and arrived at first location on trail.
                return OregonTrailApp.Instance.Trail.IsFirstLocation
                    ? DialogType.Prompt
                    : DialogType.YesNo;
            }
        }

        /// <summary>
        ///     Fired after the state has been completely attached to the simulation letting the state know it can browse the user
        ///     data and other properties below it.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Vehicle is stopped when you are looking around.
            OregonTrailApp.Instance.Vehicle.Status = VehicleStatus.Stopped;
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Grab instance of game simulation for easy reading.
            var game = OregonTrailApp.Instance;
            var pointReached = new StringBuilder();

            // Build up representation of arrival to new location, depending on location it can change.
            if (game.Trail.IsFirstLocation)
            {
                // First point of interest has slightly different message about time travel.
                pointReached.AppendLine(
                    string.Format("{0}Going back to {1}...{2}", Environment.NewLine, game.Time.CurrentYear,
                        Environment.NewLine));
            }
            else if (game.Trail.LocationIndex < game.Trail.Locations.Count)
            {
                // Build up message about location the player is arriving at.
                pointReached.AppendLine(
                    string.Format("{0}You are now at the {1}.", Environment.NewLine, game.Trail.CurrentLocation.Name));
                pointReached.Append("Would you like to look around? Y/N");
            }

            // Wait for input on deciding if we should take a look around.
            return pointReached.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // First location we will always clear state back to location since it is starting point.
            if (OregonTrailApp.Instance.Trail.IsFirstLocation)
            {
                ClearForm();
                return;
            }

            // Subsequent locations will confirm what the player wants to do, they can stop or keep going on the trail at their own demise.
            switch (reponse)
            {
                case DialogResponse.Custom:
                case DialogResponse.No:
                    var travelMode = ParentWindow as Travel;
                    if (travelMode == null)
                        throw new InvalidCastException(
                            "Unable to cast parent game Windows into travel game Windows when it should be that!");

                    // Call the continue on trail method command inside that game Windows, it will trigger the next action accordingly.
                    travelMode.ContinueOnTrail();
                    break;
                case DialogResponse.Yes:
                    ClearForm();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reponse", reponse, null);
            }
        }
    }
}