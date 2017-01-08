// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using Assets.Maxwolf.WolfCurses.Window.Form;

namespace Assets.Maxwolf.OregonTrail.Window.Travel.Rest
{
    /// <summary>
    ///     Attaches a state that will ask the player how long they would like to rest in the number of days, zero is a valid
    ///     response and will not do anything. If greater than zero we will attach another state to tick that many days by in
    ///     the simulation.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class RestAmount : Form<TravelInfo>
    {
        /// <summary>
        ///     Returns a text only representation of the current game Windows state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string OnRenderForm()
        {
            return Environment.NewLine + "How many days would you like to rest?";
        }

        /// <summary>Fired when the game Windows current state is not null and input buffer does not match any known command.</summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game Windows.</param>
        public override void OnInputBufferReturned(string input)
        {
            // Parse the user input buffer as a unsigned int.
            int parsedInputNumber;
            if (!int.TryParse(input, out parsedInputNumber))
                return;

            // For diversion, or out of necessity, a party can rest from one to nine days.
            UserData.DaysToRest = parsedInputNumber;
            if (parsedInputNumber > 0 && parsedInputNumber <= 9)
                SetForm(typeof(Resting));
            else
                ClearForm();
        }
    }
}