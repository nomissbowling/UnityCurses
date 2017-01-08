// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.Engine;
using Assets.Maxwolf.OregonTrail.Event.Vehicle;
using Assets.Maxwolf.OregonTrail.Window.Travel.Dialog;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Form;
using Assets.Maxwolf.WolfCurses.Window.Form.Input;

namespace Assets.Maxwolf.OregonTrail.Window.Travel.RiverCrossing
{
    /// <summary>
    ///     Displays the final crossing result for the river crossing location. No matter what choice the player made, what
    ///     events happen along the way, this final screen will be shown to let the user know how the last leg of the journey
    ///     went. It is possible to get stuck in the mud, however most of the messages are safe and just let the user know they
    ///     finally made it across.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class CrossingResult : InputForm<TravelInfo>
    {
        /// <summary>
        ///     The crossing result.
        /// </summary>
        private StringBuilder _crossingResult;

        public override void OnFormPreCreate(IWindow window)
        {
            base.OnFormPreCreate(window);
            _crossingResult = new StringBuilder();
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The text user interface.<see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            // Clear any previous crossing result prompt.
            _crossingResult = new StringBuilder();

            // Depending on crossing type we will say different things about the crossing.
            switch (UserData.River.CrossingType)
            {
                case RiverCrossChoice.Ford:
                    if (EngineApp.Random.NextBool())
                    {
                        // No loss in time, but warning to let the player know it's dangerous.
                        _crossingResult.AppendLine(string.Format("{0}It was a muddy crossing,", Environment.NewLine));
                        _crossingResult.AppendLine("but you did not get");
                        _crossingResult.AppendLine(string.Format("stuck.{0}", Environment.NewLine));
                    }
                    else
                    {
                        // Triggers event for muddy shore that makes player lose a day, forces end of crossing also.
                        FinishCrossing();
                        OregonTrailApp.Instance.EventDirector.TriggerEvent(OregonTrailApp.Instance.Vehicle,
                            typeof(StuckInMud));
                    }

                    break;
                case RiverCrossChoice.Float:
                    if (UserData.River.DisasterHappened)
                    {
                        _crossingResult.AppendLine(string.Format("{0}Your party relieved", Environment.NewLine));
                        _crossingResult.AppendLine("to reach other side after");
                        _crossingResult.AppendLine(string.Format("trouble floating across.{0}", Environment.NewLine));
                    }
                    else
                    {
                        _crossingResult.AppendLine(string.Format("{0}You had no trouble", Environment.NewLine));
                        _crossingResult.AppendLine("floating the wagon");
                        _crossingResult.AppendLine(string.Format("across.{0}", Environment.NewLine));
                    }

                    break;
                case RiverCrossChoice.Ferry:
                    if (UserData.River.DisasterHappened)
                    {
                        _crossingResult.AppendLine(string.Format("{0}The ferry operator", Environment.NewLine));
                        _crossingResult.AppendLine("apologizes for the");
                        _crossingResult.AppendLine(string.Format("rough ride.{0}", Environment.NewLine));
                    }
                    else
                    {
                        _crossingResult.AppendLine(string.Format("{0}The ferry got your party", Environment.NewLine));
                        _crossingResult.AppendLine(string.Format("and wagon safely across.{0}", Environment.NewLine));
                    }

                    break;
                case RiverCrossChoice.Indian:
                    if (UserData.River.DisasterHappened)
                    {
                        _crossingResult.AppendLine(string.Format("{0}The Indian runs away", Environment.NewLine));
                        _crossingResult.AppendLine("as soon as you");
                        _crossingResult.AppendLine(string.Format("reach the shore.{0}", Environment.NewLine));
                    }
                    else
                    {
                        _crossingResult.AppendLine(string.Format("{0}The Indian helped your", Environment.NewLine));
                        _crossingResult.AppendLine(string.Format("wagon safely across.{0}", Environment.NewLine));
                    }

                    break;
                case RiverCrossChoice.None:
                case RiverCrossChoice.WaitForWeather:
                case RiverCrossChoice.GetMoreInformation:
                    throw new InvalidOperationException(
                        string.Format("Invalid river crossing result choice {0}.", UserData.River.CrossingType));
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Render the crossing result to text user interface.
            return _crossingResult.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            FinishCrossing();
        }

        /// <summary>
        ///     Cleans up any remaining data about this river the player just crossed.
        /// </summary>
        private void FinishCrossing()
        {
            // Shutdown the river data now that we are done with it.
            UserData.DestroyRiver();

            // River crossing takes you a day.
            OregonTrailApp.Instance.TakeTurn(false);

            // Start going there...
            SetForm(typeof(LocationDepart));
        }
    }
}