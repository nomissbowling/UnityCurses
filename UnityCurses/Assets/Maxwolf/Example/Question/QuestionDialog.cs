﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/16/2016@5:32 PM

using System;
using System.Text;
using Assets.Maxwolf.Example.MainMenu;
using Assets.Maxwolf.WolfCurses;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.Example.Question
{
    /// <summary>
    ///     Asks the user a yes/no based question, they can only reply with those predetermined answers.
    /// </summary>
    [ParentWindow(typeof(ExampleWindow))]
    public sealed class QuestionDialog : InputForm<ExampleWindowInfo>
    {
        /// <summary>
        ///     Holds all the text so we only need to render it once.
        /// </summary>
        private StringBuilder dialogYesNo = new StringBuilder();

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
        ///     The dialog prompt text.<see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            ParentWindow.PromptText = SceneGraph.PROMPT_TEXT_DEFAULT;

            dialogYesNo = new StringBuilder();

            dialogYesNo.AppendLine(string.Format("{0}Question Dialog Example{1}", Environment.NewLine,
                Environment.NewLine));
            dialogYesNo.Append("Do you like wolves? Y/N");

            return dialogYesNo.ToString();
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
                case DialogResponse.Custom:
                case DialogResponse.No:
                    SetForm(typeof(NoWolves));
                    break;
                case DialogResponse.Yes:
                    SetForm(typeof(YesWolves));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reponse", reponse, null);
            }
        }
    }
}