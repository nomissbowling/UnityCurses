using System;
using System.IO;

namespace Assets.Scripts.Engine.FileSystem
{
    /// <summary>Defines a file stream for virtual file system.</summary>
    public abstract class VirtualFileStream : Stream
    {
        public abstract int ReadUnmanaged(IntPtr buffer, int count);
    }
}