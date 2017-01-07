// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:10 PM

using Assets.Maxwolf.Engine;
using Assets.Maxwolf.Engine.FileSystem;
using Assets.Maxwolf.Example;
using Assets.Maxwolf.OregonTrail;
using Assets.Maxwolf.ProjectCommon;
using Assets.Maxwolf.ProjectCommon.Utility.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Maxwolf.Game
{
    [Prefab("GameController")]
    public sealed class GameController : UnitySingleton<GameController>
    {
        /// <summary>
        ///     Reference to the text that is going to be shown on the text component.
        /// </summary>
        private static string _screenBuffer;

        /// <summary>
        ///     Reference to scene UI text input field. Assigned from editor.
        /// </summary>
        public InputField GameInput;

        /// <summary>
        ///     Reference to scene UI output text field. Assigned from editor.
        /// </summary>
        public Text GameOutput;

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            Debug.Log("GameScript::Update()");

            // Ticks the underlying simulation.
            if (EngineApp.Instance != null)
                EngineApp.Instance.OnTick(true, false);

            // Tick the control manager for input strength.
            if (GameControlsManager.Instance != null)
                GameControlsManager.Instance.DoTick(Time.deltaTime);

            // Special hooks for low-level keys such as return, backspace, and generic input for engine application.
            // ReSharper disable once SwitchStatementMissingSomeCases
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                EngineApp.InputManager.SendInputBufferAsCommand();

                // Clear out input field.
                if (GameInput != null)
                    GameInput.text = string.Empty;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (EngineApp.Instance != null)
                    EngineApp.InputManager.RemoveLastCharOfInputBuffer();
            }
            else if (Input.anyKeyDown && EngineApp.Instance != null)
            {
                char inputChar;
                if (char.TryParse(Input.inputString, out inputChar))
                {
                    EngineApp.InputManager.AddCharToInputBuffer(inputChar);
                    GameInput.text = EngineApp.InputManager.InputBuffer;
                }
            }

            // Check if game output is different, if so then set it to that.
            if (GameOutput != null)
                GameOutput.text = _screenBuffer;
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
            Debug.Log("----------PROGRAM START----------");

            // Clear out controls.
            if (GameOutput != null)
                GameOutput.text = string.Empty;

            // Set game input to match input buffer.
            if (GameInput != null)
                GameInput.onValueChanged.AddListener(delegate { MatchInputBufferToInputField(); });

            // Create virtual filesystem.
            VirtualFileSystem.Init();

            // Create keybinding remapper system.
            GameControlsManager.Init();

            // Hook event.
            if (GameControlsManager.Instance != null)
                GameControlsManager.Instance.GameControlsEvent += GameControlsManager_GameControlsEvent;

            // Entry point for the entire simulation.
            //EngineApp.Init(new ExampleApp());
            EngineApp.Init(new OregonTrailApp());

            // Hook event to know when screen buffer wants to redraw the entire console screen.
            EngineApp.SceneGraph.ScreenBufferDirtyEvent += Simulation_ScreenBufferDirtyEvent;
        }

        private void MatchInputBufferToInputField()
        {
            if (EngineApp.InputManager.InputBuffer != GameInput.text)
            {
                GameInput.text = EngineApp.InputManager.InputBuffer;
            }
        }

        private void GameControlsManager_GameControlsEvent(GameControlsEventData e)
        {
            var keyDown = e as GameControlsKeyDownEventData;
            if (keyDown != null)
                Debug.Log(keyDown.ControlKey.ToString().ToUpperInvariant() + "_DOWN");

            var keyUp = e as GameControlsKeyUpEventData;
            if (keyUp != null)
                Debug.Log(keyUp.ControlKey.ToString().ToUpperInvariant() + "_UP");
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            Destroy();
        }

        /// <summary>
        ///     Cleanup and closedown the engine, this method can be called from multiple places by Unity and cannot know which one
        ///     will come first.
        /// </summary>
        private void Destroy()
        {
            // Filesystem.
            if (VirtualFileSystem.Instance != null)
                VirtualFileSystem.Shutdown();

            // Input manager.
            if (GameControlsManager.Instance != null)
            {
                GameControlsManager.Instance.GameControlsEvent -= GameControlsManager_GameControlsEvent;
                GameControlsManager.Shutdown();
            }

            // Tick and window manager.
            EngineApp.Shutdown();
        }

        /// <summary>
        ///     Sent to all game objects before the application is quit.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnApplicationQuit()
        {
            Destroy();
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
            // Update static text we will be using on the game object reference.
            _screenBuffer = tuiContent;
        }
    }
}