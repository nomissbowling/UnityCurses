// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Source.Engine.Window;
using Assets.Source.Engine.Window.Form;
using Assets.Source.Engine.Window.Form.Input;

namespace Assets.Source.OregonTrail.Window.MainMenu.Options
{
    /// <summary>
    ///     Erases all the saved JSON Tombstone epitaphs on the disk so other players will not encounter them, new ones can
    ///     be created then.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class EraseTombstone : InputForm<NewGameInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EraseTombstone" /> class.
        ///     This constructor will be used by the other one
        /// </summary>
        /// <param name="window">The window.</param>
        public EraseTombstone(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Defines what type of dialog this will act like depending on this enumeration value. Up to implementation to define
        ///     desired behavior.
        /// </summary>
        protected override DialogType DialogType
        {
            get { return DialogType.YesNo; }
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnDialogPrompt()
        {
            var eraseEpitaphs = new StringBuilder();

            // Text above the table to declare what this state is.
            eraseEpitaphs.Append(
                string.Format("{0}Erase tombstone messages{1}{2}", Environment.NewLine, Environment.NewLine,
                    Environment.NewLine));

            // Tell the user how tombstones work before destroying them.
            eraseEpitaphs.Append(string.Format("There may be one tombstone on{0}", Environment.NewLine));
            eraseEpitaphs.Append(string.Format("the first half of the trail and{0}", Environment.NewLine));
            eraseEpitaphs.Append(string.Format("one tombstone on the second{0}", Environment.NewLine));
            eraseEpitaphs.Append(string.Format("half. If you erase the{0}", Environment.NewLine));
            eraseEpitaphs.Append(string.Format("tombstone messages, they will{0}", Environment.NewLine));
            eraseEpitaphs.Append(string.Format("not be replaced until team{0}", Environment.NewLine));
            eraseEpitaphs.Append(string.Format("leaders die along the trail.{0}{1}", Environment.NewLine,
                Environment.NewLine));

            eraseEpitaphs.Append("Do you want to do this? Y/N");
            return eraseEpitaphs.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Actually erase Tombstone messages.
            OregonTrailApp.Instance.Tombstone.Reset();

            SetForm(typeof(ManagementOptions));
        }
    }
}