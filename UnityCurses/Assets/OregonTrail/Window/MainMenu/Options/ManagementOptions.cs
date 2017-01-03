﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Reflection;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Form;

namespace Assets.OregonTrail.Window.MainMenu.Options
{
    /// <summary>
    ///     Glorified options menu for the game that allows player to remove top ten high scores, remove saved games, erase
    ///     Tombstone messages, etc.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class ManagementOptions : Form<NewGameInfo>
    {
        /// <summary>
        ///     Holds options menu so it will only be created once and then rendered out.
        /// </summary>
        private StringBuilder _optionsPrompt;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ManagementOptions" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public ManagementOptions(IWindow window) : base(window)
        {
            _optionsPrompt = new StringBuilder();
        }

        /// <summary>
        ///     Fired after the state has been completely attached to the simulation letting the state know it can browse the user
        ///     data and other properties below it.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            _optionsPrompt.AppendLine(string.Format("{0}The Oregon Trail", Environment.NewLine));
            _optionsPrompt.AppendLine(
                string.Format("Version: {0}{1}", Assembly.GetEntryAssembly().GetName().Version, Environment.NewLine));
            _optionsPrompt.AppendLine(string.Format("Management Options{0}", Environment.NewLine));
            _optionsPrompt.AppendLine("You may:");
            _optionsPrompt.AppendLine("1. See the original Top Ten list");
            _optionsPrompt.AppendLine("2. Erase the current Top Ten list");
            _optionsPrompt.AppendLine("3. Erase the Tombstone messages");
            _optionsPrompt.Append("4. Return to the main menu");
        }

        /// <summary>
        ///     Returns a text only representation of the current game Windows state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string OnRenderForm()
        {
            return _optionsPrompt.ToString();
        }

        /// <summary>Fired when the game Windows current state is not null and input buffer does not match any known command.</summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game Windows.</param>
        public override void OnInputBufferReturned(string input)
        {
            // Convert input into a number, otherwise just return.
#pragma warning disable IDE0018 // Inline variable declaration
            int inputNumber;
#pragma warning restore IDE0018 // Inline variable declaration
            if (!int.TryParse(input, out inputNumber))
                return;

            // Depending on what number we got we might do something.
            switch (inputNumber)
            {
                case 1:


// See the original Top Ten list.
                    SetForm(typeof(OriginalTopTen));
                    break;
                case 2:


// Erase the current Top Ten list.
                    SetForm(typeof(EraseCurrentTopTen));
                    break;
                case 3:


// Erase the Tombstone messages.
                    SetForm(typeof(EraseTombstone));
                    break;
                case 4:


// Return to the main menu.
                    ClearForm();
                    break;
            }
        }
    }
}