using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    internal class Signature
    {
        internal Signature(byte[] data, PrivateKey privateKey)
        {
            var sig = new byte[64];

            try
            {
                var sk = new byte[64];
                Array.Copy(CryptoBytes.FromHexString(StringUtils.ConvertToUnsecureString(privateKey.Raw)), sk, 32);
                Array.Copy(
                    GetKeyBytes(
                        new PublicKey(CryptoBytes.ToHexStringLower(PublicKeyConversion.ToPublicKey(privateKey))).Raw), 0,
                    sk, 32, 32);
                Ed25519.crypto_sign2(sig, data, sk, 32);
                CryptoBytes.Wipe(sk);
            }
            finally
            {
                LongSignature = sig;
            }
        }

        internal byte[] LongSignature { get; private set; }

        internal byte[] GetKeyBytes(string raw)
        {
            var set = Split(raw, 2);

            var bytes = new byte[32];
            var i = 0;
            foreach (var s in set)
            {
                var x = int.Parse(s, NumberStyles.HexNumber);
                bytes[i] = (byte) x;
                i++;
            }

            return bytes;
        }

        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}