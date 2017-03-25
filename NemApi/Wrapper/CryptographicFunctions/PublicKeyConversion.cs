using System;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace NemApi
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

    public class PublicKeyConversion
    {
        /*
        * Converts a provided private key to a public key
        *
        * @Param: privatrKey, The key to convert to a public key
        *
        * @Returns: PublicKey
        */

        public static byte[] ToPublicKey(PrivateKey privateKey)
        {
            if (!StringUtils.OnlyHexInString(privateKey.Raw) ||
                privateKey.Raw.Length == 64 && privateKey.Raw.Length == 66)
                throw new ArgumentException("invalid private key");

            var privateKeyArray = CryptoBytes.FromHexString(StringUtils.ConvertToUnsecureString(privateKey.Raw));

            Array.Reverse(privateKeyArray);

            return Ed25519.PublicKeyFromSeed(privateKeyArray);
        }
    }
}