// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:10 PM

using Assets.Source.Engine;
using Assets.Source.Example;
using Assets.Source.ProjectCommon.Keybind;
using Assets.Source.ProjectCommon.Singleton;
using UnityEngine;

namespace Assets.Source.Game
{
    [Prefab("GameController")]
    public sealed class GameController : UnitySingleton<GameController>
    {
        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            Debug.Log("GameScript::Update()");

            // Skip if the engine is not currently initialized.
            if (EngineApp.Instance == null)
                return;

            // Tick the control manager for input strength.
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
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        protected override void Awake()
        {
            Debug.Log("----------PROGRAM START----------");

            // Create keybinding remapper system.
            GameControlsManager.Init();

            // Hook event.
            if (GameControlsManager.Instance != null)
                GameControlsManager.Instance.GameControlsEvent += GameControlsManager_GameControlsEvent;

            // Entry point for the entire simulation.
            EngineApp.Init(new ExampleApp());
            //EngineApp.Init(new OregonTrailApp());

            // Hook event to know when screen buffer wants to redraw the entire console screen.
            //EngineApp.Instance.SceneGraph.ScreenBufferDirtyEvent += Simulation_ScreenBufferDirtyEvent;
        }

        private void GameControlsManager_GameControlsEvent(GameControlsEventData e)
        {
            var keyDown = e as GameControlsKeyDownEventData;
            if (keyDown != null)
            {
                switch (keyDown.ControlKey)
                {
                    case GameControlKeys.Fire1:
                        Debug.Log("FIRE1 DOWN!");
                        break;
                    case GameControlKeys.Forward:
                        Debug.Log("FORWARD DOWN!");
                        break;
                    case GameControlKeys.Backward:
                        Debug.Log("BACKWARD DOWN!");
                        break;
                    case GameControlKeys.Left:
                        Debug.Log("LEFT DOWN!");
                        break;
                    case GameControlKeys.Right:
                        Debug.Log("RIGHT DOWN!");
                        break;
                    case GameControlKeys.Fire2:
                        Debug.Log("FIRE2 DOWN!");
                        break;
                    case GameControlKeys.Reload:
                        Debug.Log("RELOAD DOWN!");
                        break;
                    case GameControlKeys.Use:
                        Debug.Log("USE DOWN!");
                        break;
                }
            }

            var keyUp = e as GameControlsKeyUpEventData;
            if (keyUp != null)
            {
                switch (keyUp.ControlKey)
                {
                    case GameControlKeys.Fire1:
                        Debug.Log("FIRE1 UP!");
                        break;
                    case GameControlKeys.Forward:
                        Debug.Log("FORWARD UP!");
                        break;
                    case GameControlKeys.Backward:
                        Debug.Log("BACKWARD UP!");
                        break;
                    case GameControlKeys.Left:
                        Debug.Log("LEFT UP!");
                        break;
                    case GameControlKeys.Right:
                        Debug.Log("RIGHT UP!");
                        break;
                    case GameControlKeys.Fire2:
                        Debug.Log("FIRE2 UP!");
                        break;
                    case GameControlKeys.Reload:
                        Debug.Log("RELOAD UP!");
                        break;
                    case GameControlKeys.Use:
                        Debug.Log("USE UP!");
                        break;
                }
            }
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
            Destroy();
        }

        /// <summary>
        ///     Cleanup and closedown the engine, this method can be called from multiple places by Unity and cannot know which one
        ///     will come first.
        /// </summary>
        private void Destroy()
        {
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