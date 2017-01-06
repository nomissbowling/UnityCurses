﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Maxwolf.OregonTrail.Module.Tombstone;
using Assets.Maxwolf.WolfCurses.Window;

namespace Assets.Maxwolf.OregonTrail.Window.Graveyard
{
    /// <summary>
    ///     Holds intermediate information about tombstone for editing purposes. Eventually the data edited here will be passed
    ///     off to the graveyard module which handles the archiving of tombstones.
    /// </summary>
    public sealed class TombstoneInfo : WindowData
    {
        /// <summary>
        ///     Tombstone for the player (or another dead player) that will be either viewed or shown to user so they can
        ///     confirm their graves details.
        /// </summary>
        private Tombstone tempTombstone;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TombstoneInfo" /> class.
        /// </summary>
        public TombstoneInfo()
        {
            tempTombstone = null;
        }

        /// <summary>
        ///     Creates a new internally workable tombstone that can be accessed by the running window and form logic. Intended to
        ///     be used to create a tombstone for a player that has failed to reach the end of the trail.
        /// </summary>
        public Tombstone Tombstone
        {
            get
            {
                // Init a tombstone if one does not exist, otherwise reference existing one.
                return tempTombstone ?? (tempTombstone = new Tombstone());
            }
        }

        /// <summary>
        ///     Destroys any currently existing temporary tombstone.
        /// </summary>
        public void ClearTombstone()
        {
            tempTombstone = null;
        }
    }
}