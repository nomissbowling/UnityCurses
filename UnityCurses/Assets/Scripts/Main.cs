// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/07/2016@7:10 PM

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
            Debug.Log("----------PROGRAM START----------");

            // Prevent Unity from destroying this object when attaching new scenes.
            DontDestroyOnLoad(this);
        }

        public static Canvas Canvas { get; set; }

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
            Debug.Log("GameScript::Awake()");
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
    }
}