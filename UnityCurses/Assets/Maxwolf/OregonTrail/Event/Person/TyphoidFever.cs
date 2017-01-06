// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Diagnostics.CodeAnalysis;
using Assets.Maxwolf.OregonTrail.Event.Prefab;
using Assets.Maxwolf.OregonTrail.Module.Director;

namespace Assets.Maxwolf.OregonTrail.Event.Person
{
    /// <summary>
    ///     An infectious bacterial fever with an eruption of red spots on the chest and abdomen and severe intestinal
    ///     irritation.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class TyphoidFever : PersonInfect
    {
        /// <summary>Fired after the event has executed and the infection flag set on the person.</summary>
        /// <param name="person">Person whom is now infected by whatever you say they are here.</param>
        /// <returns>Name or type of infection the person is currently affected with.</returns>
        protected override string OnPostInfection(Entity.Person.Person person)
        {
            return string.Format("{0} has typhoid fever.", person.Name);
        }
    }
}