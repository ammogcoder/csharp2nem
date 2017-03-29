using System;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    internal static class ByteUtils
    {
        internal static byte[] TruncateByteArray(byte[] bytes, int len)
        {
            var truncBytes = new byte[len];

            Array.Copy(bytes, 0, truncBytes, 0, len);

            return truncBytes;
        }

        internal static byte[] ConcatonatatBytes(byte[] a, byte[] b)
        {
            var combined = new byte[a.Length + b.Length];

            Array.Copy(a, combined, a.Length);
            Array.Copy(b, 0, combined, a.Length, b.Length);

            return combined;
        }
    }
}