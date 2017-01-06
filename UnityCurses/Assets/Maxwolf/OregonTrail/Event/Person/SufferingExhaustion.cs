// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Diagnostics.CodeAnalysis;
using Assets.Maxwolf.OregonTrail.Event.Prefab;
using Assets.Maxwolf.OregonTrail.Module.Director;

namespace Assets.Maxwolf.OregonTrail.Event.Person
{
    /// <summary>
    ///     A person with weakness or fatigue lacks energy, feels weary, and is constantly tired.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class SufferingExhaustion : PersonInjure
    {
        /// <summary>Fired after the event has executed and the injury flag set on the person.</summary>
        /// <param name="person">Person whom is now injured by whatever you say they are here.</param>
        /// <returns>Describes what type of physical injury has come to the person.</returns>
        protected override string OnPostInjury(Entity.Person.Person person)
        {
            return string.Format("{0} is suffering from exhaustion.", person.Name);
        }
    }
}