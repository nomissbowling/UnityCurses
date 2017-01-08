// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.MainMenu.Start_Month
{
    /// <summary>
    ///     Shows the player information about what the various starting months mean.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class StartMonthHelp : InputForm<NewGameInfo>
    {
        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Inform the user about a decision they need to make.
            var _startMonthHelp = new StringBuilder();
            _startMonthHelp.Append(string.Format("{0}You attend a public meeting held{1}", Environment.NewLine,
                Environment.NewLine));
            _startMonthHelp.Append(string.Format("for \"folks with the California -{0}", Environment.NewLine));
            _startMonthHelp.Append(string.Format("Oregon fever.\" You're told:{0}{1}", Environment.NewLine,
                Environment.NewLine));
            _startMonthHelp.Append(string.Format("If you leave too early, there{0}", Environment.NewLine));
            _startMonthHelp.Append(string.Format("won't be any grass for your{0}", Environment.NewLine));
            _startMonthHelp.Append(string.Format("oxen to eat. If you leave too{0}", Environment.NewLine));
            _startMonthHelp.Append(string.Format("late, you may not get to Oregon{0}", Environment.NewLine));
            _startMonthHelp.Append(string.Format("before winter comes. If you{0}", Environment.NewLine));
            _startMonthHelp.Append(string.Format("leave at just the right time,{0}", Environment.NewLine));
            _startMonthHelp.Append(string.Format("there will be green grass and{0}", Environment.NewLine));
            _startMonthHelp.Append(string.Format("the weather will still be cool.{0}{1}", Environment.NewLine,
                Environment.NewLine));
            return _startMonthHelp.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = new SelectStartingMonthState(parentGameMode, UserData);
            SetForm(typeof(SelectStartingMonthState));
        }
    }
}