// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:10 PM

using System;
using System.Collections.Generic;
using Assets.Engine;
using UnityEngine;

namespace Assets.Game
{
    /// <summary>
    ///     Example console application using wolf curses library to power interaction.
    /// </summary>
    public class GameEngineApp : EngineApp
    {
        /// <summary>
        ///     Instance of this singleton.
        /// </summary>
        private static GameEngineApp _instance;

        /// <summary>
        ///     Determines if the current game is paused
        /// </summary>
        private bool _isPaused;

        /// <summary>
        ///     Returns the instance of this singleton.
        /// </summary>
        public new static GameEngineApp Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = (GameEngineApp) FindObjectOfType(typeof(GameEngineApp));

                if (_instance == null)
                    Debug.LogError("An instance of " + typeof(GameEngineApp) +
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

        }

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
        private void Awake()
        {
            // Clear out any previous debugging data.
            Debug.ClearDeveloperConsole();

            // Creates instance of the engine application.
            Debug.Log("----------PROGRAM START----------");

            // Prevent Unity from destroying this object when attaching new scenes.
            DontDestroyOnLoad(this);
        }

        /// <summary>
        ///     OnGUI is called for rendering and handling GUI events.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            if (_isPaused && _instance != null)
                Debug.Log("----------PROGRAM PAUSED----------");
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
            Debug.Log("----------PROGRAM END----------");
        }

        /// <summary>
        ///     Sent to all game objects before the application is quit.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationQuit()
        {
            Debug.Log("GameScript::OnApplicationQuit()");
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

        /// <summary>
        ///     Determines what windows the simulation will be capable of using and creating using the window managers factory.
        /// </summary>
        public override IEnumerable<Type> AllowedWindows
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Fired when the ticker receives the first system tick event.
        /// </summary>
        protected override void OnFirstTick()
        {
        }

        /// <summary>
        ///     Called when simulation is about to destroy itself, but right before it actually does it.
        /// </summary>
        protected override void OnShutdown()
        {
        }

        /// <summary>
        ///     Called by the text user interface scene graph renderer before it asks the active window to render itself out for
        ///     display.
        /// </summary>
        public override string OnPreRenderScene()
        {
            return null;
        }
    }
}