using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Engine.FileSystem
{
    /// <summary>
    ///     Auxiliary class for work with <see cref="T:Engine.FileSystem.TextBlock" />.
    /// </summary>
    public static class TextBlockUtils
    {
        /// <summary>Loads the block from a file of virtual file system.</summary>
        /// <param name="path">The virtual file path.</param>
        /// <param name="errorString">The information on an error.</param>
        /// <param name="fileNotFound"><b>true</b> if file not found.</param>
        /// <returns><see cref="T:Engine.FileSystem.TextBlock" /> if the block has been loaded; otherwise, <b>null</b>.</returns>
        public static TextBlock LoadFromVirtualFile(string path, out string errorString, out bool fileNotFound)
        {
            errorString = null;
            fileNotFound = false;
            try
            {
                using (var stream = (Stream) VirtualFile.Open(path))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        string errorString1;
                        var textBlock = TextBlock.Parse(streamReader.ReadToEnd(), out errorString1);
                        if (textBlock == null)
                            errorString = string.Format("Parsing text block failed \"{0}\" ({1}).", path, errorString1);

                        return textBlock;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                errorString = string.Format("Reading file failed \"{0}\".", path);
                fileNotFound = true;
                return null;
            }
            catch (Exception ex)
            {
                errorString = string.Format("Reading file failed \"{0}\".", path);
                return null;
            }
        }

        /// <summary>Loads the block from a file of virtual file system.</summary>
        /// <param name="path">The virtual file path.</param>
        /// <param name="errorString">The information on an error.</param>
        /// <returns><see cref="T:Engine.FileSystem.TextBlock" /> if the block has been loaded; otherwise, <b>null</b>.</returns>
        public static TextBlock LoadFromVirtualFile(string path, out string errorString)
        {
            bool fileNotFound;
            return LoadFromVirtualFile(path, out errorString, out fileNotFound);
        }

        /// <summary>Loads the block from a file of virtual file system.</summary>
        /// <param name="path">The virtual file path.</param>
        /// <returns><see cref="T:Engine.FileSystem.TextBlock" /> if the block has been loaded; otherwise, <b>null</b>.</returns>
        public static TextBlock LoadFromVirtualFile(string path)
        {
            string errorString;
            var textBlock = LoadFromVirtualFile(path, out errorString);
            if (textBlock != null)
                return textBlock;
            Debug.LogError(errorString);
            return textBlock;
        }

        /// <summary>Loads the block from a file of real file system.</summary>
        /// <param name="path">The real file path.</param>
        /// <param name="errorString">The information on an error.</param>
        /// <param name="fileNotFound"><b>true</b> if file not found.</param>
        /// <returns><see cref="T:Engine.FileSystem.TextBlock" /> if the block has been loaded; otherwise, <b>null</b>.</returns>
        public static TextBlock LoadFromRealFile(string path, out string errorString, out bool fileNotFound)
        {
            errorString = null;
            fileNotFound = false;
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        string errorString1;
                        var textBlock = TextBlock.Parse(streamReader.ReadToEnd(), out errorString1);
                        if (textBlock == null)
                            errorString = string.Format("Parsing text block failed \"{0}\" ({1}).", path, errorString1);
                        return textBlock;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                errorString = string.Format("Reading file failed \"{0}\".", path);
                fileNotFound = true;
                return null;
            }
            catch (Exception ex)
            {
                errorString = string.Format("Reading file failed \"{0}\".", path);
                return null;
            }
        }

        /// <summary>Loads the block from a file of real file system.</summary>
        /// <param name="path">The real file path.</param>
        /// <param name="errorString">The information on an error.</param>
        /// <returns><see cref="T:Engine.FileSystem.TextBlock" /> if the block has been loaded; otherwise, <b>null</b>.</returns>
        public static TextBlock LoadFromRealFile(string path, out string errorString)
        {
            bool fileNotFound;
            return LoadFromRealFile(path, out errorString, out fileNotFound);
        }

        /// <summary>Loads the block from a file of real file system.</summary>
        /// <param name="path">The real file path.</param>
        /// <returns><see cref="T:Engine.FileSystem.TextBlock" /> if the block has been loaded; otherwise, <b>null</b>.</returns>
        public static TextBlock LoadFromRealFile(string path)
        {
            string errorString;
            var textBlock = LoadFromRealFile(path, out errorString);
            if (textBlock != null)
                return textBlock;
            Debug.LogError(errorString);
            return textBlock;
        }
    }
}