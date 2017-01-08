// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.Travel.RiverCrossing.Help
{
    /// <summary>
    ///     Information about what fording a river means and how it works for the player vehicle and their party members.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class FordRiverHelp : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            var fordRiver = new StringBuilder();
            fordRiver.AppendLine(string.Format("{0}To ford a river means to", Environment.NewLine));
            fordRiver.AppendLine("pull your wagon across a");
            fordRiver.AppendLine("shallow part of the river,");
            fordRiver.AppendLine("with the oxen still");
            fordRiver.AppendLine(string.Format("attached.{0}", Environment.NewLine));
            return fordRiver.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = new CaulkRiverHelp(parentGameMode, UserData);
            SetForm(typeof(CaulkRiverHelp));
        }
    }
}