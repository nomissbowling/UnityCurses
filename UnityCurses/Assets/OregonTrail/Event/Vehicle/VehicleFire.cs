﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Assets.OregonTrail.Entity;
using Assets.OregonTrail.Event.Prefab;
using Assets.OregonTrail.Module.Director;

namespace Assets.OregonTrail.Event.Vehicle
{
    /// <summary>
    ///     Fire in the vehicle occurs, there is a chance that some of the inventory items or people were burned to death.
    /// </summary>
    [DirectorEvent(EventCategory.Vehicle, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class VehicleFire : ItemDestroyer
    {
        /// <summary>Fired by the item destroyer event prefab before items are destroyed.</summary>
        /// <param name="destroyedItems">Items that were destroyed from the players inventory.</param>
        /// <returns>The <see cref="string" />.</returns>
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Change event text depending on if items were destroyed or not.
            return destroyedItems.Count > 0
                ? TryKillPassengers("burned")
                : "no loss of items.";
        }

        /// <summary>
        ///     Fired by the item destroyer event prefab after items are destroyed.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnPreDestroyItems()
        {
            var firePrompt = new StringBuilder();
            firePrompt.AppendLine("Fire in the wagon!");
            firePrompt.Append("Resulting in ");
            return firePrompt.ToString();
        }
    }
}