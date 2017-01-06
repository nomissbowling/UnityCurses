// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/16/2016@5:33 PM

using System;
using System.Text;
using Assets.Maxwolf.Example.MainMenu;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Form;

namespace Assets.Maxwolf.Example.CustomInput
{
    /// <summary>
    ///     Asks for user name and then accepts the input from the input buffer.
    /// </summary>
    [ParentWindow(typeof(ExampleWindow))]
    public sealed class DialogCustomInput : Form<ExampleWindowInfo>
    {
        /// <summary>
        ///     Makes it easy to print and manage multiple lines of text.
        /// </summary>
        private StringBuilder _inputNamesHelp;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Form{TData}" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public DialogCustomInput(IWindow window) : base(window)
        {
            _inputNamesHelp = new StringBuilder();
        }

        /// <summary>
        ///     Returns a text only representation of the current game Windows state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        /// <returns>
        ///     The text user interface.<see cref="string" />.
        /// </returns>
        public override string OnRenderForm()
        {
            ParentWindow.PromptText = string.Empty;

            _inputNamesHelp = new StringBuilder();

            _inputNamesHelp.AppendLine(string.Format("{0}Dialog Custom Input{1}", Environment.NewLine,
                Environment.NewLine));
            _inputNamesHelp.Append("What is your name?");

            return _inputNamesHelp.ToString();
        }

        /// <summary>Fired when the game Windows current state is not null and input buffer does not match any known command.</summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game Windows.</param>
        public override void OnInputBufferReturned(string input)
        {
            // Do not allow empty names.
            if (string.IsNullOrEmpty(input))
                return;

            // Copy name into user name and show form.
            UserData.PlayerName = input;
            SetForm(typeof(ShowName));
        }
    }
}