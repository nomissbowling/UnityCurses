﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Source.OregonTrail.Entity.Location.Weather;

namespace Assets.Source.OregonTrail.Entity.Location.Point
{
    /// <summary>
    ///     Civilized area where many other people from different vehicles congregate together and share resources.
    /// </summary>
    public sealed class Settlement : Location
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Settlement" /> class. Initializes a new instance of the
        ///     <see cref="T:Assets.Source.OregonTrail.Entity.Location.Location" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="climateType">The climate Type.</param>
        public Settlement(string name, Climate climateType) : base(name, climateType)
        {
        }

        /// <summary>
        ///     Determines if the location allows the player to chat to other NPC's in the area which can offer up advice about the
        ///     trail ahead.
        /// </summary>
        public override bool ChattingAllowed
        {
            get { return true; }
        }

        /// <summary>
        ///     Determines if this location has a store which the player can buy items from using their monies.
        /// </summary>
        public override bool ShoppingAllowed
        {
            get { return true; }
        }
    }
}