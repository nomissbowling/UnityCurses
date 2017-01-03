// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;
using Assets.OregonTrail.Window.Travel.Command;

namespace Assets.OregonTrail.Window.Travel.Dialog
{
    /// <summary>
    ///     Shows information about what the different pace settings mean in terms for the simulation and how they will affect
    ///     vehicle, party, and events.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class PaceHelp : InputForm<TravelInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PaceHelp" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public PaceHelp(IWindow window) : base(window)
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
            // Steady
            var _paceHelp = new StringBuilder();
            _paceHelp.Append(string.Format("{0}steady - You travel about 8 hours a{1}", Environment.NewLine,
                Environment.NewLine));
            _paceHelp.Append(string.Format("day, taking frequent rests. You take{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("care not to get too tired.{0}{1}", Environment.NewLine, Environment.NewLine));

            // Strenuous
            _paceHelp.Append(string.Format("strenuous - You travel about 12 hours{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("a day, starting just after sunrise{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("and stopping shortly before sunset.{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("You stop to rest only when necessary.{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("You finish each day feeling very{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("tired.{0}{1}", Environment.NewLine, Environment.NewLine));

            // Grueling
            _paceHelp.Append(string.Format("grueling - You travel about 16 hours{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("a day, starting before sunrise and{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("continuing until dark. You almost{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("never stop to rest. You do not get{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("enough sleep at night. You finish{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("each day feeling absolutely{0}", Environment.NewLine));
            _paceHelp.Append(string.Format("exhausted, and your health suffers.{0}{1}", Environment.NewLine,
                Environment.NewLine));
            return _paceHelp.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = new ChangePace(parentGameMode, UserData);
            SetForm(typeof(ChangePace));
        }
    }
}