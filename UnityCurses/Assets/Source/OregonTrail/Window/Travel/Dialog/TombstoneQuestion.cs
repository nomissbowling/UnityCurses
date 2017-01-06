﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Source.Engine;
using Assets.Source.Engine.Window;
using Assets.Source.Engine.Window.Form;
using Assets.Source.Engine.Window.Form.Input;
using Assets.Source.OregonTrail.Window.Travel.Command;

namespace Assets.Source.OregonTrail.Window.Travel.Dialog
{
    /// <summary>
    ///     Asks the player if they would like to stop and check out a tombstone that is on this particular mile marker.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class TombstoneQuestion : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TombstoneQuestion" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public TombstoneQuestion(IWindow window) : base(window)
        {
        }

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
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            var pointReached = new StringBuilder();

            // Build up message about there being something on the side of the road.
            pointReached.AppendLine(
                string.Format("{0}You pass a gravesite. Would you", Environment.NewLine));
            pointReached.Append("like to look closer? Y/N");

            return pointReached.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Check if the player wants to look at the tombstone or not.
            switch (reponse)
            {
                case DialogResponse.No:
                    SetForm(typeof(ContinueOnTrail));
                    break;
                case DialogResponse.Yes:
                case DialogResponse.Custom:
                    EngineApp.WindowManager.Add(typeof(Graveyard.Graveyard));

                    // Goes back to continue on trail form below us.
                    ClearForm();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reponse", reponse, null);
            }
        }
    }
}