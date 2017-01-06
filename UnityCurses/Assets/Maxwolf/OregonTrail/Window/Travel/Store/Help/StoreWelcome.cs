// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using System;
using System.Text;
using Assets.Maxwolf.WolfCurses;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Form;

namespace Assets.Maxwolf.OregonTrail.Window.Travel.Store.Help
{
    /// <summary>
    ///     Offers up some free information about what items are important to the player and what they mean for the during the
    ///     course of the simulation.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class StoreWelcome : Form<TravelInfo>
    {
        /// <summary>
        ///     Keeps track if the player has read all the advice and this dialog needs to be closed.
        /// </summary>
        private bool _hasReadAdvice;

        /// <summary>
        ///     Keeps track of the message we want to show to the player but only build it actually once.
        /// </summary>
        private StringBuilder _storeHelp;

        /// <summary>
        ///     Determines which panel of information we have shown to the user, pressing return will cycle through them.
        /// </summary>
        private int adviceCount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StoreWelcome" /> class.
        ///     Offers up some free information about what items are important to the player and what they mean for the during the
        ///     course of the simulation.
        /// </summary>
        /// <param name="window">The window.</param>
        public StoreWelcome(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Determines if user input is currently allowed to be typed and filled into the input buffer.
        /// </summary>
        /// <remarks>Default is FALSE. Setting to TRUE allows characters and input buffer to be read when submitted.</remarks>
        public override bool InputFillsBuffer
        {
            get { return false; }
        }

        /// <summary>
        ///     Fired after the state has been completely attached to the simulation letting the state know it can browse the user
        ///     data and other properties below it.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            _hasReadAdvice = false;
            _storeHelp = new StringBuilder();
            UpdateAdvice();
        }

        /// <summary>
        ///     Since the advice can change we have to do this in chunks.
        /// </summary>
        private void UpdateAdvice()
        {
            // Clear any previous string builder message.
            _storeHelp = new StringBuilder();

            // Init the current state of our advice to player.
            _storeHelp.Append(string.Format("{0}Hello, I'm Matt. So you're going{1}", Environment.NewLine,
                Environment.NewLine));
            _storeHelp.Append(string.Format("to Oregon! I can fix you up with{0}", Environment.NewLine));
            _storeHelp.Append(string.Format("what you need:{0}{1}", Environment.NewLine, Environment.NewLine));

            if (adviceCount <= 0)
            {
                _storeHelp.Append(string.Format(" - a team of oxen to pull{0}", Environment.NewLine));
                _storeHelp.Append(string.Format(" your vehicle{0}{1}", Environment.NewLine, Environment.NewLine));

                _storeHelp.Append(string.Format(" - clothing for both{0}", Environment.NewLine));
                _storeHelp.Append(string.Format(" summer and winter{0}{1}", Environment.NewLine, Environment.NewLine));
            }
            else if (adviceCount == 1)
            {
                _storeHelp.Append(string.Format(" - plenty of food for the{0}", Environment.NewLine));
                _storeHelp.Append(string.Format(" trip{0}{1}", Environment.NewLine, Environment.NewLine));

                _storeHelp.Append(string.Format(" - ammunition for your{0}", Environment.NewLine));
                _storeHelp.Append(string.Format(" rifles{0}{1}", Environment.NewLine, Environment.NewLine));

                _storeHelp.Append(string.Format(" - spare parts for your{0}", Environment.NewLine));
                _storeHelp.Append(string.Format(" wagon{0}{1}", Environment.NewLine, Environment.NewLine));
            }

            // Wait for user input...
            _storeHelp.Append(InputManager.PRESSENTER);
        }

        /// <summary>
        ///     Returns a text only representation of the current game Windows state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string OnRenderForm()
        {
            return _storeHelp.ToString();
        }

        /// <summary>Fired when the game Windows current state is not null and input buffer does not match any known command.</summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game Windows.</param>
        public override void OnInputBufferReturned(string input)
        {
            // On the last advice panel we flip a normal boolean to know we are definitely done here.
            if (_hasReadAdvice)
            {
                ClearForm();
                return;
            }

            // Tick the advice to next panel when we get input.
            if (adviceCount <= 0)
            {
                adviceCount++;
                UpdateAdvice();
                return;
            }

            // Make sure we don't run final logic to show actual store until we show all advice to player.
            if (adviceCount < 1)
                return;

            _hasReadAdvice = true;
            SetForm(typeof(Store));
        }
    }
}