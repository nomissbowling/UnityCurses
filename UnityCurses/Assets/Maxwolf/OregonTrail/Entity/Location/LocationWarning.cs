﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Maxwolf.ProjectCommon.Utility;

namespace Assets.Maxwolf.OregonTrail.Entity.Location
{
    /// <summary>
    ///     Defines all of the possible warnings that we would want to inform the user about while they are traveling on the
    ///     trail.
    /// </summary>
    public enum LocationWarning
    {
        /// <summary>
        ///     Default warning, meaning there is no warning and none will be displayed.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Water at this location is probably infested with cholera.
        /// </summary>
        [Description("Bad Water")] BadWater = 1,

        /// <summary>
        ///     Vehicle has very little food left.
        /// </summary>
        [Description("Low Food")] LowFood = 2,

        /// <summary>
        ///     Oxen unable to graze and operate at less and peak performance.
        /// </summary>
        [Description("Low Grass")] LowGrass = 3,

        /// <summary>
        ///     Location is very dry and has almost no localized water sources.
        /// </summary>
        [Description("Low Water")] LowWater = 4,

        /// <summary>
        ///     Vehicle is running very low on food for passengers to consume.
        /// </summary>
        [Description("No Food")] NoFood = 5,

        /// <summary>
        ///     Location has no water in the immediate area, performance of oxen and passengers suffer.
        /// </summary>
        [Description("No Water")] NoWater = 6,

        /// <summary>
        ///     Vehicle passengers have gone without food for several days and are now starving to death.
        /// </summary>
        Starvation = 7
    }
}