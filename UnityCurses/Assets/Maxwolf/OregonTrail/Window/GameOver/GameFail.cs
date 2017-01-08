﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Maxwolf.Engine;
using Assets.Maxwolf.WolfCurses.Window.Form;

namespace Assets.Maxwolf.OregonTrail.Window.GameOver
{
    /// <summary>
    ///     Fired when the simulation has determined the player has died. It specifically only attaches at this time. The flow
    ///     for death like this is to first show the player the failure state like this, then ask if they want to leave an
    ///     epitaph, process that decision, confirm it, and finally show the viewer that will also show the reason why the
    ///     player died using description attribute from an enumeration value that determines how they died.
    /// </summary>
    [ParentWindow(typeof(GameOver))]
    public sealed class GameFail : Form<GameOverInfo>
    {
        /// <summary>
        ///     Determines if user input is currently allowed to be typed and filled into the input buffer.
        /// </summary>
        /// <remarks>Default is FALSE. Setting to TRUE allows characters and input buffer to be read when submitted.</remarks>
        public override bool InputFillsBuffer
        {
            get { return false; }
        }

        /// <summary>
        ///     Determines if this dialog state is allowed to receive any input at all, even empty line returns. This is useful for
        ///     preventing the player from leaving a particular dialog until you are ready or finished processing some data.
        /// </summary>
        public override bool AllowInput
        {
            get { return false; }
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
            // Jump right to tombstone game window, it will reset the game.
            EngineApp.WindowManager.Add(typeof(Graveyard.Graveyard));
            return string.Empty;
        }

        /// <summary>Fired when the game Windows current state is not null and input buffer does not match any known command.</summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game Windows.</param>
        public override void OnInputBufferReturned(string input)
        {
        }
    }
}