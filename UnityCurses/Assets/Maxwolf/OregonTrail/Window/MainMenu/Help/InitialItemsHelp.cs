// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.MainMenu.Help
{
    /// <summary>
    ///     Spawns a new game Windows in the game simulation while maintaining the state of previous one so when we bounce back
    ///     we
    ///     can move from here to next state.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class InitialItemsHelp : InputForm<NewGameInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InitialItemsHelp" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public InitialItemsHelp(IWindow window) : base(window)
        {
            // Pass the game data to the simulation for each new game Windows state.
            OregonTrailApp.Instance.SetStartInfo(UserData);
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Init text we will display to user about the store before they actually load that game Windows.
            var _storeHelp = new StringBuilder();
            _storeHelp.AppendLine(string.Format("{0}Before leaving Independence you", Environment.NewLine));
            _storeHelp.AppendLine("should buy equipment and");
            _storeHelp.AppendLine(string.Format("supplies. You have {0:C2} in", UserData.StartingMonies));
            _storeHelp.AppendLine("cash, but you don't have to");
            _storeHelp.AppendLine(string.Format("spend it all now.{0}", Environment.NewLine));
            return _storeHelp.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(StoreHelp));
        }
    }
}