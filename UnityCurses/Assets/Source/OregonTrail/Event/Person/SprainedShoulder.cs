﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Diagnostics.CodeAnalysis;
using Assets.Source.OregonTrail.Event.Prefab;
using Assets.Source.OregonTrail.Module.Director;

namespace Assets.Source.OregonTrail.Event.Person
{
    /// <summary>
    ///     If you have sprained your acromioclavicular joint (the joint at the top of your shoulder), you may be advised to
    ///     avoid activities that involve moving your arm.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class SprainedShoulder : PersonInjure
    {
        /// <summary>Fired after the event has executed and the injury flag set on the person.</summary>
        /// <param name="person">Person whom is now injured by whatever you say they are here.</param>
        /// <returns>Describes what type of physical injury has come to the person.</returns>
        protected override string OnPostInjury(Entity.Person.Person person)
        {
            return string.Format("{0} has a sprained shoulder.", person.Name);
        }
    }
}