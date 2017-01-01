﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/16/2016@5:33 PM

using System;
using Assets.Scripts.Engine.Window;
using Assets.Scripts.Engine.Window.Form;
using Assets.Scripts.Engine.Window.Form.Input;

namespace Assets.Scripts.Example.Question
{
    [ParentWindow(typeof(ExampleWindow))]
    public sealed class YesWolves : InputForm<ExampleWindowInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InputForm{T}" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public YesWolves(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The dialog prompt text.<see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            return string.Format("{0}You like wolves! Yayyy!{1}", Environment.NewLine, Environment.NewLine);
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            ClearForm();
        }
    }
}