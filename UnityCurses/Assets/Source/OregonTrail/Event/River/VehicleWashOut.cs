﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Collections.Generic;
using System.Text;
using Assets.Source.Engine;
using Assets.Source.OregonTrail.Entity;
using Assets.Source.OregonTrail.Event.Prefab;
using Assets.Source.OregonTrail.Module.Director;
using Assets.Source.OregonTrail.Window.RandomEvent;

namespace Assets.Source.OregonTrail.Event.River
{
    /// <summary>
    ///     Player forded the river and it was to deep, they have been washed out by the current and some items destroyed.
    /// </summary>
    [DirectorEvent(EventCategory.RiverCross, EventExecution.ManualOnly)]
    public sealed class VehicleWashOut : ItemDestroyer
    {
        /// <summary>Fired by the item destroyer event prefab before items are destroyed.</summary>
        /// <param name="destroyedItems">Items that were destroyed from the players inventory.</param>
        /// <returns>The <see cref="string" />.</returns>
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            return destroyedItems.Count > 0
                ? TryKillPassengers("drowned")
                : "no loss of items.";
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
            base.Execute(eventExecutor);

            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            // Reduce the total possible mileage of the vehicle this turn.
            if (vehicle != null)
                vehicle.ReduceMileage(20 - 20 * EngineApp.Random.Next());
        }

        /// <summary>
        ///     Fired by the item destroyer event prefab after items are destroyed.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnPreDestroyItems()
        {
            var _eventText = new StringBuilder();
            _eventText.AppendLine("Vehicle was washed");
            _eventText.AppendLine("out when attempting to");
            _eventText.Append("ford the river results");
            return _eventText.ToString();
        }
    }
}