﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine;
using Assets.Engine.Window;
using Assets.OregonTrail.Window.MainMenu.Help;
using Assets.OregonTrail.Window.MainMenu.Options;
using Assets.OregonTrail.Window.MainMenu.Profession;

namespace Assets.OregonTrail.Window.MainMenu
{
    /// <summary>
    ///     Allows the configuration of party names, player profession, and purchasing initial items for trip.
    /// </summary>
    public sealed class MainMenu : Window<MainMenuCommands, NewGameInfo>
    {
        /// <summary>
        ///     Asked for the first party member.
        /// </summary>
        public const string LEADER_QUESTION = "What is the first name of the wagon leader?";

        /// <summary>
        ///     Asked for every other party member name we want to collect.
        /// </summary>
        public static readonly string MEMBERS_QUESTION =
            string.Format("What are the first names of the{0}three other members in your party?", Environment.NewLine);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Window{TCommands,TData}" /> class.
        /// </summary>
        /// <param name="simUnit">Core simulation which is controlling the form factory.</param>
        public MainMenu(EngineApp simUnit) : base(simUnit)
        {
        }

        /// <summary>
        ///     Called after the Windows has been added to list of modes and made active.
        /// </summary>
        public override void OnWindowPostCreate()
        {
            var headerText = new StringBuilder();
            headerText.Append(string.Format("{0}The Oregon Trail{1}{2}", Environment.NewLine, Environment.NewLine,
                Environment.NewLine));
            headerText.Append("You may:");
            MenuHeader = headerText.ToString();

            AddCommand(TravelTheTrail, MainMenuCommands.TravelTheTrail);
            AddCommand(LearnAboutTrail, MainMenuCommands.LearnAboutTheTrail);
            AddCommand(SeeTopTen, MainMenuCommands.SeeTheOregonTopTen);
            AddCommand(ChooseManagementOptions, MainMenuCommands.ChooseManagementOptions);
            AddCommand(CloseSimulation, MainMenuCommands.CloseSimulation);
        }

        /// <summary>
        ///     Does exactly what it says on the tin, closes the simulation and releases all memory.
        /// </summary>
        private static void CloseSimulation()
        {
            EngineApp.Shutdown();
        }

        /// <summary>
        ///     Glorified options menu, used to clear top ten, Tombstone messages, and saved games.
        /// </summary>
        private void ChooseManagementOptions()
        {
            SetForm(typeof(ManagementOptions));
        }

        /// <summary>
        ///     High score list, defaults to hard-coded values if no custom ones present.
        /// </summary>
        private void SeeTopTen()
        {
            SetForm(typeof(CurrentTopTen));
        }

        /// <summary>
        ///     Instruction manual that explains how the game works and what is expected of the player.
        /// </summary>
        private void LearnAboutTrail()
        {
            SetForm(typeof(RulesHelp));
        }

        /// <summary>
        ///     Start with choosing profession in the new game Windows, the others are chained together after this one.
        /// </summary>
        private void TravelTheTrail()
        {
            SetForm(typeof(ProfessionSelector));
        }
    }
}