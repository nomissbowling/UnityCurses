// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:10 PM

using System;
using UnityEngine;

namespace Assets.Scripts.Example
{
    /// <summary>
    ///     Example console application using wolf curses library to power interaction.
    /// </summary>
    public sealed class ExampleScript<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        ///     Instance of this singleton.
        /// </summary>
        private static T _instance;

        /// <summary>
        ///     Returns the instance of this singleton.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = (T) FindObjectOfType(typeof(T));

                if (_instance == null)
                    Debug.LogError("An instance of " + typeof(T) +
                                   " is needed in the scene, but there is none.");

                return _instance;
            }
        }

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (Input.GetKeyDown("space"))
                print("space key was pressed");

            //Debug.Log("ExampleScript::Update()");

            // Simulation takes any numbers of pulses to determine seconds elapsed.
            ExampleApp.Instance.OnTick(true);

            // Check if a key is being pressed, without blocking thread.
            if (Console.KeyAvailable)
            {
                // GetModule the key that was pressed, without printing it to console.
                var key = Console.ReadKey(true);

                // If enter is pressed, pass whatever we have to simulation.
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        ExampleApp.Instance.InputManager.SendInputBufferAsCommand();
                        break;
                    case ConsoleKey.Backspace:
                        ExampleApp.Instance.InputManager.RemoveLastCharOfInputBuffer();
                        break;
                    default:
                        ExampleApp.Instance.InputManager.AddCharToInputBuffer(key.KeyChar);
                        break;
                }
            }
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
            Debug.Log("ExampleScript::Reset()");
        }

        /// <summary>
        ///     Start is called on the frame when a script is enabled just before any of the Update methods is called the first
        ///     time.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            // Creates instance of the engine application.
            Debug.Log("EngineApp Example Script");
            Debug.Log("Starting...");

            // Entry point for the entire simulation.
            ExampleApp.Create();

            // Hook event to know when screen buffer wants to redraw the entire console screen.
            ExampleApp.Instance.SceneGraph.ScreenBufferDirtyEvent += Simulation_ScreenBufferDirtyEvent;

            // Clear out any previous debugging data.
            Debug.ClearDeveloperConsole();
        }

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            Debug.Log("ExampleScript::Awake()");
        }

        /// <summary>
        ///     OnGUI is called for rendering and handling GUI events.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            Debug.Log("ExampleScript::OnGUI()");
        }

        /// <summary>
        ///     This function is called when the object becomes enabled and active.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnEnable()
        {
            Debug.Log("ExampleScript::OnEnable()");
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            Debug.Log("ExampleScript::OnDestroy()");
        }

        /// <summary>
        ///     Sent to all game objects before the application is quit.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationQuit()
        {
            Debug.Log("OnApplicationQuit::OnApplicationQuit()");
            Debug.Log("Goodbye!");
        }

        /// <summary>
        ///     Sent to all GameObjects when the application pauses.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationPause()
        {
            Debug.Log("ExampleScript::OnApplicationPause()");
        }

        /// <summary>
        ///     Sent to all GameObjects when the player gets or loses focus.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationFocus()
        {
            Debug.Log("ExampleScript::OnApplicationFocus()");
        }

        /// <summary>Write all text from objects to screen.</summary>
        /// <param name="tuiContent">The text user interface content.</param>
        private static void Simulation_ScreenBufferDirtyEvent(string tuiContent)
        {
            var tuiContentSplit = tuiContent.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            for (var index = 0; index < Console.WindowHeight - 1; index++)
            {
                Console.CursorLeft = 0;
                Console.SetCursorPosition(0, index);

                var emptyStringData = new string(' ', Console.WindowWidth);

                if (tuiContentSplit.Length > index)
                    emptyStringData = tuiContentSplit[index].PadRight(Console.WindowWidth);

                Console.Write(emptyStringData);
            }
        }

        /// <summary>
        ///     Forces the current simulation app to close and return control to underlying operating system.
        /// </summary>
        public static void Destroy()
        {
            // Cleans up simulation
            ExampleApp.Instance.Destroy();

            // Closes unity engine.
            Application.Quit();
        }
    }
}