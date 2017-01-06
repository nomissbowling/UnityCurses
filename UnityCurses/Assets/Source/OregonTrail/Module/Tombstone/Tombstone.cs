﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;

namespace Assets.Source.OregonTrail.Module.Tombstone
{
    /// <summary>
    ///     Facilitates a tombstone base class that supports shallow copies of itself to be created.
    /// </summary>
    public sealed class Tombstone
    {
        private readonly string _playerName;
        private readonly string _tombstoneID;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tombstone" /> class.
        ///     Creates a shallow copy of the tombstone, generates a new tombstone ID in the process.
        /// </summary>
        public Tombstone()
        {
            // Loop through all the vehicle passengers and find the leader.
            foreach (var passenger in OregonTrailApp.Instance.Vehicle.Passengers)
            {
                // Skip if not the leader.
                if (!passenger.Leader)
                    continue;

                // Add the leaders name to the Tombstone header.
                _playerName = passenger.Name;
                break;
            }

            // Grabs the current mile marker where the player died on the trail for the Tombstone to sit at.
            MileMarker = OregonTrailApp.Instance.Vehicle.Odometer;

            // Epitaph is left empty by default and ready to be filled in.
            Epitaph = string.Empty;

            // Init a new ID, even though it is clone of existing tombstone it is a different instance.
            _tombstoneID = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        ///     Name of the player whom died and the Tombstone is paying respects to.
        /// </summary>
        public string PlayerName
        {
            get { return _playerName; }
        }

        /// <summary>
        ///     Determines the unique and randomly generated tombstone identification string. This is created once for every
        ///     tombstone and should be unique to each one, used to check if the tombstone is the same as the one for player who
        ///     recently died so we know if they should be offered a chance to edit it.
        /// </summary>
        private string TombstoneID
        {
            get { return _tombstoneID; }
        }

        /// <summary>
        ///     Defines where on the trail in regards to length in miles traveled. The purpose of this is so we can check for this
        ///     Tombstone in the exact same spot where the player actually died on the trail.
        /// </summary>
        public int MileMarker { get; private set; }

        /// <summary>
        ///     Message that can be included on the Tombstone below the players name. It can only be a few characters long but
        ///     interesting to see what people leave as warnings or just silliness for other travelers.
        /// </summary>
        public string Epitaph { get; set; }

        /// <summary>
        ///     Creates a shallow copy of our tombstone, used to add to list without having direct copy still tied to it.
        /// </summary>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public object Clone()
        {
            var clone = (Tombstone) MemberwiseClone();
            return clone;
        }

        /// <summary>
        ///     Creates a nice formatted version of the Tombstone for use in text renderer.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            // Only print the name if the epitaph is empty or null.
            if (string.IsNullOrEmpty(Epitaph))
                return string.Format("Here lies {0}{1}", PlayerName, Environment.NewLine);

            // Print the name and epitaph message the player left for others to read.
            return string.Format("Here lies {0}{1}", PlayerName, Environment.NewLine) +
                   string.Format("{0}{1}", Epitaph, Environment.NewLine);
        }
    }
}