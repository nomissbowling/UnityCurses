﻿// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 12/31/2015@2:38 PM

using System;
using System.Text;
using Assets.Maxwolf.Engine;

namespace Assets.Maxwolf.WolfCurses
{
    /// <summary>
    ///     Provides base functionality for rendering out the simulation state via text user interface (TUI). This class has no
    ///     idea about how other modules work and only serves to query them for string data which will be compiled into a
    ///     console only view of the simulation which is intended to be the lowest level of visualization but theoretically
    ///     anything could be a renderer for the simulation.
    /// </summary>
    public class SceneGraph : Module.Module
    {
        /// <summary>
        ///     Fired when the screen back buffer has changed from what is currently being shown, this forces a redraw.
        /// </summary>
        public delegate void ScreenBufferDirty(string tuiContent);

        /// <summary>
        ///     Default string used when game Windows has nothing better to say.
        /// </summary>
        private const string GAMEMODE_DEFAULT_TUI = "[DEFAULT WINDOW TEXT]";

        /// <summary>
        ///     Default string used when there are no game modes at all.
        /// </summary>
        private const string GAMEMODE_EMPTY_TUI = "[NO WINDOW ATTACHED]";

        /// <summary>
        ///     Default string that is used in menu generations when the user is given a choice. Can be changed per window or form.
        /// </summary>
        public const string PROMPT_TEXT_DEFAULT = "What is your choice?";

        /// <summary>
        ///     Reference to simulation that is controlling the text user interface and actually filling the screen buffer with
        ///     data.
        /// </summary>
        private readonly EngineApp _simUnit;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SceneGraph" /> class.
        /// </summary>
        /// <param name="simUnit">Core simulation which is controlling the window manager.</param>
        public SceneGraph(EngineApp simUnit)
        {
            _simUnit = simUnit;
            ScreenBuffer = string.Empty;
        }

        /// <summary>
        ///     Holds the last known representation of the game simulation and current Windows text user interface, only pushes
        ///     update
        ///     when a change occurs.
        /// </summary>
        private string ScreenBuffer { get; set; }

        /// <summary>
        ///     Fired when the simulation is closing and needs to clear out any data structures that it created so the program can
        ///     exit cleanly.
        /// </summary>
        public override void Destroy()
        {
            ScreenBuffer = string.Empty;
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
            // GetModule the current text user interface data from inheriting class.
            var tuiContent = OnRender();
            if (ScreenBuffer.Equals(tuiContent, StringComparison.OrdinalIgnoreCase))
                return;

            // Update the screen buffer with altered data.
            ScreenBuffer = tuiContent;
            if (ScreenBufferDirtyEvent != null)
                ScreenBufferDirtyEvent.Invoke(ScreenBuffer);
        }

        /// <summary>
        ///     Prints game Windows specific text and options.
        /// </summary>
        /// <returns>
        ///     The text user interface that is the game simulation.<see cref="string" />.
        /// </returns>
        private string OnRender()
        {
            // Spinning ticker that shows activity, lets us know if application hangs or freezes.
            var tui = new StringBuilder();
            tui.Append(string.Format("[ {0} ] - ", EngineApp.TickPhase));

            // Keeps track of active Windows name and active Windows current state name for debugging purposes.
            tui.Append(EngineApp.WindowManager.FocusedWindow != null &&
                       EngineApp.WindowManager.FocusedWindow.CurrentForm != null
                ? string.Format("Window({0}): {1}({2}) - ", EngineApp.WindowManager.Count,
                    EngineApp.WindowManager.FocusedWindow, EngineApp.WindowManager.FocusedWindow.CurrentForm)
                : string.Format("Window({0}): {1}() - ", EngineApp.WindowManager.Count,
                    EngineApp.WindowManager.FocusedWindow));

            // Allows the implementing simulation to control text before window is rendered out.
            tui.Append(_simUnit.OnPreRender());

            // Prints game Windows specific text and options. This typically is menus from commands, or states showing some information.
            tui.Append(string.Format("{0}{1}", RenderWindow(), Environment.NewLine));

            // Determines if the user is allowed to see their input from buffer as they type it, or is it stored until they press enter.
            if (EngineApp.WindowManager.AcceptingInput)
                tui.Append(EngineApp.WindowManager.FocusedWindow != null
                    ? string.Format("{0} {1}", EngineApp.WindowManager.FocusedWindow.PromptText,
                        EngineApp.InputManager.InputBuffer)
                    : string.Format("{0} {1}", PROMPT_TEXT_DEFAULT, EngineApp.InputManager.InputBuffer));

            // Outputs the result of the string builder to TUI builder above.
            return tui.ToString();
        }

        /// <summary>Prints game Windows specific text and options.</summary>
        /// <returns>The current window text to be rendered out.<see cref="string" />.</returns>
        private string RenderWindow()
        {
            // If TUI for active game Windows is not null or empty then use it.
            // ReSharper disable once InvertIf
            if (EngineApp.WindowManager.FocusedWindow != null)
            {
                var activeWindowText = EngineApp.WindowManager.FocusedWindow.OnRenderWindow();
                if (!string.IsNullOrEmpty(activeWindowText))
                    return activeWindowText;
            }

            // Otherwise, display default message if null for Windows.
            return EngineApp.WindowManager.FocusedWindow == null ? GAMEMODE_EMPTY_TUI : GAMEMODE_DEFAULT_TUI;
        }

        /// <summary>
        ///     Fired when the screen back buffer has changed from what is currently being shown, this forces a redraw.
        /// </summary>
        public event ScreenBufferDirty ScreenBufferDirtyEvent;

        /// <summary>
        ///     Removes any and all data from the text user interface renderer.
        /// </summary>
        public void Clear()
        {
            ScreenBuffer = string.Empty;
        }
    }
}