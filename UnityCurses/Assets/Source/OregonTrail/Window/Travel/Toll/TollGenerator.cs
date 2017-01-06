﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Source.Engine;
using Assets.Source.OregonTrail.Entity.Location.Point;

namespace Assets.Source.OregonTrail.Window.Travel.Toll
{
    /// <summary>
    ///     Generates a new toll amount and keeps track of the location to be inserted if the deal goes through with the
    ///     player. Otherwise this information will be destroyed.
    /// </summary>
    public sealed class TollGenerator
    {
        private readonly int _cost;
        private readonly TollRoad _road;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Assets.Source.OregonTrail.Window.Travel.Toll.TollGenerator" /> class.
        /// </summary>
        /// <param name="tollRoad">Location that is going to cost the player money in order to use the path to travel to it.</param>
        public TollGenerator(TollRoad tollRoad)
        {
            _cost = EngineApp.Random.Next(1, 13);
            _road = tollRoad;
        }

        /// <summary>
        ///     Location that is going to cost the player money in order to use the path to travel to it.
        /// </summary>
        public TollRoad Road
        {
            get { return _road; }
        }

        /// <summary>
        ///     Gets the total toll for the cost road the player must pay before they will be allowed on the cost road.
        /// </summary>
        public int Cost
        {
            get { return _cost; }
        }
    }
}