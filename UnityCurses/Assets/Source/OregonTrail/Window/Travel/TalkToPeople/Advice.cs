﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

namespace Assets.Source.OregonTrail.Window.Travel.TalkToPeople
{
    /// <summary>
    ///     Holds a single piece of advice for the trail. The purpose of this class is to clearly define what the advice is and
    ///     how it works, and finally what type of advice it is intended for as some are better suited for river crossings,
    ///     start of game, middle, and near the end of the game.
    /// </summary>
    public sealed class Advice
    {
        private readonly string _name;
        private readonly string _quote;

        /// <summary>Initializes a new instance of the <see cref="Advice" /> class. </summary>
        /// <param name="name">Name of the person that the advice was given by.</param>
        /// <param name="quote">Actual text advice given by the person.</param>
        public Advice(string name, string quote)
        {
            _name = name;
            _quote = quote;
        }

        /// <summary>
        ///     Name of the person that the advice was given by.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     Actual text advice given by the person.
        /// </summary>
        public string Quote
        {
            get { return _quote; }
        }
    }
}