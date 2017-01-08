// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Collections.Generic;
using System.Text;
using Assets.Maxwolf.OregonTrail.Entity.Item;
using Assets.Maxwolf.OregonTrail.Module.Scoring;
using Assets.Maxwolf.WolfCurses.Window.Control;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.MainMenu.Help
{
    /// <summary>
    ///     Second panel on point information, shows how the number of resources you end the game with contribute to your final
    ///     score.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class PointsAwardHelp : InputForm<NewGameInfo>
    {
        /// <summary>
        ///     Reference to points that will be given for entities of given matching types in this list.
        /// </summary>
        private static IEnumerable<Points> ResourcePoints
        {
            get
            {
                return new List<Points>
                {
                    new Points(Resources.Person, string.Empty),
                    new Points(Resources.Vehicle, string.Empty),
                    new Points(Parts.Oxen, string.Empty),
                    new Points(Parts.Wheel, string.Empty),
                    new Points(Parts.Axle, string.Empty),
                    new Points(Parts.Tongue, string.Empty),
                    new Points(Resources.Clothing, string.Empty),
                    new Points(Resources.Bullets, string.Empty),
                    new Points(Resources.Food, string.Empty),
                    new Points(Resources.Cash, string.Empty)
                };
            }
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            var _pointsItems = new StringBuilder();
            _pointsItems.Append(string.Format("{0}On Arriving in Oregon{1}{2}", Environment.NewLine, Environment.NewLine,
                Environment.NewLine));
            _pointsItems.Append(string.Format("The resources you arrive with will{0}", Environment.NewLine));
            _pointsItems.Append(string.Format("help you get started in the new{0}", Environment.NewLine));
            _pointsItems.Append(string.Format("land. You receive points for each{0}", Environment.NewLine));
            _pointsItems.Append(string.Format("item you bring safely to Oregon.{0}{1}", Environment.NewLine,
                Environment.NewLine));

            // Build up the table of resource points and how they work for player.
            var partyTable = ResourcePoints.ToStringTable(
                new[] {"Resources of Party", "Points per Item"},
                u => u.ToString(),
                u => u.PointsAwarded
            );

            // Print the table of how resources earn points.
            _pointsItems.AppendLine(partyTable);
            return _pointsItems.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = new PointsMultiplyerHelp(parentGameMode, UserData);
            SetForm(typeof(PointsMultiplyerHelp));
        }
    }
}