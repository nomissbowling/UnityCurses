using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Assets.Scripts.Engine.FileSystem
{
    public sealed class MemoryVirtualFileStream : VirtualFileStream
    {
        private byte[] al;
        private bool am;
        private int aM;

        public MemoryVirtualFileStream(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            al = buffer;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return al.Length; }
        }

        public override long Position
        {
            get
            {
                if (am)
                    throw new ObjectDisposedException(null);
                return aM;
            }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public override void Close()
        {
            am = true;
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            am = true;
            base.Dispose(disposing);
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (am)
                throw new ObjectDisposedException(null);
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset < 0L)
                        throw new IOException("Seek before begin.");
                    aM = (int) offset;
                    break;
                case SeekOrigin.Current:
                    if (aM + offset < 0L)
                        throw new IOException("Seek before begin.");
                    aM = aM + (int) offset;
                    break;
                case SeekOrigin.End:
                    if (al.Length + offset < 0L)
                        throw new IOException("Seek before begin.");
                    aM = al.Length + (int) offset;
                    break;
                default:
                    throw new ArgumentException("Invalid seek origin.");
            }
            return aM;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("The method is not supported.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("The method is not supported.");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (am)
                throw new ObjectDisposedException(null);
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid offset length.");
            var count1 = al.Length - aM;
            if (count1 > count)
                count1 = count;
            if (count1 <= 0)
                return 0;
            if (count1 <= 8)
            {
                var num = count1;
                while (--num >= 0)
                    buffer[offset + num] = al[aM + num];
            }
            else
                Buffer.BlockCopy(al, aM, buffer, offset, count1);
            aM = aM + count1;
            return count1;
        }

        public override int ReadUnmanaged(IntPtr buffer, int count)
        {
            if (am)
                throw new ObjectDisposedException(null);
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            var length = al.Length - aM;
            if (length > count)
                length = count;
            if (length <= 0)
                return 0;
            Marshal.Copy(al, aM, buffer, length);
            aM = aM + length;
            return length;
        }

        public override int ReadByte()
        {
            if (am)
                throw new ObjectDisposedException(null);
            if (this.aM >= this.al.Length)
                return -1;
            var al = this.al;
            var aM = this.aM;
            this.aM = aM + 1;
            var index = aM;
            return al[index];
        }
    }
}