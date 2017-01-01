using UnityEngine;

namespace Assets.Scripts.Engine.UISystem
{
    /// <summary>
    ///     Used to attach other GUI controls to it and manage the lifecycle of these objects in relation to the current game
    ///     mode being played. This a screen overlay and will scale over 3D objects in a given scene.
    /// </summary>
    public sealed class ScreenControlManager : MonoBehaviour
    {
        /// <summary>
        ///     OnGUI is called for rendering and handling GUI events.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            Debug.Log("ScreenControlManager::OnGUI()");
        }

        /// <summary>
        ///     Start is called on the frame when a script is enabled just before any of the Update methods is called the first
        ///     time.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            Debug.Log("ScreenControlManager::Start()");
        }

        /// <summary>
        ///     Reset to default values.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Reset()
        {
            Debug.Log("ScreenControlManager::Reset()");
        }

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
        }
    }
}