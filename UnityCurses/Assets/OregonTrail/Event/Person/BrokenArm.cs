﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Diagnostics.CodeAnalysis;
using Assets.OregonTrail.Event.Prefab;
using Assets.OregonTrail.Module.Director;

namespace Assets.OregonTrail.Event.Person
{
    /// <summary>
    ///     One of the members of the vehicle passenger manifest broke their arm somehow.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class BrokenArm : PersonInjure
    {
        /// <summary>Fired after the event has executed and the injury flag set on the person.</summary>
        /// <param name="person">Person whom is now injured by whatever you say they are here.</param>
        /// <returns>Describes what type of physical injury has come to the person.</returns>
        protected override string OnPostInjury(Entity.Person.Person person)
        {
            return string.Format("{0} has broken their arm.", person.Name);
        }
    }
}