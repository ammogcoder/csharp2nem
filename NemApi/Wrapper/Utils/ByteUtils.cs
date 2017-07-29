using System;

namespace CSharp2nem.Utils
{
    public static class ByteUtils
    {
        public static byte[] TruncateByteArray(byte[] bytes, int len)
        {
            var truncBytes = new byte[len];

            Array.Copy(bytes, 0, truncBytes, 0, len);

            return truncBytes;
        }

        public static byte[] ConcatonatetBytes(byte[] a, byte[] b)
        {
            var combined = new byte[a.Length + b.Length];

            Array.Copy(a, combined, a.Length);
            Array.Copy(b, 0, combined, a.Length, b.Length);

            return combined;
        }
    }
}