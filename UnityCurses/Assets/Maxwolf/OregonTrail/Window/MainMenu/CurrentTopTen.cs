// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.OregonTrail.Window.MainMenu.Help;
using Assets.Maxwolf.WolfCurses.Window.Control;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.MainMenu
{
    /// <summary>
    ///     References the top ten players in regards to final score they earned at the end of the game, this list is by
    ///     default hard-coded by players have the chance to save their own scores to the list if they beat the default values.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class CurrentTopTen : InputForm<NewGameInfo>
    {
        /// <summary>
        ///     Defines what type of dialog this will act like depending on this enumeration value. Up to implementation to define
        ///     desired behavior.
        /// </summary>
        protected override DialogType DialogType
        {
            get { return DialogType.YesNo; }
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            var currentTopTen = new StringBuilder();

            // Text above the table to declare what this state is.
            currentTopTen.Append(string.Format("{0}Current Top Ten List{1}{2}", Environment.NewLine, Environment.NewLine,
                Environment.NewLine));

            // Init text table representation of default high score list.
            var table = OregonTrailApp.Instance.Scoring.TopTen.ToStringTable(
                u => u.Name,
                u => u.Points,
                u => u.Rating);
            currentTopTen.AppendLine(table);

            // Question about viewing point distribution information.
            currentTopTen.Append(string.Format("Would you like to see how{0}", Environment.NewLine));
            currentTopTen.Append("points are earned? Y/N");
            return currentTopTen.ToString();
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
                case DialogResponse.No:
                    ClearForm();
                    break;
                case DialogResponse.Yes:
                    SetForm(typeof(PointsDistributionHelp));
                    break;
                case DialogResponse.Custom:
                    ClearForm();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reponse", reponse, null);
            }
        }
    }
}