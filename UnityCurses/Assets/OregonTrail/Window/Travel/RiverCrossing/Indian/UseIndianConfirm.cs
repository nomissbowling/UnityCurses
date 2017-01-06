﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;

namespace Assets.OregonTrail.Window.Travel.RiverCrossing.Indian
{
    /// <summary>
    ///     Confirms with the player the decision they made about crossing the riving using the Indian guide in exchange for a
    ///     set amount of clothing. This form we will actually process that transaction and then let the Indian take the player
    ///     across the river like he promised.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class UseIndianConfirm : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UseIndianConfirm" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public UseIndianConfirm(IWindow window) : base(window)
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
            var _prompt = new StringBuilder();
            _prompt.AppendLine(string.Format("{0}The Shoshoni guide will help", Environment.NewLine));
            _prompt.AppendLine(string.Format("you float your wagon across.{0}", Environment.NewLine));
            return _prompt.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Player has enough clothing to satisfy the Indians cost.
            SetForm(typeof(CrossingTick));
        }
    }
}