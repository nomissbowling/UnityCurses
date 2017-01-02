// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:10 PM

using System;
using Assets.Game;
using Assets.ProjectCommon;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    ///     Example console application using wolf curses library to power interaction.
    /// </summary>
    public class Main : MonoBehaviour
    {
        /// <summary>
        ///     Instance of this singleton.
        /// </summary>
        private static Main _instance;

        /// <summary>
        ///     Determines if the current game is paused
        /// </summary>
        private bool _isPaused;

        /// <summary>
        ///     Returns the instance of this singleton.
        /// </summary>
        public static Main Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = (Main) FindObjectOfType(typeof(Main));

                if (_instance == null)
                    Debug.LogError("An instance of " + typeof(Main) +
                                   " is needed in the scene, but there is none.");

                return _instance;
            }
        }

        /// <summary>
        ///     Determines if the current game is paused
        /// </summary>
        public bool Paused
        {
            get { return _isPaused; }
        }

        /// <summary>
        ///     Start is called on the frame when a script is enabled just before any of the Update methods is called the first
        ///     time.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            // Clear out any previous debugging data.
            Debug.ClearDeveloperConsole();

            // Creates instance of the engine application.
            Debug.Log("EngineApp Starting...");

            // Prevent Unity from destroying this object when attaching new scenes.
            DontDestroyOnLoad(this);

            // Entry point for the entire simulation.
            GameEngineApp.Create(GetComponentInParent<Canvas>());

            // Hook event to know when screen buffer wants to redraw the entire console screen.
            GameEngineApp.Instance.SceneGraph.ScreenBufferDirtyEvent += Simulation_ScreenBufferDirtyEvent;

            // Accept commands of the player.
            if (GameControlsManager.Instance != null)
                GameControlsManager.Instance.GameControlsEvent += GameControlsManager_GameControlsEvent;
        }

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            // Skip if the example application is not ready to execute.
            if (GameEngineApp.Instance == null)
                return;

            // Send to currently active window and form if they exist.
            var e = Event.current;
            if ((e != null) && e.isKey)
                switch (e.keyCode)
                {
                    case KeyCode.Return:
                        GameEngineApp.Instance.InputManager.SendInputBufferAsCommand();
                        break;
                    case KeyCode.Backspace:
                        GameEngineApp.Instance.InputManager.RemoveLastCharOfInputBuffer();
                        break;
                    default:
                        GameEngineApp.Instance.InputManager.AddCharToInputBuffer(e.character);
                        break;
                }

            // Simulation takes any numbers of pulses to determine seconds elapsed.
            GameEngineApp.Instance.OnTick(true, false);

            // Provides the time since last unity tick for control manager.
            if (GameControlsManager.Instance != null)
                GameControlsManager.Instance.DoTick(Time.deltaTime);
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
        ///     Interprets commands of an external influence such as the player using his keyboard and mouse.
        /// </summary>
        /// <param name="e">Incoming key data that needs to be interpreted into correct key-up or key-down events.</param>
        private void GameControlsManager_GameControlsEvent(GameControlsEventData e)
        {
            // Skip if the example application is not ready to execute.
            if (GameEngineApp.Instance == null)
                return;

            //GameControlsKeyDownEventData
            {
                var evt = e as GameControlsKeyDownEventData;
                if (evt != null)
                {
                    OnKeyDown(evt);
                    return;
                }
            }

            //GameControlsKeyUpEventData
            {
                var evt = e as GameControlsKeyUpEventData;
                OnKeyUp(evt);
            }
        }

        protected virtual void OnKeyUp(GameControlsKeyUpEventData evt)
        {
            if (evt == null)
                return;

            Debug.Log("GameScript::OnKeyUp: " + evt.ControlKey);
        }

        protected virtual void OnKeyDown(GameControlsKeyDownEventData evt)
        {
            if (evt == null)
                return;

            Debug.Log("GameScript::OnKeyDown: " + evt.ControlKey);
        }

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            Debug.Log("GameScript::Awake()");
        }

        /// <summary>
        ///     OnGUI is called for rendering and handling GUI events.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            if (_isPaused && (GameEngineApp.Instance != null))
                GameEngineApp.Instance.ControlManager.AddTextToCanvas("Game paused");
        }

        /// <summary>
        ///     This function is called when the object becomes enabled and active.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnEnable()
        {
            Debug.Log("GameScript::OnEnable()");
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            Debug.Log("GameScript::OnDestroy()");
        }

        /// <summary>
        ///     Sent to all game objects before the application is quit.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationQuit()
        {
            // Cleans up simulation
            if (GameEngineApp.Instance != null)
                GameEngineApp.Instance.Destroy();

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
            _isPaused = pauseStatus;
        }

        /// <summary>
        ///     Sent to all GameObjects when the player gets or loses focus.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationFocus(bool hasFocus)
        {
            Debug.Log("GameScript::OnApplicationFocus()");
            _isPaused = !hasFocus;
        }

        /// <summary>Write all text from objects to screen.</summary>
        /// <param name="tuiContent">The text user interface content.</param>
        private static void Simulation_ScreenBufferDirtyEvent(string tuiContent)
        {
            GameEngineApp.Instance.ControlManager.AddTextToCanvas(tuiContent);
        }

        /// <summary>
        ///     Forces the current simulation app to close and return control to underlying operating system.
        /// </summary>
        public static void Destroy()
        {
            // Cleans up simulation
            if (GameEngineApp.Instance != null)
                GameEngineApp.Instance.Destroy();
        }
    }
}