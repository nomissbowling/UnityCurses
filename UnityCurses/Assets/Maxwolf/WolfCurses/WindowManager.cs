﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 12/31/2015@2:38 PM

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Maxwolf.Engine;
using Assets.Maxwolf.WolfCurses.Window;
using Assets.Maxwolf.WolfCurses.Window.Form;

namespace Assets.Maxwolf.WolfCurses
{
    /// <summary>
    ///     Builds up a list of game modes and their states using reflection and attributes. Contains methods to add game modes
    ///     to running simulation. Can also remove modes and modify them further with states.
    /// </summary>
    public sealed class WindowManager : Module.Module
    {
        /// <summary>
        ///     Keeps track of all the possible states a given game mode can have by using attributes and reflection to keep track
        ///     of which user data object gets mapped to which particular state.
        /// </summary>
        private FormFactory _formFactory;

        /// <summary>
        ///     Factory pattern that will create game modes for it based on attribute at the top of each one that defines what
        ///     window type it is responsible for.
        /// </summary>
        private WindowFactory _windowFactory;

        /// <summary>
        ///     Current list of all game modes, only the last one added gets ticked this is so game modes can attach things on-top
        ///     of themselves like stores and trades.
        /// </summary>
        private Dictionary<Type, IWindow> _windowList = new Dictionary<Type, IWindow>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowManager" /> class.
        ///     Initializes a new instance of the <see cref="T:TrailSimulation.Core.ModuleProduct" /> class.
        /// </summary>
        /// <param name="simUnit">Core simulation which is controlling the window manager.</param>
        public WindowManager(EngineApp simUnit)
        {
            // Factories for modes and states that can be attached to them during runtime.
            _windowFactory = new WindowFactory(simUnit);
            _formFactory = new FormFactory();
        }

        /// <summary>
        ///     References the current active game Windows, or the last attached game Windows in the simulation.
        /// </summary>
        public IWindow FocusedWindow
        {
            get
            {
                lock (_windowList)
                {
                    return _windowList.LastOrDefault().Value;
                }
            }
        }

        /// <summary>
        ///     Retrieves the total number of windows that the manager is currently handling.
        /// </summary>
        internal int Count
        {
            get { return _windowList.Count; }
        }

        /// <summary>
        ///     Determines if this simulation is currently accepting input at all, the conditions for this require some game
        ///     Windows
        ///     to be attached and or active move to not be null.
        /// </summary>
        internal bool AcceptingInput
        {
            get
            {
                // Skip if there is no active modes.
                if (FocusedWindow == null)
                    return false;

                // Skip if Windows doesn't want input and has no state.
                if (!FocusedWindow.AcceptsInput && FocusedWindow.CurrentForm == null)
                    return false;

                // Skip if Windows state doesn't want input and current state is not null.
                if (FocusedWindow.CurrentForm != null && !FocusedWindow.AcceptsInput)
                    return false;

                // Skip if state is not null and, game Windows accepts input, but current state doesn't want input.
                return FocusedWindow.CurrentForm == null ||
                       !FocusedWindow.AcceptsInput ||
                       FocusedWindow.CurrentForm.InputFillsBuffer;
            }
        }

        /// <summary>
        ///     Fired when the simulation is closing and needs to clear out any data structures that it created so the program can
        ///     exit cleanly.
        /// </summary>
        public override void Destroy()
        {
            // Windows factory and list of modes in simulation.
            _windowFactory.Destroy();
            _windowFactory = null;
            lock (_windowList)
            {
                _windowList.Clear();
            }

            // State factory only references parent Windows type, they are added directly to active Windows so no list of them here.
            _formFactory.Destroy();
            _formFactory = null;
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
        public override void OnTick(bool systemTick, bool skipDay)
        {
            // If the active Windows is not null and flag is set to remove then do that!
            var updatedModes = false;
            if (FocusedWindow != null && FocusedWindow.ShouldRemoveMode)
                updatedModes = CleanWindows();

            // When list of modes is updated then we need to activate now active Windows since they shifted.
            if (updatedModes)
                FocusedWindow.OnWindowActivate();

            // Otherwise just tick the game Windows logic.
            if (FocusedWindow != null)
                FocusedWindow.OnTick(systemTick, skipDay);
        }

        /// <summary>Creates and adds the specified type of state to currently active game Windows.</summary>
        /// <param name="parentMode">The parent Mode.</param>
        /// <param name="stateType">The state Type.</param>
        /// <returns>The <see cref="IForm" />.</returns>
        public IForm CreateStateFromType(IWindow parentMode, Type stateType)
        {
            return _formFactory.CreateStateFromType(stateType, parentMode);
        }

        /// <summary>
        ///     Removes any and all inactive game modes that need to be removed from the simulation.
        /// </summary>
        /// <returns>
        ///     TRUE if modes were removes, changing the active Windows or nulling it. FALSE if nothing changed because nothing
        ///     was removed or no modes.
        /// </returns>
        private bool CleanWindows()
        {
            lock (_windowList)
            {
                // Ensure the Windows exists as active Windows.
                if (FocusedWindow == null)
                    return false;

                // Init copy of all modes so we can destroy while iterating.
                var tempWindowList = new Dictionary<Type, IWindow>(_windowList);
                var updatedWindowList = false;
                foreach (var mode in tempWindowList)
                {
                    // Skip if the Windows doesn't want to be removed.
                    if (!mode.Value.ShouldRemoveMode)
                        continue;

                    // Remove the Windows from list if it is flagged for removal.
                    _windowList.Remove(mode.Key);
                    updatedWindowList = true;
                }

                // Clear temporary dictionary of modes
                tempWindowList.Clear();

                // Return the result of the Windows cleansing operation.
                return updatedWindowList;
            }
        }

        /// <summary>
        ///     Tell all the other game modes that we added another Windows.
        /// </summary>
        private void OnWindowAdded()
        {
            lock (_windowList)
            {
                var tempWindowList = new Dictionary<Type, IWindow>(_windowList);
                foreach (var loadedMode in tempWindowList)
                    if (loadedMode.Key == FocusedWindow.GetType())
                        loadedMode.Value.OnWindowPostCreate();
                    else
                        loadedMode.Value.OnWindowAdded();

                tempWindowList.Clear();
            }
        }

        /// <summary>
        ///     Creates and adds the specified game Windows to the simulation if it does not already exist in the list of
        ///     modes.
        /// </summary>
        /// <param name="window">Enumeration value of the Windows which should be created.</param>
        public void Add(Type window)
        {
            lock (_windowList)
            {
                // Check if any other modes match the one we are adding.
                if (_windowList.ContainsKey(window))
                {
                    // If Windows is attempted to be added we will fire activate for it so Windows knows it was added again without having to call post create.
                    _windowList[window].OnWindowActivate();
                    return;
                }

                // Init the game Windows using factory.
                var modeProduct = _windowFactory.CreateWindow(window);

                // Add the game Windows to the simulation now that we know it does not exist in the stack yet.
                _windowList.Add(window, modeProduct);
                OnWindowAdded();
            }
        }

        /// <summary>
        ///     Removes every window and form from the simulation and makes it a blank slate. Use with caution, if there is an
        ///     operation in progress, or waiting for user input this will not respect that and just forcefully destroy everything.
        /// </summary>
        public void Clear()
        {
            lock (_windowList)
            {
                _windowList.Clear();
            }
        }
    }
}