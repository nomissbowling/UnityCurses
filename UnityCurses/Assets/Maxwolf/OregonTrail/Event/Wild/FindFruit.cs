﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Diagnostics.CodeAnalysis;
using Assets.Maxwolf.OregonTrail.Entity;
using Assets.Maxwolf.OregonTrail.Module.Director;
using Assets.Maxwolf.OregonTrail.Window.RandomEvent;

namespace Assets.Maxwolf.OregonTrail.Event.Wild
{
    /// <summary>
    ///     Similar to wild berries, but with fruit there will be more to go around.
    /// </summary>
    [DirectorEvent(EventCategory.Wild, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class FindFruit : EventProduct
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
            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            if (vehicle != null)
                vehicle.Inventory[Entities.Food].AddQuantity(17);
        }

        /// <summary>
        ///     Fired when the simulation would like to render the event, typically this is done AFTER executing it but this could
        ///     change depending on requirements of the implementation.
        /// </summary>
        /// <param name="userData"></param>
        /// <returns>Text user interface string that can be used to explain what the event did when executed.</returns>
        protected override string OnRender(RandomEventInfo userData)
        {
            return "Find wild fruit.";
        }
    }
}