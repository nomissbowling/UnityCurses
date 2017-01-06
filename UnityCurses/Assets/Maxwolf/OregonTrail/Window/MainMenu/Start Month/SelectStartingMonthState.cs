﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Maxwolf.OregonTrail.Module.Time;
using Assets.Maxwolf.OregonTrail.Window.MainMenu.Help;
using Assets.Maxwolf.ProjectCommon.Utility;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Form;

namespace Assets.Maxwolf.OregonTrail.Window.MainMenu.Start_Month
{
    /// <summary>
    ///     Offers the player the ability to change the starting month of the simulation, this affects how many resources will
    ///     be available to them and the severity of the random events they encounter along the trail.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class SelectStartingMonthState : Form<NewGameInfo>
    {
        /// <summary>
        ///     References the string representing the question about starting month, only builds it once and holds in memory while
        ///     state is active.
        /// </summary>
        private StringBuilder _startMonthQuestion;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SelectStartingMonthState" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public SelectStartingMonthState(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Determines if user input is currently allowed to be typed and filled into the input buffer.
        /// </summary>
        /// <remarks>Default is FALSE. Setting to TRUE allows characters and input buffer to be read when submitted.</remarks>
        public override bool InputFillsBuffer
        {
            get { return true; }
        }

        /// <summary>
        ///     Fired after the state has been completely attached to the simulation letting the state know it can browse the user
        ///     data and other properties below it.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Pass the game data to the simulation for each new game Windows state.
            OregonTrailApp.Instance.SetStartInfo(UserData);

            // Tell the user they need to make a decision.
            _startMonthQuestion = new StringBuilder();
            _startMonthQuestion.AppendLine(string.Format("{0}It is 1848. Your jumping off place", Environment.NewLine));
            _startMonthQuestion.AppendLine("for Oregon is Independence, Missouri.");
            _startMonthQuestion.AppendLine("You must decide which month");
            _startMonthQuestion.AppendLine(string.Format("to leave Independence{0}", Environment.NewLine));

            // Loop through every possible starting month and list them out by their enumeration integer values along with description attribute.
            var choices = new List<StartingMonth>(Enum.GetValues(typeof(StartingMonth)).Cast<StartingMonth>());
            for (var index = 0; index < choices.Count; index++)
            {
                // Get the current river choice enumeration value we casted into list.
                var monthValue = choices[index];

                // Last line should not print new line.
                if (index == choices.Count - 1)
                {
                    _startMonthQuestion.AppendLine(string.Format("  {0}. {1}", (int) monthValue, monthValue));
                    _startMonthQuestion.Append(string.Format("  {0}. Ask for advice", choices.Count + 1));
                }
                else
                {
                    _startMonthQuestion.AppendLine(string.Format("  {0}. {1}", (int) monthValue, monthValue));
                }
            }
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
            return _startMonthQuestion.ToString();
        }

        /// <summary>Fired when the game Windows current state is not null and input buffer does not match any known command.</summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game Windows.</param>
        public override void OnInputBufferReturned(string input)
        {
            // Skip if the input is null or empty.
            if (string.IsNullOrEmpty(input))
                return;

            // Attempt to cast string to enum value, can be characters or integer.
#pragma warning disable IDE0018 // Inline variable declaration
            StartingMonth startMonth;
#pragma warning restore IDE0018 // Inline variable declaration
            if (!EnumExtensions.TryParse(input, out startMonth))
                return;

            // Depending on what was selected we will set starting month to correct one in full listing, or show advice to player.
            switch (startMonth)
            {
                case StartingMonth.March:
                    UserData.StartingMonth = Month.March;
                    SetForm(typeof(InitialItemsHelp));
                    break;
                case StartingMonth.April:
                    UserData.StartingMonth = Month.April;
                    SetForm(typeof(InitialItemsHelp));
                    break;
                case StartingMonth.May:
                    UserData.StartingMonth = Month.May;
                    SetForm(typeof(InitialItemsHelp));
                    break;
                case StartingMonth.June:
                    UserData.StartingMonth = Month.June;
                    SetForm(typeof(InitialItemsHelp));
                    break;
                case StartingMonth.July:
                    UserData.StartingMonth = Month.July;
                    SetForm(typeof(InitialItemsHelp));
                    break;
                default:


// Shows information about what the different starting months mean.
                    SetForm(typeof(StartMonthHelp));
                    break;
            }
        }
    }
}