﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Assets.Maxwolf.Engine;
using Assets.Maxwolf.OregonTrail.Entity;
using Assets.Maxwolf.OregonTrail.Event.Prefab;
using Assets.Maxwolf.OregonTrail.Module.Director;

namespace Assets.Maxwolf.OregonTrail.Event.Wild
{
    /// <summary>
    ///     Robber who can come in the middle of the night and steal things from the vehicle inventory. He is also very
    ///     dangerous and will do whatever it takes to get what he wants, so there is a chance some of your party members may
    ///     get murdered.
    /// </summary>
    [DirectorEvent(EventCategory.Wild, EventExecution.RandomOrManual)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class Thief : ItemDestroyer
    {
        /// <summary>Fired by the item destroyer event prefab before items are destroyed.</summary>
        /// <param name="destroyedItems">Items that were destroyed from the players inventory.</param>
        /// <returns>The <see cref="string" />.</returns>
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Ammo used to kill the thief is randomly generated.
            OregonTrailApp.Instance.Vehicle.Inventory[Entities.Ammo].ReduceQuantity(
                EngineApp.Random.Next(1, 5));

            // Change event text depending on if items were destroyed or not.
            return destroyedItems.Count > 0
                ? TryKillPassengers("murdered")
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
            var theifPrompt = new StringBuilder();
            theifPrompt.AppendLine("Thief comes in the");
            theifPrompt.Append("night resulting in ");
            return theifPrompt.ToString();
        }
    }
}