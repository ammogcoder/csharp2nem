using System;
using System.Text;
using Chaos.NaCl;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Utils;
using Org.BouncyCastle.Crypto.Digests;

namespace CSharp2nem.CryptographicFunctions
{
    /// <summary>
    /// Contains a function to convert public keys to base32 encoded addresses
    /// </summary>
    public static class AddressEncoding
    {
        /// <summary>
        /// Converts a public key to a main net or test net address. Network byte determines which network version to convert to.
        /// </summary>
        /// <param name="network">The network byte.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>The unhyphenated address string.</returns>
        /// <exception cref="ArgumentException">invalid public key. Thrown when a public key is not a 64 char hex string.</exception>
        /// <example> 
        /// This sample shows how to use the <see cref="ToEncoded"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        /// 
        ///         string address = AddressEncoding.ToEncoded(connection.GetNetworkVersion(), new PublicKey("0705c2634de7e58325dabc58c4a794559be4d55d102d3aafcb189acb2e596add"));
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string ToEncoded(byte network, PublicKey publicKey) 
        {
            if (!StringUtils.OnlyHexInString(publicKey.Raw) || publicKey.Raw.Length != 64)
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