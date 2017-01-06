// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Assets.Maxwolf.OregonTrail.Entity;
using Assets.Maxwolf.OregonTrail.Event.Prefab;
using Assets.Maxwolf.OregonTrail.Module.Director;

namespace Assets.Maxwolf.OregonTrail.Event.Wild
{
    /// <summary>
    ///     Discover a vehicle on the side of the road that might have some items inside of it that will be added to the
    ///     players inventory.
    /// </summary>
    [DirectorEvent(EventCategory.Wild, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class AbandonedVehicle : ItemCreator
    {
        /// <summary>Fired by the event prefab after the event has executed.</summary>
        /// <param name="createdItems"></param>
        /// <returns>The <see cref="string" />.</returns>
        protected override string OnPostCreateItems(IDictionary<Entities, int> createdItems)
        {
            return createdItems.Count > 0 ? string.Format("and find:{0}", Environment.NewLine) : "but it is empty";
        }

        /// <summary>
        ///     Fired by the event prefab before the event has executed.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnPreCreateItems()
        {
            var _eventText = new StringBuilder();
            _eventText.AppendLine("You find an abandoned wagon,");
            return _eventText.ToString();
        }
    }
}