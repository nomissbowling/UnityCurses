﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;

namespace Assets.OregonTrail.Window.Travel.Store.Help
{
    /// <summary>
    ///     Informs the player they need to purchase at least a single one of the specified SimItem in order to
    ///     continue. This is used in the new game Windows to force the player to have at least one oxen to pull their vehicle
    ///     in
    ///     order to start the simulation.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class RequiredItem : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RequiredItem" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public RequiredItem(IWindow window) : base(window)
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
            var missingItem = new StringBuilder();
            missingItem.AppendLine(
                string.Format("{0}You need to purchase at {1}", Environment.NewLine, Environment.NewLine) +
                string.Format("least a single {0} in order {1}", UserData.Store.SelectedItem.DelineatingUnit,
                    Environment.NewLine) +
                string.Format("to begin your trip!{0}", Environment.NewLine));

            return missingItem.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            UserData.Store.SelectedItem = null;
            SetForm(typeof(Store));
        }
    }
}