﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System.Text;
using Assets.Engine.Utility;
using Assets.Engine.Window;
using Assets.OregonTrail.Entity;
using Assets.OregonTrail.Entity.Location;
using Assets.OregonTrail.Entity.Location.Point;
using Assets.OregonTrail.Window.Travel.Hunt;
using Assets.OregonTrail.Window.Travel.RiverCrossing;
using Assets.OregonTrail.Window.Travel.Store;
using Assets.OregonTrail.Window.Travel.Toll;

namespace Assets.OregonTrail.Window.Travel
{
    /// <summary>
    ///     Holds all the information about traveling that we want to know, such as how long we need to go until next point,
    ///     what our current Windows is like moving, paused, etc.
    /// </summary>
    public sealed class TravelInfo : WindowData
    {
        private readonly StoreGenerator _store;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TravelInfo" /> class.
        ///     Creates default store implementation.
        /// </summary>
        public TravelInfo()
        {
            // Store so player can buy food, clothes, ammo, etc.
            _store = new StoreGenerator();
        }

        /// <summary>
        ///     Reference for any river information that we might need to be holding when we encounter one it will be generated and
        ///     this object filled with needed data that can be accessed by the other states as we attach them.
        /// </summary>
        public RiverGenerator River { get; private set; }

        /// <summary>
        ///     Keeps track of all the pending transactions that need to be made when the player visits a store.
        /// </summary>
        public StoreGenerator Store
        {
            get { return _store; }
        }

        /// <summary>
        ///     Holds all the important information related to a hunt for animals using bullets. When hunting form is attached this
        ///     will be used to maintain the state of the hunt and manage all the data related to it and scoring.
        /// </summary>
        public HuntManager Hunt { get; private set; }

        /// <summary>
        ///     Gets the current cost of the toll road that would like to be inserted into the trail, normally this is done from a
        ///     fork in the road however it could be on a linear path without any decision making.
        /// </summary>
        public TollGenerator Toll { get; private set; }

        /// <summary>
        ///     Used when the player is traveling on the trail between locations. Also known as drive state in travel game Windows.
        /// </summary>
        public static string DriveStatus
        {
            get
            {
                // Grab instance of game simulation.
                var game = OregonTrailApp.Instance;

                // GetModule the current food item from vehicle inventory.
                var foodItem = game.Vehicle.Inventory[Entities.Food];

                // Set default food status text, update to actual food item total weight if it exists.
                var foodStatus = "0 pounds";
                if (foodItem != null)
                    foodStatus = string.Format("{0} pounds", foodItem.TotalWeight);

                // Build up the status for the vehicle as it moves through the simulation.
                var driveStatus = new StringBuilder();
                driveStatus.AppendLine("--------------------------------");
                driveStatus.AppendLine(string.Format("Date: {0}", game.Time.Date));
                driveStatus.AppendLine(
                    string.Format("Weather: {0}", game.Trail.CurrentLocation.Weather.ToDescriptionAttribute()));
                driveStatus.AppendLine(string.Format("Health: {0}",
                    game.Vehicle.PassengerHealthStatus.ToDescriptionAttribute()));
                driveStatus.AppendLine(string.Format("Food: {0}", foodStatus));
                driveStatus.AppendLine(string.Format("Next landmark: {0} miles", game.Trail.DistanceToNextLocation));
                driveStatus.AppendLine(string.Format("Miles traveled: {0} miles", game.Vehicle.Odometer));
                driveStatus.AppendLine("--------------------------------");
                return driveStatus.ToString();
            }
        }

        /// <summary>
        ///     Determines how many days of rest the player had, and were simulated both in time and on event system.
        /// </summary>
        public int DaysToRest { get; internal set; }

        /// <summary>
        ///     Used when the player stops at a location on the trail, or the travel game Windows with no attached state. The
        ///     difference this state has from others is showing the name of the location, when between points we don't show this
        ///     since we already know the next point but don't want the player to know that.
        /// </summary>
        public static string TravelStatus
        {
            get
            {
                // Grab instance of game simulation.
                var game = OregonTrailApp.Instance;

                var showLocationName = game.Trail.CurrentLocation.Status == LocationStatus.Arrived;
                var locationStatus = new StringBuilder();
                locationStatus.AppendLine("--------------------------------");

                // Only add the location name if we are on the next point, otherwise we should not show this.
                locationStatus.AppendLine(showLocationName
                    ? game.Trail.CurrentLocation.Name
                    : string.Format("{0:N0} miles to {1}", game.Trail.DistanceToNextLocation,
                        game.Trail.NextLocation.Name));

                locationStatus.AppendLine(string.Format("{0}", game.Time.Date));
                locationStatus.AppendLine("--------------------------------");
                locationStatus.AppendLine(
                    string.Format("Weather: {0}", game.Trail.CurrentLocation.Weather.ToDescriptionAttribute()));
                locationStatus.AppendLine(string.Format("Health: {0}",
                    game.Vehicle.PassengerHealthStatus.ToDescriptionAttribute()));
                locationStatus.AppendLine(string.Format("Pace: {0}", game.Vehicle.Pace.ToDescriptionAttribute()));
                locationStatus.AppendLine(string.Format("Rations: {0}", game.Vehicle.Ration.ToDescriptionAttribute()));
                locationStatus.AppendLine("--------------------------------");
                return locationStatus.ToString();
            }
        }

        /// <summary>
        ///     Creates a new hunt with prey for the player to hunt with their ammunition.
        /// </summary>
        public void GenerateHunt()
        {
            if (Hunt != null)
                return;

            Hunt = new HuntManager();
        }

        /// <summary>
        ///     Destroys all the data about animals the player can hunt.
        /// </summary>
        public void DestroyHunt()
        {
            if (Hunt == null)
                return;

            Hunt = null;
        }

        /// <summary>
        ///     Creates a new toll cost for the given location that is inputted. If the player has enough monies and says YES the
        ///     location will be inserted into the trail, otherwise all the data will be destroyed and prompt returned to the fork
        ///     in the road where the toll probably came from.
        /// </summary>
        /// <param name="tollRoad">Location that is going to cost the player money in order to use the path to travel to it.</param>
        public void GenerateToll(TollRoad tollRoad)
        {
            if (Toll != null)
                return;

            Toll = new TollGenerator(tollRoad);
        }

        /// <summary>
        ///     Destroys all the associated data related to keeping track of a toll road and the cost for crossing it. If the
        ///     player encounters another toll toad this information will be re-generated.
        /// </summary>
        public void DestroyToll()
        {
            if (Toll == null)
                return;

            Toll = null;
        }

        /// <summary>
        ///     Creates a new river that can be accessed as a property from the travel game window.
        /// </summary>
        public void GenerateRiver()
        {
            // Skip if river has already been created.
            if (River != null)
                return;

            // Creates a new river.
            River = new RiverGenerator();
        }

        /// <summary>
        ///     Destroys all of the data associated with the previous river the player encountered.
        /// </summary>
        public void DestroyRiver()
        {
            // Skip if the river is already null.
            if (River == null)
                return;

            // Shutdown the river data.
            River = null;
        }
    }
}