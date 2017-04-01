using System;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
    * Converts a private key to public key.
    *
    * Note: Nem uses sha3 for public key encryption and address encoding. 
    *       Sha3 was not available in the BouncyCastle Cryptography library
    *       so the Chaos.NaCl implementation of Sha3 was imported into the 
    *       BouncyCastle library for use in this wrapper.
    *
    * Note 2: Bouncy castles Ed25519 does not by default support 66 char (aka. negative)
    *         private keys so this also had to be updated within the bouncycastle
    *         library, specifically in the Ed25519 class. 
    */

    public static class PublicKeyConversion
    {
        /*
        * Converts a provided private key to a public key
        *
        * @privatrKey The key to convert to a public key
        *
        * Returns: The public key produced from the private key
        */

        public static byte[] ToPublicKey(this PrivateKey privateKey)
        {
            if (!privateKey.Raw.OnlyHexInString() ||
                privateKey.Raw.Length == 64 && privateKey.Raw.Length == 66)
                throw new ArgumentException("invalid private key");

            var privateKeyArray = CryptoBytes.FromHexString(privateKey.Raw.ConvertToUnsecureString());

            Array.Reverse(privateKeyArray);

            return Ed25519.PublicKeyFromSeed(privateKeyArray);
        }
    }
}