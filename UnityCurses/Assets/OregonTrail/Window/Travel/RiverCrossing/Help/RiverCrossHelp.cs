﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;

namespace Assets.OregonTrail.Window.Travel.RiverCrossing.Help
{
    /// <summary>
    ///     Shown to the user the first time they cross a river, this way it can be explained to them they must cross it in
    ///     order to continue and there is no going around. We tell them how deep the water is and how many feed across the
    ///     river is they will need to travel.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class RiverCrossHelp : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RiverCrossHelp" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public RiverCrossHelp(IWindow window) : base(window)
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
            // Generates a new river with randomized width and depth.
            UserData.GenerateRiver();

            var riverPrompt = new StringBuilder();
            riverPrompt.AppendLine(string.Format("{0}You must cross the river in", Environment.NewLine));
            riverPrompt.AppendLine("order to continue. The");
            riverPrompt.AppendLine("river at this point is");
            riverPrompt.AppendLine(string.Format("currently {0} feet across,", UserData.River.RiverWidth));
            riverPrompt.AppendLine(string.Format("and {0} feet deep in the", UserData.River.RiverDepth));
            riverPrompt.AppendLine(string.Format("middle.{0}", Environment.NewLine));
            return riverPrompt.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(RiverCross));
        }
    }
}