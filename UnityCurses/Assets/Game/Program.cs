// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:10 PM

using System;
using System.Threading;
using Assets.Engine;
using Assets.Example;

namespace Assets.Game
{
    /// <summary>
    ///     Example console application using wolf curses library to power interaction.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     GameEngineApp entry point for the application being startup.
        /// </summary>
        private static void Main(string[] args)
        {
            // Init console with title, no cursor, make CTRL-C act as input.
            Console.Title = "WolfCurses Console Application";
            Console.WriteLine("Starting...");
            Console.CursorVisible = false;
            Console.CancelKeyPress += Console_CancelKeyPress;

            // Entry point for the entire simulation.
            EngineApp.Init(new ExampleApp());
            //EngineApp.Init(new OregonTrailApp());

            // Hook event to know when screen buffer wants to redraw the entire console screen.
            EngineApp.Instance.SceneGraph.ScreenBufferDirtyEvent += Simulation_ScreenBufferDirtyEvent;

            // Prevent console session from closing.
            while (EngineApp.Instance != null)
            {
                // Simulation takes any numbers of pulses to determine seconds elapsed.
                EngineApp.Instance.OnTick(true, false);

                // Check if a key is being pressed, without blocking thread.
                if (Console.KeyAvailable)
                {
                    // GetModule the key that was pressed, without printing it to console.
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    // If enter is pressed, pass whatever we have to simulation.
                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            EngineApp.Instance.InputManager.SendInputBufferAsCommand();
                            break;
                        case ConsoleKey.Backspace:
                            EngineApp.Instance.InputManager.RemoveLastCharOfInputBuffer();
                            break;
                        default:
                            EngineApp.Instance.InputManager.AddCharToInputBuffer(key.KeyChar);
                            break;
                    }
                }

                // Do not consume all of the CPU, allow other messages to occur.
                Thread.Sleep(1);
            }

            // Make user press any key to close out the simulation completely, this way they know it closed without error.
            Console.Clear();
            Console.WriteLine("Goodbye!");
            Console.WriteLine("Press ANY KEY to close this window...");
            Console.ReadKey();
        }

        /// <summary>Write all text from objects to screen.</summary>
        /// <param name="tuiContent">The text user interface content.</param>
        private static void Simulation_ScreenBufferDirtyEvent(string tuiContent)
        {
            string[] tuiContentSplit = tuiContent.Split(new string[] {Environment.NewLine}, StringSplitOptions.None);

            for (int index = 0; index < Console.WindowHeight - 1; index++)
            {
                Console.CursorLeft = 0;
                Console.SetCursorPosition(0, index);

                string emptyStringData = new string(' ', Console.WindowWidth);

                if (tuiContentSplit.Length > index)
                    emptyStringData = tuiContentSplit[index].PadRight(Console.WindowWidth);

                Console.Write(emptyStringData);
            }
        }

        /// <summary>
        ///     Fired when the user presses CTRL-C on their keyboard, this is only relevant to operating system tick and this view
        ///     of simulation. If moved into another framework like game engine this statement would be removed and just destroy
        ///     the simulation when the engine is destroyed using its overrides.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            // Shutdown the simulation.
            EngineApp.Shutdown();

            // Stop the operating system from killing the entire process.
            e.Cancel = true;
        }

        /// <summary>
        ///     Forces the current simulation app to close and return control to underlying operating system.
        /// </summary>
        public static void Destroy()
        {
            EngineApp.Shutdown();
        }
    }
}