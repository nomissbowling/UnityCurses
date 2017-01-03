// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 12/31/2015@4:49 AM

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Assets.Engine.Window.Control;
using UnityEngine;

namespace Assets.Engine
{
    /// <summary>
    ///     Base simulation application class object. This class should not be declared directly but inherited by actual
    ///     instance of game controller.
    /// </summary>
    public abstract class EngineApp : MonoBehaviour, ITick
    {
        /// <summary>
        ///     Determines if the dynamic menu system should show the command names or only numbers. If false then only numbers
        ///     will be shown.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const bool SHOW_COMMANDS = false;

        /// <summary>
        ///     Constant for the amount of time difference that should occur from last tick and current tick in milliseconds before
        ///     the simulation logic will be ticked.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private const double TICK_INTERVAL = 1000.0d;

        /// <summary>
        ///     Time and date of latest system tick, used to measure total elapsed time and tick simulation after each second.
        /// </summary>
        private DateTime _currentTickTime;

        /// <summary>
        ///     Last known time the simulation was ticked with logic and all sub-systems. This is not the same as a system tick
        ///     which can happen hundreds of thousands of times a second or just a few, we only measure the difference in time on
        ///     them.
        /// </summary>
        private DateTime _lastTickTime;

        /// <summary>
        ///     Spinning character pixel.
        /// </summary>
        private SpinningPixel _spinningPixel;

        private static EngineApp _instance;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailGame.EngineApp" /> class.
        /// </summary>
        protected EngineApp()
        {
            // We are not closing...
            IsClosing = false;

            // Date and time the simulation was started, which we use as benchmark for all future time passed.
            _lastTickTime = DateTime.UtcNow;
            _currentTickTime = DateTime.UtcNow;

            // Visual tick representations for other sub-systems.
            TotalSecondsTicked = 0;

            // Setup spinning pixel to show game is not thread locked.
            _spinningPixel = new SpinningPixel();
            TickPhase = _spinningPixel.Step();

            // Init modules needed for managing simulation.
            Random = new Randomizer();
            WindowManager = new WindowManager(this);
            SceneGraph = new SceneGraph(this);

            // Input manager needs event hook for knowing when buffer is sent.
            InputManager = new InputManager(this);
        }

        /// <summary>
        ///     Determines if the simulation is currently closing down.
        /// </summary>
        public bool IsClosing { get; private set; }

        /// <summary>
        ///     Shows the current status of the simulation visually as a spinning glyph, the purpose of which is to show that there
        ///     is no hang in the simulation or logic controllers and everything is moving along and waiting for input or
        ///     displaying something to user.
        /// </summary>
        internal string TickPhase { get; private set; }

        /// <summary>
        ///     Total number of ticks that have gone by from measuring system ticks, this means this measures the total number of
        ///     seconds that have gone by using the pulses and time dilation without the use of dirty times that spawn more
        ///     threads.
        /// </summary>
        private ulong TotalSecondsTicked { get; set; }

        /// <summary>
        ///     Used for rolling the virtual dice in the simulation to determine the outcome of various events.
        /// </summary>
        public Randomizer Random { get; private set; }

        /// <summary>
        ///     Keeps track of the currently attached game Windows, which one is active, and getting text user interface data.
        /// </summary>
        public WindowManager WindowManager { get; private set; }

        /// <summary>
        ///     Handles input from the users keyboard, holds an input buffer and will push it to the simulation when return key is
        ///     pressed.
        /// </summary>
        public InputManager InputManager { get; private set; }

        /// <summary>
        ///     Shows the current state of the simulation as text only interface (TUI). Uses default constants if the attached
        ///     Windows
        ///     or state does not override this functionality and it is ticked.
        /// </summary>
        public SceneGraph SceneGraph { get; private set; }

        /// <summary>
        ///     Determines what windows the simulation will be capable of using and creating using the window managers factory.
        /// </summary>
        public abstract IEnumerable<Type> AllowedWindows { get; }

        /// <summary>
        ///     Holds reference to the current simulation logic that inherits and extends the functionality provided by engine app
        ///     base ticks.
        /// </summary>
        public static EngineApp Instance
        {
            get { return _instance ?? (_instance = (new GameObject("EngineApp")).AddComponent<EngineApp>()); }
        }

        /// <summary>
        ///     Called when the simulation is ticked by underlying operating system, game engine, or potato. Each of these system
        ///     ticks is called at unpredictable rates, however if not a system tick that means the simulation has processed enough
        ///     of them to fire off event for fixed interval that is set in the core simulation by constant in milliseconds.
        /// </summary>
        /// <remarks>Default is one second or 1000ms.</remarks>
        /// <param name="systemTick">
        ///     TRUE if ticked unpredictably by underlying operating system, game engine, or potato. FALSE if
        ///     pulsed by game simulation at fixed interval.
        /// </param>
        /// <param name="skipDay">
        ///     Determines if the simulation has force ticked without advancing time or down the trail. Used by
        ///     special events that want to simulate passage of time without actually any actual time moving by.
        /// </param>
        [SuppressMessage("ReSharper", "TailRecursiveCall")]
        public virtual void OnTick(bool systemTick, bool skipDay)
        {
            // No ticks allowed if simulation is shutting down.
            if (IsClosing)
                return;

            // Sends commands if queue has any.
            if (InputManager != null)
                InputManager.OnTick(systemTick, skipDay);

            // Back buffer for only sending text when changed.
            if (SceneGraph != null)
                SceneGraph.OnTick(systemTick, skipDay);

            // Changes game Windows and state when needed.
            if (WindowManager != null)
                WindowManager.OnTick(systemTick, skipDay);

            // Rolls virtual dice.
            if (Random != null)
                Random.OnTick(systemTick, skipDay);

            // System tick is from execution platform, otherwise they are linear simulation ticks.
            if (systemTick)
            {
                _currentTickTime = DateTime.UtcNow;
                var elapsedTicks = _currentTickTime.Ticks - _lastTickTime.Ticks;
                var elapsedSpan = new TimeSpan(elapsedTicks);

                // Check if more than an entire second has gone by.
                if (!(elapsedSpan.TotalMilliseconds > TICK_INTERVAL))
                    return;

                // Reset last tick time to current time for measuring towards next second tick.
                _lastTickTime = _currentTickTime;

                // Recursive call on ourselves to process non-system ticks.
                OnTick(false, skipDay);
            }
            else
            {
                // Increase the total seconds ticked.
                TotalSecondsTicked++;

                // Fire event for first tick when it occurs, and only then.
                if (TotalSecondsTicked == 1)
                    OnFirstTick();

                // Visual representation of ticking for debugging purposes.
                TickPhase = _spinningPixel.Step();
            }
        }

        /// <summary>
        ///     Creates new instance of game simulation. Complains if instance already exists.
        /// </summary>
        public static void Init(EngineApp instance)
        {
            // Ensure the instance does not already exist.
            if (Instance != null)
            {
                throw new InvalidOperationException(
                    "Unable to create new instance of game simulation since it already exists!");
            }

            // Use the instance that was passed into us from constructor.
            _instance = instance;

            // Call OnCreate on the abstract method to allow inheriting simulation data to initialize.
            if (!_instance.OnCreate())
            {
                throw new InvalidOperationException(
                    "Unable to create engine instance, there was a problem during Init and OnCreate methods!");
            }
        }

        /// <summary>
        ///     Called when the engine is created and a simulation made active via a chain of inheritence.
        /// </summary>
        protected virtual bool OnCreate()
        {
            // Return false if we are unable to grab proper instance data from initialization process.
            return Instance != null;
        }

        /// <summary>
        ///     Fired when the ticker receives the first system tick event.
        /// </summary>
        protected abstract void OnFirstTick();

        /// <summary>
        ///     Fired when the simulation is closing and needs to clear out any data structures that it created so the program can
        ///     exit cleanly.
        /// </summary>
        public static void Shutdown()
        {
            if (Instance != null)
            {
                // Set flag that we are closing now so we can ignore ticks during shutdown.
                Instance.IsClosing = true;

                // Allows game simulation above us to cleanup any data structures it cares about.
                Instance.OnShutdown();

                // Remove simulation presentation variables.
                Instance._lastTickTime = DateTime.MinValue;
                Instance._currentTickTime = DateTime.MinValue;
                Instance.TotalSecondsTicked = 0;
                Instance._spinningPixel = null;
                Instance.TickPhase = string.Empty;

                // Remove simulation core modules.
                Instance.Random = null;
                Instance.WindowManager = null;
                Instance.SceneGraph = null;
                Instance.InputManager = null;
            }

            // Destroys the session instance.
            _instance = null;
        }

        /// <summary>
        ///     Creates and or clears data sets required for game simulation and attaches the travel menu and the main menu to make
        ///     the program completely restarted as if fresh.
        /// </summary>
        public virtual void Restart()
        {
            // Resets the window manager and clears out all windows and forms from previous session.
            WindowManager.Clear();

            // Clears the input buffer of any left over input from last play session.
            InputManager.ClearBuffer();

            // Removes any text from the interface from previous session.
            SceneGraph.Clear();
        }

        /// <summary>
        ///     Called when simulation is about to destroy itself, but right before it actually does it.
        /// </summary>
        protected abstract void OnShutdown();

        /// <summary>
        ///     Called by the text user interface scene graph renderer before it asks the active window to render itself out for
        ///     display.
        /// </summary>
        public abstract string OnPreRenderScene();
    }
}