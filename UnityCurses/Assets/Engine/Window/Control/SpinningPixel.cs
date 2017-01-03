﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 12/31/2015@4:49 AM

using System.Collections.Generic;

namespace Assets.Engine.Window.Control
{
    /// <summary>
    ///     Old school spinning pixel progress, normally used to show the thread is not locked by some running process.
    /// </summary>
    internal sealed class SpinningPixel
    {
        /// <summary>
        ///     The animation.
        /// </summary>
        private List<string> animation;

        /// <summary>
        ///     The counter.
        /// </summary>
        private int counter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpinningPixel" /> class.
        /// </summary>
        public SpinningPixel()
        {
            animation = new List<string>();
            animation.Add("/");
            animation.Add("-");
            animation.Add(@"\");
            animation.Add("|");
            counter = 0;
        }

        /// <summary>
        ///     prints the character found in the animation according to the current index
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string Step()
        {
            string barText = animation[counter];
            counter++;
            if (counter == animation.Count)
                counter = 0;

            return barText;
        }
    }
}