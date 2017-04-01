using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Produces a signature for a given byte array
     * 
     * @privateKey { PrivateKey } The private key used to sign the bytes
     * @bytes { bytes[] } The bytes to sign
     */
    internal class Signature
    {
        internal Signature(byte[] data, PrivateKey privateKey)
        {
            var sig = new byte[64];

            try
            {
                var sk = new byte[64];
                Array.Copy(CryptoBytes.FromHexString(privateKey.Raw.ConvertToUnsecureString()), sk, 32);
                Array.Copy(
                    GetKeyBytes(
                        new PublicKey(CryptoBytes.ToHexStringLower(privateKey.ToPublicKey())).Raw), 0,
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

        /*
         * Get the private key bytes
         * 
         * @raw { string } The raw private kye in string form
         * 
         * Return: The bytes of the private key
         */
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

        /*
         * Split a string into a set of substrings 
         * based on a given length of substring
         * 
         * @str { string } The string to split
         * @chunkSize { int } The size of chunks to be returned
         * 
         * Return: IEnumerable list of sub strings
         */
        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}