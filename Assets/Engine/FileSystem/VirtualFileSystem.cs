using System;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Assets.Engine.FileSystem
{
    /// <summary>
    ///     Keeps track of file locations on users disk across whatever platforms Unity supports using the persistent data
    ///     path.
    /// </summary>
    public static class VirtualFileSystem
    {
        internal static readonly object FileLock = new object();
        internal static bool Started;

        private static bool _loggingFileOperations;

        public static bool LoggingFileOperations
        {
            get { return _loggingFileOperations; }
            set { _loggingFileOperations = value; }
        }

        /// <summary>
        ///     Contains the path to the game data folder (Read Only).
        /// </summary>
        public static string ResourceDirectoryPath
        {
            get { return Application.dataPath; }
        }

        /// <summary>
        ///     Contains the path to the game executable data folder (Read Only).
        /// </summary>
        public static string ExecutableDirectoryPath
        {
            get
            {
                var path = Application.dataPath;
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXPlayer:
                        path += "/../../";
                        break;
                    case RuntimePlatform.WindowsPlayer:
                        path += "/../";
                        break;
                    //case RuntimePlatform.LinuxPlayer:
                    //    path += "/../";
                    //    break;
                }

                return path;
            }
        }

        /// <summary>
        ///     Determines if the project is not in editor and currently running in any player runtime.
        /// </summary>
        public static bool Deployed
        {
            get { return Application.isPlaying && !Application.isEditor; }
        }

        /// <summary>
        ///     Contains the path to a persistent data directory (Read Only).
        /// </summary>
        public static string UserDirectoryPath
        {
            get { return Application.persistentDataPath; }
        }

        /// <summary>
        ///     Determines if the path is a user directory path.
        /// </summary>
        public static bool IsUserDirectoryPath(string path)
        {
            return (path.Length >= 5) && (path[4] == 58) && (path.Substring(0, 5) == "user:");
        }

        /// <summary>Reset the current directory of the application.</summary>
        public static void _CorrectCurrentDirectory()
        {
            if ((Application.platform != RuntimePlatform.WindowsPlayer) &&
                (Application.platform != RuntimePlatform.WindowsEditor))
                return;

            Directory.SetCurrentDirectory(ExecutableDirectoryPath);
        }

        public static bool Init()
        {
            // Set culture to en-US to prevent strange things happening with numbers and thousands-separators.
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            Debug.Log("VirtualFileSystem::Init()");
            Started = true;
            return Started;
        }

        public static void Shutdown()
        {
            Debug.Log("VirtualFileSystem::Shutdown()");
        }

        /// <summary>
        ///     Converts a file path of virtual file system to path of real file system.
        /// </summary>
        /// <param name="virtualPath">The virtual file path.</param>
        /// <returns>The real file path.</returns>
        public static string GetRealPathByVirtual(string virtualPath)
        {
            if ((Application.platform != RuntimePlatform.WindowsPlayer) &&
                (Application.platform != RuntimePlatform.WindowsEditor))
                virtualPath = virtualPath.Replace('\\', '/');

            if ((virtualPath.Length >= 5) && (virtualPath[4] == 58) && (virtualPath.Substring(0, 5) == "user:"))
                return Path.Combine(UserDirectoryPath, virtualPath.Substring(5));
            return Path.Combine(ResourceDirectoryPath, virtualPath);
        }

        /// <summary>
        ///     Converts a file path of real file system to path of virtual file system.
        /// </summary>
        /// <param name="realPath">The real file path.</param>
        /// <returns>The virtual file path.</returns>
        public static string GetVirtualPathByReal(string realPath)
        {
            if (realPath == null)
                Debug.LogError("VirtualFileSystem: GetVirtualPathByReal: realPath == null.");

            realPath = NormalizePath(realPath);
            var str = Path.IsPathRooted(realPath) ? realPath : Path.Combine(ExecutableDirectoryPath, realPath);

            if ((str.Length <= ResourceDirectoryPath.Length) ||
                !string.Equals(str.Substring(0, ResourceDirectoryPath.Length), ResourceDirectoryPath,
                    StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            return str.Substring(ResourceDirectoryPath.Length + 1, str.Length - ResourceDirectoryPath.Length - 1);
        }

        public static string NormalizePath(string path)
        {
            var str = path;

            if (str != null)
                str = (Application.platform != RuntimePlatform.WindowsPlayer) &&
                      (Application.platform != RuntimePlatform.WindowsEditor)
                    ? str.Replace('\\', '/')
                    : str.Replace('/', '\\');

            return str;
        }
    }
}