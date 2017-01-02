using UnityEngine;
using UnityEngine.UI;

namespace Assets.Engine.UISystem
{
    /// <summary>
    ///     Deals with attaching various GUI controls such as FPS controls and other special interactions via this system so
    ///     that it can bind to the wolf curses style of window management to Unity with this class acting as the bridge
    ///     between the two systems.
    /// </summary>
    public class ScreenControlManager
    {
        /// <summary>
        ///     Holds reference to the canvas object which all GUI controls are attached to while the game is running.
        /// </summary>
        private Canvas _controlsCanvas;

        public ScreenControlManager(Canvas guiRenderer)
        {
            // Find the Canvas which should be attached to this object.
            _controlsCanvas = guiRenderer;
            if (_controlsCanvas == null)
                return;
        }

        /// <summary>
        ///     Holds reference to the canvas object which all GUI controls are attached to while the game is running.
        /// </summary>
        public Canvas Controls
        {
            get { return _controlsCanvas; }
            set { _controlsCanvas = value; }
        }

        /// <summary>
        ///     Adds a piece of text to a given game canvas object.
        /// </summary>
        /// <remarks>http://answers.unity3d.com/answers/1084894/view.html</remarks>
        public void AddTextToCanvas(string textString)
        {
            if (_controlsCanvas == null)
                return;

            var text = _controlsCanvas.gameObject.AddComponent<Text>();
            text.text = textString;

            var arialFont = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.font = arialFont;
            text.material = arialFont.material;
        }

        public void PlaySound(string name)
        {
            var audio = _controlsCanvas.gameObject.AddComponent<AudioSource>();
            var clip = (AudioClip) Resources.Load(name);
            if (clip != null)
                audio.PlayOneShot(clip, 1.0F);
            else
                Debug.Log("Attempted to play missing audio clip by name" + name);
        }
    }
}