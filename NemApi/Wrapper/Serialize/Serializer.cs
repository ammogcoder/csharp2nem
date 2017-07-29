using System.IO;
using System.Text;

namespace CSharp2nem.Serialize
{
    /*
    * Serializer contains a number of methods which convert verious data to bytes,
    * then writing then to a MemoryStream.
    */

    internal class Serializer
    {
        internal MemoryStream Stream = new MemoryStream();

        internal Serializer()
        {
            Writer = new BinaryWriter(Stream);
        }

        private BinaryWriter Writer { get; }

        internal void WriteString(string src)
        {
            var bytes = Encoding.UTF8.GetBytes(src);

            Writer.Write(bytes);
        }

        internal void WriteCustomBytes(int src, int src2)
        {
            var bytes = new byte[4];
            bytes[0] = (byte) src;
            bytes[3] = (byte) src2;

            Writer.Write(bytes);
        }

        internal void WriteBytes(byte[] src)
        {
            Writer.Write(src);
        }

        internal void WriteInt(int src)
        {
            var bytes = new[]
            {
                (byte) src,
                (byte) (src >> 8),
                (byte) (src >> 16),
                (byte) (src >> 24)
            };
            Writer.Write(bytes);
        }

        internal void WriteLong(long src)
        {
            WriteInt((int) src);
            WriteInt((int) (src >> 32));
        }

        internal byte[] GetBytes()
        {
            return Stream.GetBuffer();
        }

        internal void UpdateNthFourBytes(long index, int newValue)
        {
            var currPos = Stream.Position;
            try
            {
                var offset = index;
                Stream.Position = offset;
                Writer.Write(newValue);
            }
            finally
            {
                Stream.Position = currPos;
            }
        }

        internal void UpdateNthEightBytes(long index, long newValue)
        {
            var currPos = Stream.Position;
            try
            {
                var offset = index;
                Stream.Position = offset;
                Writer.Write(newValue);
            }
            finally
            {
                Stream.Position = currPos;
            }
        }

        internal void UpdateNthByte(long index, byte newValue)
        {
            var currPos = Stream.Position;
            try
            {
                var offset = index;
                Stream.Position = offset;
                Writer.Write(newValue);
            }
            finally
            {
                Stream.Position = currPos;
            }
        }
    }
}