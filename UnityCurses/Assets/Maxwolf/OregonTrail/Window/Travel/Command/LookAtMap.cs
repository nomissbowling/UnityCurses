﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.OregonTrail.Entity.Location;
using Assets.Maxwolf.OregonTrail.Entity.Location.Point;
using Assets.Maxwolf.OregonTrail.Window.Travel.Dialog;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Control;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.Travel.Command
{
    /// <summary>
    ///     Shows the player their vehicle and list of all the points in the trail they could possibly travel to. It marks the
    ///     spot they are on and all the spots they have visited, shows percentage for completion and some other basic
    ///     statistics about the journey that could only be seen from this state.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class LookAtMap : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LookAtMap" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public LookAtMap(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Init visual progress representation of the trail.
            var _map = new StringBuilder();
            _map.AppendLine(string.Format("{0}Trail progress{1}", Environment.NewLine, Environment.NewLine));
            _map.AppendLine(TextProgress.DrawProgressBar(
                                OregonTrailApp.Instance.Trail.LocationIndex + 1,
                                OregonTrailApp.Instance.Trail.Locations.Count, 32) + Environment.NewLine);

            // Build up a table of location names and if the player has visited them.
            var locationTable = OregonTrailApp.Instance.Trail.Locations.ToStringTable(
                new[] {"Visited", "Location Name"},
                u => u.Status >= LocationStatus.Arrived,
                u => u.Name
            );
            _map.AppendLine(locationTable);

            return _map.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Check if current location is a fork in the road, if so we will return to that form.
            if (OregonTrailApp.Instance.Trail.CurrentLocation is ForkInRoad)
            {
                SetForm(typeof(LocationFork));
                return;
            }

            // Default action is to return to travel menu.
            ClearForm();
        }
    }
}