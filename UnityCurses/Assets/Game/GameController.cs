// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:10 PM

using System;
using Assets.Common;
using Assets.Engine;
using Assets.Example;
using UnityEngine;

namespace Assets.Game
{
    [Prefab("Game Controller")]
    public class GameController : UnitySingleton<GameController>
    {
        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            
        }

        /// <summary>
        ///     OnPreRender is called before a camera starts rendering the scene.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnPreRender()
        {
            //Debug.Log("ExampleScript::OnPreRender()");
        }

        /// <summary>
        ///     OnPostRender is called after a camera finished rendering the scene.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnPostRender()
        {
            //Debug.Log("ExampleScript::OnPostRender()");
        }

        /// <summary>
        ///     Reset to default values.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Reset()
        {
            Debug.Log("GameScript::Reset()");
        }

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        protected override void Awake()
        {
            Debug.Log("GameScript::Awake()");

            // Init console with title, no cursor, make CTRL-C act as input.
            //Console.Title = "WolfCurses Console Application";
            //Console.WriteLine("Starting...");
            //Console.CursorVisible = false;
            //Console.CancelKeyPress += Console_CancelKeyPress;

            // Entry point for the entire simulation.
            EngineApp.Init(new ExampleApp());
            //EngineApp.Init(new OregonTrailApp());

            // Hook event to know when screen buffer wants to redraw the entire console screen.
            EngineApp.Instance.SceneGraph.ScreenBufferDirtyEvent += Simulation_ScreenBufferDirtyEvent;

            //// Prevent console session from closing.
            //while (EngineApp.Instance != null)
            //{
            //    // Simulation takes any numbers of pulses to determine seconds elapsed.
            //    EngineApp.Instance.OnTick(true, false);

            //    // Check if a key is being pressed, without blocking thread.
            //    if (Console.KeyAvailable)
            //    {
            //        // GetModule the key that was pressed, without printing it to console.
            //        ConsoleKeyInfo key = Console.ReadKey(true);

            //        // If enter is pressed, pass whatever we have to simulation.
            //        // ReSharper disable once SwitchStatementMissingSomeCases
            //        switch (key.Key)
            //        {
            //            case ConsoleKey.Enter:
            //                EngineApp.Instance.InputManager.SendInputBufferAsCommand();
            //                break;
            //            case ConsoleKey.Backspace:
            //                EngineApp.Instance.InputManager.RemoveLastCharOfInputBuffer();
            //                break;
            //            default:
            //                EngineApp.Instance.InputManager.AddCharToInputBuffer(key.KeyChar);
            //                break;
            //        }
            //    }

            //    // Do not consume all of the CPU, allow other messages to occur.
            //    Thread.Sleep(1);
            //}

            //// Make user press any key to close out the simulation completely, this way they know it closed without error.
            //Console.Clear();
            //Console.WriteLine("Goodbye!");
            //Console.WriteLine("Press ANY KEY to close this window...");
            //Console.ReadKey();
        }

        /// <summary>
        ///     OnGUI is called for rendering and handling GUI events.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            Debug.Log("GameScript::OnGUI()");
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            EngineApp.Shutdown();
            Debug.Log("GameScript::OnDestroy()");
        }

        /// <summary>
        ///     Sent to all game objects before the application is quit.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationQuit()
        {
            // Cleans up simulation
            EngineApp.Shutdown();

            Debug.Log("GameScript::OnApplicationQuit()");
            Debug.Log("Goodbye!");
        }

        /// <summary>
        ///     Sent to all GameObjects when the application pauses.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationPause(bool pauseStatus)
        {
            Debug.Log("GameScript::OnApplicationPause()");
        }

        /// <summary>
        ///     Sent to all GameObjects when the player gets or loses focus.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationFocus(bool hasFocus)
        {
            Debug.Log("GameScript::OnApplicationFocus()");
        }

        /// <summary>Write all text from objects to screen.</summary>
        /// <param name="tuiContent">The text user interface content.</param>
        private static void Simulation_ScreenBufferDirtyEvent(string tuiContent)
        {
            //string[] tuiContentSplit = tuiContent.Split(new string[] {Environment.NewLine}, StringSplitOptions.None);

            //for (int index = 0; index < Console.WindowHeight - 1; index++)
            //{
            //    Console.CursorLeft = 0;
            //    Console.SetCursorPosition(0, index);

            //    string emptyStringData = new string(' ', Console.WindowWidth);

            //    if (tuiContentSplit.Length > index)
            //        emptyStringData = tuiContentSplit[index].PadRight(Console.WindowWidth);

            //    Console.Write(emptyStringData);
            //}
        }
    }
}