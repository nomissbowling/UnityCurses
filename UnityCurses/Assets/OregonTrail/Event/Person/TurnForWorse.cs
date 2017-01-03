﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.OregonTrail.Module.Director;
using Assets.OregonTrail.Window.RandomEvent;

namespace Assets.OregonTrail.Event.Person
{
    /// <summary>
    ///     To start to get worse. It appeared that person was going to get well; then, unfortunately, they took a turn for the
    ///     worse.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class TurnForWorse : EventProduct
    {
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
            // Cast the source entity as person.
            var person = eventExecutor.SourceEntity as Entity.Person.Person;

            // We are going to inflict enough damage to probably kill the person.
            if (person != null)
                person.Damage(100);
        }

        /// <summary>
        ///     Fired when the simulation would like to render the event, typically this is done AFTER executing it but this could
        ///     change depending on requirements of the implementation.
        /// </summary>
        /// <param name="userData"></param>
        /// <returns>Text user interface string that can be used to explain what the event did when executed.</returns>
        protected override string OnRender(RandomEventInfo userData)
        {
            // Cast the source entity as person.
            var person = userData.SourceEntity as Entity.Person.Person;

            // Skip if the source entity is not a person.
            return person == null
                ? "Nobody has taken a turn for the worse."
                : string.Format("{0} has taken a turn for the worse.", person.Name);
        }
    }
}