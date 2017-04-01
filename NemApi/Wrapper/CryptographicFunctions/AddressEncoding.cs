using System;
using System.Text;
using Chaos.NaCl;
using Org.BouncyCastle.Crypto.Digests;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public static class AddressEncoding
    {
        /*
        * Converts a provided public key to an encoded address
        *
        * @publicKey The key to convert to an encoded address
        *
        * Returns: EncodedAddress
        */

        public static string ToEncoded(this byte network, PublicKey publicKey)
        {
            if (!publicKey.Raw.OnlyHexInString() || publicKey.Raw.Length != 64 && publicKey.Raw.Length != 66)
                throw new ArgumentException("invalid public key");

            // step 1) sha-3(256) public key
            var digestSha3 = new KeccakDigest(256);
            var stepOne = new byte[32];

            digestSha3.BlockUpdate(CryptoBytes.FromHexString(publicKey.Raw), 0, 32);
            digestSha3.DoFinal(stepOne, 0);

            // step 2) perform ripemd160 on previous step
            var digestRipeMd160 = new RipeMD160Digest();
            var stepTwo = new byte[20];
            digestRipeMd160.BlockUpdate(stepOne, 0, 32);
            digestRipeMd160.DoFinal(stepTwo, 0);

            // step3) prepend network byte    
            var stepThree =
                CryptoBytes.FromHexString(string.Concat(network == 0x68 ? 68 : 98, CryptoBytes.ToHexStringLower(stepTwo)));

            // step 4) perform sha3 on previous step
            var stepFour = new byte[32];
            digestSha3.BlockUpdate(stepThree, 0, 21);
            digestSha3.DoFinal(stepFour, 0);

            // step 5) retrieve checksum
            var stepFive = new byte[4];
            Array.Copy(stepFour, 0, stepFive, 0, 4);

            // step 6) append stepFive to resulst of stepThree
            var stepSix = new byte[25];
            Array.Copy(stepThree, 0, stepSix, 0, 21);
            Array.Copy(stepFive, 0, stepSix, 21, 4);

            // step 7) return base 32 encode address byte array

            return new Base32Encoder().Encode(stepSix).ToUpper();
        }
    }
}