// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Diagnostics.CodeAnalysis;
using Assets.Source.OregonTrail.Event.Prefab;
using Assets.Source.OregonTrail.Module.Director;

namespace Assets.Source.OregonTrail.Event.Person
{
    /// <summary>
    ///     Localized death and decomposition of body tissue, resulting from either obstructed circulation or bacterial
    ///     infection.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class Gangrene : PersonInfect
    {
        /// <summary>Fired after the event has executed and the infection flag set on the person.</summary>
        /// <param name="person">Person whom is now infected by whatever you say they are here.</param>
        /// <returns>Name or type of infection the person is currently affected with.</returns>
        protected override string OnPostInfection(Entity.Person.Person person)
        {
            return string.Format("{0} has gangrene.", person.Name);
        }
    }
}