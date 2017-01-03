﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Control;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;
using Assets.OregonTrail.Module.Scoring;

namespace Assets.OregonTrail.Window.MainMenu.Options
{
    /// <summary>
    ///     Shows the player hard-coded top ten list as it is known internally in static list.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class OriginalTopTen : InputForm<NewGameInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OriginalTopTen" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public OriginalTopTen(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            var sourceTopTen = new StringBuilder();

            // Text above the table to declare what this state is.
            sourceTopTen.AppendLine(string.Format("{0}The Oregon Top Ten{1}", Environment.NewLine, Environment.NewLine));

            // Init text table representation of default high score list.
            var table = ScoringModule.DefaultTopTen.ToStringTable(
                u => u.Name,
                u => u.Points,
                u => u.Rating);
            sourceTopTen.AppendLine(table);
            return sourceTopTen.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(ManagementOptions));
        }
    }
}