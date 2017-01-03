﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.OregonTrail.Entity.Location.Weather;

namespace Assets.OregonTrail.Entity.Location.Point
{
    /// <summary>
    ///     Scenic area along the trail where the player can stop and rest, mostly used to signify total progress along the
    ///     entire trail with clear visual markers for the player to recognize.
    /// </summary>
    public sealed class Landmark : Location
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Landmark" /> class. Initializes a new instance of the
        ///     <see cref="T:Assets.OregonTrail.Entity.Location.Location" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="climateType">The climate Type.</param>
        public Landmark(string name, Climate climateType) : base(name, climateType)
        {
        }

        /// <summary>
        ///     Determines if the location allows the player to chat to other NPC's in the area which can offer up advice about the
        ///     trail ahead.
        /// </summary>
        public override bool ChattingAllowed
        {
            get { return false; }
        }

        /// <summary>
        ///     Determines if this location has a store which the player can buy items from using their monies.
        /// </summary>
        public override bool ShoppingAllowed
        {
            get { return false; }
        }
    }
}