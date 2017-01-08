// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.OregonTrail.Window.Travel.Command;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.Travel.Dialog
{
    /// <summary>
    ///     Attached when the player wants to continue on the trail, and doing so will force them to leave that point and be
    ///     back on the trail counting up distance traveled until they reach the next one. The purpose of this state is to
    ///     inform the player of the next points name, the distance away that it is, and that is all it will close and
    ///     simulation resume after return key is pressed.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class LocationDepart : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Tell player how far it is to next location before attaching drive state.
            var prompt = new StringBuilder();
            var nextPoint = OregonTrailApp.Instance.Trail.NextLocation;
            prompt.AppendLine(
                string.Format("{0}From {1} it is {2}", Environment.NewLine,
                    OregonTrailApp.Instance.Trail.CurrentLocation.Name,
                    OregonTrailApp.Instance.Trail.DistanceToNextLocation));
            prompt.AppendLine(string.Format("miles to {0}{1}", nextPoint.Name, Environment.NewLine));
            return prompt.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(ContinueOnTrail));
        }
    }
}