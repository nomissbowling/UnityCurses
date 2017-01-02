using System;
using System.IO;
using UnityEngine;

namespace Assets.Engine.FileSystem
{
    /// <summary>Defines a file for virtual file system.</summary>
    public static class VirtualFile
    {
        /// <summary>Determines whether the specified file exists.</summary>
        /// <param name="path">The file to check.</param>
        /// <returns><b>true</b> if the file is exists; otherwise, <b>false</b>.</returns>
        public static bool Exists(string path)
        {
            lock (VirtualFileSystem.FileLock)
            {
                if (!VirtualFileSystem.Started)
                {
                    Debug.LogError("VirtualFileSystem: File system is not initialized.");
                    return false;
                }

                if (VirtualFileSystem.LoggingFileOperations)
                    Debug.LogFormat("Logging File Operations: VirtualFile.Exists( \"{0}\" )", (object) path);

                path = VirtualFileSystem.NormalizePath(path);
                return File.Exists(VirtualFileSystem.GetRealPathByVirtual(path));
            }
        }

        public static Stream Open(string path)
        {
            lock (VirtualFileSystem.FileLock)
            {
                if (!VirtualFileSystem.Started)
                {
                    Debug.LogError("VirtualFileSystem: File system is not initialized.");
                    return null;
                }

                if (VirtualFileSystem.LoggingFileOperations)
                    Debug.LogFormat("Logging File Operations: VirtualFile.Open( \"{0}\" )", path as object);

                path = VirtualFileSystem.NormalizePath(path);

                Stream localBytes = null;
                string localPath = VirtualFileSystem.GetRealPathByVirtual(path);
                try
                {
                    localBytes = new MemoryStream(File.ReadAllBytes(localPath));
                }
                catch (FileNotFoundException)
                {
                }
                catch (Exception err)
                {
                    // ReSharper disable once PossibleIntendedRethrow
                    throw err;
                }

                return localBytes;
            }
        }
    }
}