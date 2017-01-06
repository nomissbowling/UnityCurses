// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Engine.Window;
using Assets.Engine.Window.Form;
using Assets.Engine.Window.Form.Input;

namespace Assets.OregonTrail.Window.MainMenu.Profession
{
    /// <summary>
    ///     Shows information about what the player leader professions mean and how it affects the party, vehicle, game
    ///     difficulty, and scoring at the end (if they make it).
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class ProfessionHelp : InputForm<NewGameInfo>
    {
        /// <summary>Initializes a new instance of the <see cref="ProfessionHelp" /> class.</summary>
        /// <param name="window">The window.</param>
        public ProfessionHelp(IWindow window) : base(window)
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
            // Information about professions and how they work.
            var _job = new StringBuilder();
            _job.Append(string.Format("{0}Traveling to Oregon isn't easy!{1}", Environment.NewLine, Environment.NewLine));
            _job.Append(string.Format("But if you're a banker, you'll{0}", Environment.NewLine));
            _job.Append(string.Format("have more money for supplies{0}", Environment.NewLine));
            _job.Append(string.Format("and services than a carpenter{0}", Environment.NewLine));
            _job.Append(string.Format("or a farmer.{0}{1}", Environment.NewLine, Environment.NewLine));
            _job.Append(string.Format("However, the harder you have{0}", Environment.NewLine));
            _job.Append(string.Format("to try, the more points you{0}", Environment.NewLine));
            _job.Append(string.Format("deserve! Therefore, the{0}", Environment.NewLine));
            _job.Append(string.Format("farmer earns the greatest{0}", Environment.NewLine));
            _job.Append(string.Format("number of points and the{0}", Environment.NewLine));
            _job.Append(string.Format("banker earns the least.{0}{1}", Environment.NewLine, Environment.NewLine));
            return _job.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = new ProfessionSelector(parentGameMode, UserData);
            SetForm(typeof(ProfessionSelector));
        }
    }
}