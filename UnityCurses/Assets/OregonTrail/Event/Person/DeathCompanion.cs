﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.OregonTrail.Module.Director;
using Assets.OregonTrail.Window.RandomEvent;

namespace Assets.OregonTrail.Event.Person
{
    /// <summary>
    ///     Called when one of your party members dies that is not the leader of the group, the game will still be able to
    ///     continue without this person.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class DeathCompanion : EventProduct
    {
        /// <summary>
        ///     The _passenger death.
        /// </summary>
        private StringBuilder _passengerDeath;

        /// <summary>
        ///     Fired when the event is created by the event factory, but before it is executed. Acts as a constructor mostly but
        ///     used in this way so that only the factory will call the method and there is no worry of it accidentally getting
        ///     called by creation.
        /// </summary>
        public override void OnEventCreate()
        {
            base.OnEventCreate();

            _passengerDeath = new StringBuilder();
        }

        /// <summary>
        ///     Fired when the event handler associated with this enum type triggers action on target entity. Implementation is
        ///     left completely up to handler.
        /// </summary>
        /// <param name="eventExecutor">
        ///     Entities which the event is going to directly affect. This way there is no confusion about
        ///     what entity the event is for. Will require casting to correct instance type from interface instance.
        /// </param>
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as a passenger from vehicle.
            var sourcePerson = eventExecutor.SourceEntity as Entity.Person.Person;
            if (sourcePerson == null)
                throw new ArgumentNullException("eventExecutor",
                    "Could not cast source entity as passenger of vehicle.");

            // Check to make sure this player is not the leader (aka the player).
            if (sourcePerson.Leader)
                throw new ArgumentException("Cannot kill this person because it is the player!");

            _passengerDeath.AppendLine(string.Format("{0} has died.", sourcePerson.Name));
        }

        /// <summary>
        ///     Fired when the simulation would like to render the event, typically this is done AFTER executing it but this could
        ///     change depending on requirements of the implementation.
        /// </summary>
        /// <param name="userData"></param>
        /// <returns>Text user interface string that can be used to explain what the event did when executed.</returns>
        protected override string OnRender(RandomEventInfo userData)
        {
            return _passengerDeath.ToString();
        }
    }
}