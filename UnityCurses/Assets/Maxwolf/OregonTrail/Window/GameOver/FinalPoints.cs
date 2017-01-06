// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using System.Text;
using Assets.Maxwolf.OregonTrail.Entity;
using Assets.Maxwolf.OregonTrail.Entity.Item;
using Assets.Maxwolf.OregonTrail.Entity.Person;
using Assets.Maxwolf.OregonTrail.Module.Scoring;
using Assets.Maxwolf.ProjectCommon.Utility;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Control;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.GameOver
{
    /// <summary>
    ///     Shows point tabulation based on current simulation statistics. This way if the player dies or finishes the game we
    ///     just attach this state to the travel mode and it will show the final score and reset the game and return to main
    ///     menu when the player is done.
    /// </summary>
    [ParentWindow(typeof(GameOver))]
    public sealed class FinalPoints : InputForm<GameOverInfo>
    {
        /// <summary>
        ///     Holds the final point tabulation for the player to see.
        /// </summary>
        private StringBuilder _pointsPrompt;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FinalPoints" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public FinalPoints(IWindow window) : base(window)
        {
            _pointsPrompt = new StringBuilder();
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Build up a representation of the current points the player has.
            _pointsPrompt.AppendLine(string.Format("{0}Points for arriving in Oregon{1}", Environment.NewLine,
                Environment.NewLine));

            // Shortcut to the game simulation instance to make code easier to read.
            var game = OregonTrailApp.Instance;

            // Calculate the total points of all spare parts for the tuple list below ahead of time.
            var spareAxles = new PointHolder(
                game.Vehicle.Inventory[Entities.Axle].Quantity,
                game.Vehicle.Inventory[Entities.Axle].PluralForm,
                game.Vehicle.Inventory[Entities.Axle].Points);

            var spareTongues = new PointHolder(
                game.Vehicle.Inventory[Entities.Tongue].Quantity,
                game.Vehicle.Inventory[Entities.Tongue].PluralForm,
                game.Vehicle.Inventory[Entities.Tongue].Points);

            var spareWheels = new PointHolder(
                game.Vehicle.Inventory[Entities.Wheel].Quantity,
                game.Vehicle.Inventory[Entities.Wheel].PluralForm,
                game.Vehicle.Inventory[Entities.Wheel].Points);

            var spareParts = new PointHolder(
                spareAxles.Quantity + spareTongues.Quantity + spareWheels.Quantity,
                "spare wagon parts",
                spareAxles.Points + spareTongues.Points + spareWheels.Points);

            // Calculates the average health just once because we need it many times.
            var avgHealth = game.Vehicle.PassengerHealthStatus;

            // Figures out who the leader is among the vehicle passengers.
            var leaderPerson = game.Vehicle.PassengerLeader;

            // Builds up a list of tuples that represent quantity, description, and total points.
            var tuplePoints = new List<PointHolder>
            {
                // HealthStatus of vehicle passengers that are still alive.
                new PointHolder(
                    game.Vehicle.PassengerLivingCount,
                    string.Format("people in {0} health", avgHealth.ToDescriptionAttribute().ToLowerInvariant()),
                    game.Vehicle.PassengerLivingCount * (int) avgHealth),
                new PointHolder(1, "wagon", Resources.Vehicle.Points),
                new PointHolder(
                    game.Vehicle.Inventory[Entities.Animal].Quantity,
                    "oxen",
                    game.Vehicle.Inventory[Entities.Animal].Points),
                spareParts,
                new PointHolder(
                    game.Vehicle.Inventory[Entities.Clothes].Quantity,
                    "sets of clothing",
                    game.Vehicle.Inventory[Entities.Clothes].Points),
                new PointHolder(
                    game.Vehicle.Inventory[Entities.Ammo].Quantity,
                    "bullets",
                    game.Vehicle.Inventory[Entities.Ammo].Points),
                new PointHolder(
                    game.Vehicle.Inventory[Entities.Food].Quantity,
                    "pounds of food",
                    game.Vehicle.Inventory[Entities.Food].Points),
                new PointHolder(
                    game.Vehicle.Inventory[Entities.Cash].Quantity,
                    "cash",
                    game.Vehicle.Inventory[Entities.Cash].Points)
            };

            // Init the actual points table from the tuple list data we created above from game simulation state.
            var locationTable = tuplePoints.ToStringTable(
                new[] {"Quantity", "Description", "Points"},
                u => u.Quantity,
                u => u.PluralForm,
                u => u.Points
            );
            _pointsPrompt.AppendLine(locationTable);

            // Calculate total points for all entities and items.
            var totalPoints = 0;
            foreach (var tuplePoint in tuplePoints)
                totalPoints += tuplePoint.Points;

            _pointsPrompt.AppendLine(string.Format("Total: {0}", totalPoints));

            // Add the total with the bonus so player can see the difference.
            var totalPointsWithBonus = totalPoints * (int) leaderPerson.Profession;
            switch (leaderPerson.Profession)
            {
                case Profession.Banker:
                    break;
                case Profession.Carpenter:
                    _pointsPrompt.AppendLine(string.Format("Bonus Total: {0}", totalPointsWithBonus));
                    break;
                case Profession.Farmer:
                    _pointsPrompt.AppendLine(string.Format("Bonus Total: {0}", totalPointsWithBonus));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // When building up the bonus text we will change the message about point multiplier so it makes sense.
            _pointsPrompt.AppendLine(
                string.Format("{0}For going as a {1}, your", Environment.NewLine,
                    leaderPerson.Profession.ToString().ToLowerInvariant()));
            switch (leaderPerson.Profession)
            {
                case Profession.Banker:
                    _pointsPrompt.AppendLine(string.Format("points are normal, no bonus!{0}", Environment.NewLine));
                    break;
                case Profession.Carpenter:
                    _pointsPrompt.AppendLine(string.Format("points are doubled.{0}", Environment.NewLine));
                    break;
                case Profession.Farmer:
                    _pointsPrompt.AppendLine(string.Format("points are tripled.{0}", Environment.NewLine));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Add the score to the current listing that will get saved.
            OregonTrailApp.Instance.Scoring.Add(new Highscore(leaderPerson.Name, totalPointsWithBonus));

            return _pointsPrompt.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Completely resets the game to default state it was in when it first started.
            OregonTrailApp.Instance.Restart();
        }

        /// <summary>
        ///     Acts like a tuple and holds several pieces of data that are used in final points tabulation and scoring.
        /// </summary>
        private class PointHolder
        {
            private readonly string _pluralForm;
            private readonly int _points;
            private readonly int _quantity;

            public PointHolder(int quantity, string pluralForm, int points)
            {
                _quantity = quantity;
                _pluralForm = pluralForm;
                _points = points;
            }

            public int Quantity
            {
                get { return _quantity; }
            }

            public string PluralForm
            {
                get { return _pluralForm; }
            }

            public int Points
            {
                get { return _points; }
            }
        }
    }
}