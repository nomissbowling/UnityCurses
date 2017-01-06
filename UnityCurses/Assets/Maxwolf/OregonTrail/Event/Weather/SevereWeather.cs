// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using Assets.Maxwolf.OregonTrail.Entity;
using Assets.Maxwolf.OregonTrail.Event.Prefab;
using Assets.Maxwolf.OregonTrail.Module.Director;

namespace Assets.Maxwolf.OregonTrail.Event.Weather
{
    /// <summary>
    ///     Severe weather will cause destruction of items and waste your time, but nobody will get killed.
    /// </summary>
    [DirectorEvent(EventCategory.Weather, EventExecution.ManualOnly)]
    public sealed class SevereWeather : ItemDestroyer
    {
        /// <summary>Fired by the item destroyer event prefab before items are destroyed.</summary>
        /// <param name="destroyedItems">Items that were destroyed from the players inventory.</param>
        /// <returns>The <see cref="string" />.</returns>
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            return destroyedItems.Count > 0
                ? string.Format("time and supplies lost:{0}", Environment.NewLine)
                : "no items lost.";
        }

        /// <summary>
        ///     Fired by the item destroyer event prefab after items are destroyed.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnPreDestroyItems()
        {
            return "heavy rains---";
        }
    }
}