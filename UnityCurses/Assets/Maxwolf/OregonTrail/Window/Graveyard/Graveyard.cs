﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Maxwolf.WolfCurses.Window;

namespace Assets.Maxwolf.OregonTrail.Window.Graveyard
{
    /// <summary>
    ///     Displays the name of a previous player whom traveled the trail and died at a given mile marker. There is also an
    ///     optional epitaph that can be displayed. These tombstones are saved per trail, and can be reset from main menu.
    /// </summary>
    public sealed class Graveyard : Window<TombstoneCommands, TombstoneInfo>
    {
        /// <summary>
        ///     Called after the Windows has been added to list of modes and made active.
        /// </summary>
        public override void OnWindowPostCreate()
        {
            base.OnWindowPostCreate();

            // Depending on the living status of passengers in current player vehicle we will attach a different form.
            SetForm(OregonTrailApp.Instance.Vehicle.PassengerLivingCount <= 0
                ? typeof(EpitaphQuestion)
                : typeof(TombstoneView));
        }
    }
}