﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Diagnostics.CodeAnalysis;
using Assets.Maxwolf.Engine;
using Assets.Maxwolf.OregonTrail.Event.Prefab;
using Assets.Maxwolf.OregonTrail.Module.Director;

namespace Assets.Maxwolf.OregonTrail.Event.Vehicle
{
    /// <summary>
    ///     Wastes the players time by forcing them to go around a section of the trail that has been blocked by some natural
    ///     and or man made obstruction.
    /// </summary>
    [DirectorEvent(EventCategory.Vehicle, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ImpassableTrail : LoseTime
    {
        /// <summary>
        ///     Grabs the correct number of days that should be skipped by the lose time event. The event skip day form that
        ///     follows will count down the number of days to zero before letting the player continue.
        /// </summary>
        /// <returns>Number of days that should be skipped in the simulation.</returns>
        protected override int DaysToSkip()
        {
            return EngineApp.Random.Next(2, 6);
        }

        /// <summary>
        ///     Defines the string that will be used to define the event and how it affects the user. It will automatically append
        ///     the number of days lost and count them down this only wants the text that days what the player lost the days
        ///     because of.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnLostTimeReason()
        {
            return "Impassable trail--lose time going around.";
        }
    }
}