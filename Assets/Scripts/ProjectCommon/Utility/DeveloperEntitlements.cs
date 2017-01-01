// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System.ComponentModel;

namespace Assets.Scripts.ProjectCommon.Utility
{
    /// <summary>
    ///     List of all the developers in the game, the description attribute is used and if that is not there the actual
    ///     enumeration value will be used.
    /// </summary>
    public enum DeveloperEntitlements
    {
        /// <summary>
        ///     Name of the player character and his age.
        /// </summary>
        [Description("Subject: Vladimir Gagarin - Age: 27")]
        PlayerName,

        /// <summary>
        ///     Explains to the player that Vlad is indeed Russian if they could not get that from his name.
        /// </summary>
        [Description("Nationality: Russian")]
        PlayerNationality,

        /// <summary>
        ///     Title of the game and also the occupation of the player character.
        /// </summary>
        [Description("Occupation: Space Trucker")]
        PlayerJob,

        /// <summary>
        ///     Arf!
        /// </summary>
        [Description("Lead Programming: Ron \"Maxwolf\" McDowell")]
        Maxwolf,

        /// <summary>
        ///     Yip!
        /// </summary>
        [Description("Additional Programming: Kyle \"Fox\" Polulak")]
        Fox,

        /// <summary>
        ///     Rufus!
        /// </summary>
        [Description("Models and Artwork: Sam \"Prowler\" Coleman")]
        Prowler,

        /// <summary>
        ///     Helped us get greenlit, thank you!
        /// </summary>
        [Description("Publishing Assistance: WE LOVE WOLVES")]
        LittleWolfe
    }
}