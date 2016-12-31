using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Engine.FileSystem
{
    /// <summary>Defines a file for virtual file system.</summary>
    public static class VirtualFile
    {
        /// <summary>Determines whether the specified file exists.</summary>
        /// <param name="path">The file to check.</param>
        /// <returns><b>true</b> if the file is exists; otherwise, <b>false</b>.</returns>
        public static bool Exists(string path)
        {
            lock (VirtualFileSystem.W)
            {
                if (!VirtualFileSystem.s)
                {
                    Debug.LogError("VirtualFileSystem: File system is not initialized.");
                    return false;
                }

                if (VirtualFileSystem.LoggingFileOperations)
                    Debug.Log("Logging File Operations: VirtualFile.Exists( \"{0}\" )", (object)path);

                path = VirtualFileSystem.NormalizePath(path);
                path = VirtualFileSystem.A(path, true);
                return File.Exists(VirtualFileSystem.GetRealPathByVirtual(path)) || VirtualFile.IsInArchive(path);
            }
        }

        public static VirtualFileStream Open(string path)
        {
            lock (VirtualFileSystem.W)
            {
                if (!VirtualFileSystem.s)
                {
                    Debug.LogError("VirtualFileSystem: File system is not initialized.");
                    return (VirtualFileStream)null;
                }

                if (VirtualFileSystem.LoggingFileOperations)
                    Debug.Log("Logging File Operations: VirtualFile.Open( \"{0}\" )", (object)path);

                path = VirtualFileSystem.NormalizePath(path);
                path = VirtualFileSystem.A(path, true);
                bool local_2 = VirtualFileSystem.A(path);

                if (local_2)
                {
                    byte[] local_5 = VirtualFileSystem.a(path);
                    if (local_5 != null)
                        return (VirtualFileStream)new MemoryVirtualFileStream(local_5);
                }

                if (VirtualFileSystem.X.Count != 0)
                {
                    string local_6 = path.ToLower();
                    VirtualFileSystem.PreloadFileToMemoryItem local_7;
                    if (VirtualFileSystem.X.TryGetValue(local_6, out local_7) && local_7.an)
                        return (VirtualFileStream)new MemoryVirtualFileStream(local_7.ao);
                }

                VirtualFileStream local_3 = (VirtualFileStream)null;
                string local_8 = VirtualFileSystem.GetRealPathByVirtual(path);
                try
                {
                    local_3 = c.Platform != c.A.Windows ? (c.Platform != c.A.MacOSX ? (VirtualFileStream)new G(local_8) : (VirtualFileStream)new f(local_8)) : (VirtualFileStream)new g(local_8);
                }
                catch (FileNotFoundException exception_0)
                {
                }
                catch (Exception exception_1)
                {
                    throw exception_1;
                }
                if (local_3 == null)
                {
                    local_3 = ArchiveManager.Instance.FileOpen(path);
                    if (local_3 == null)
                        throw new FileNotFoundException("File not found.", path);
                }

                if (local_2)
                {
                    byte[] local_9 = new byte[local_3.Length];
                    if ((long)local_3.Read(local_9, 0, (int)local_3.Length) == local_3.Length)
                        VirtualFileSystem.A(path, local_9);
                    local_3.Position = 0L;
                }
                return local_3;
            }
        }

        public static long GetLength(string path)
        {
            lock (VirtualFileSystem.W)
            {
                if (!VirtualFileSystem.s)
                {
                    Log.Fatal("VirtualFileSystem: File system is not initialized.");
                    return 0;
                }
                if (VirtualFileSystem.LoggingFileOperations)
                    Log.Info("Logging File Operations: VirtualFile.GetLength( \"{0}\" )", (object)path);
                path = VirtualFileSystem.NormalizePath(path);
                path = VirtualFileSystem.A(path, true);
                string local_2 = VirtualFileSystem.GetRealPathByVirtual(path);
                try
                {
                    return new System.IO.FileInfo(local_2).Length;
                }
                catch (FileNotFoundException exception_0)
                {
                }
                ArchiveManager.FileInfo local_3;
                if (ArchiveManager.Instance.GetFileInfo(path, out local_3))
                    return local_3.Length;
                throw new FileNotFoundException("File not found.", path);
            }
        }

        public static bool IsInArchive(string path)
        {
            lock (VirtualFileSystem.W)
            {
                if (!VirtualFileSystem.s)
                {
                    Log.Fatal("VirtualFileSystem: File system is not initialized.");
                    return false;
                }
                if (VirtualFileSystem.LoggingFileOperations)
                    Log.Info("Logging File Operations: VirtualFile.IsInArchive( \"{0}\" )", (object)path);
                path = VirtualFileSystem.NormalizePath(path);
                path = VirtualFileSystem.A(path, true);
                if (File.Exists(VirtualFileSystem.GetRealPathByVirtual(path)))
                    return false;
                ArchiveManager.FileInfo local_2;
                return ArchiveManager.Instance.GetFileInfo(path, out local_2);
            }
        }

        public static bool IsArchive(string path)
        {
            lock (VirtualFileSystem.W)
            {
                if (VirtualFileSystem.LoggingFileOperations)
                    Log.Info("Logging File Operations: VirtualFile.IsArchive( \"{0}\" )", (object)path);
                path = VirtualFileSystem.NormalizePath(path);
                path = VirtualFileSystem.A(path, true);
                return ArchiveManager.Instance.GetArchive(VirtualFileSystem.GetRealPathByVirtual(path)) != null;
            }
        }

        public static byte[] ReadAllBytes(string path)
        {
            using (VirtualFileStream virtualFileStream = VirtualFile.Open(path))
            {
                byte[] buffer = new byte[virtualFileStream.Length];
                if (virtualFileStream.Read(buffer, 0, buffer.Length) != buffer.Length)
                    throw new EndOfStreamException();
                return buffer;
            }
        }
    }
}