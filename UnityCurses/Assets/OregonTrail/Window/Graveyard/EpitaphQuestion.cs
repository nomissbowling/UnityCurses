﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;

namespace Assets.OregonTrail.Window.Graveyard
{
    /// <summary>
    ///     Asks the user if they would like to write a custom message on their Tombstone for other users to see when the
    ///     come across this part of the trail in the future.
    /// </summary>
    [ParentWindow(typeof(Graveyard))]
    public sealed class EpitaphQuestion : InputForm<TombstoneInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EpitaphQuestion" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public EpitaphQuestion(IWindow window) : base(window)
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
            var epitaphPrompt = new StringBuilder();

            // Add Tombstone message with here lies player name, no epitaph yet.
            epitaphPrompt.Append(string.Format("{0}{1}", Environment.NewLine, UserData.Tombstone));
            epitaphPrompt.AppendLine(string.Format("{0}Would you like to write", Environment.NewLine));
            epitaphPrompt.Append("an epitaph? Y/N");
            return epitaphPrompt.ToString();
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
                case DialogResponse.Yes:
                    SetForm(typeof(EpitaphEditor));
                    break;
                case DialogResponse.No:
                case DialogResponse.Custom:
                    OregonTrailApp.Instance.Tombstone.Add(UserData.Tombstone);
                    SetForm(typeof(TombstoneView));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reponse", reponse, null);
            }
        }
    }
}