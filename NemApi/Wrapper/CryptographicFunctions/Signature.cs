using System;
using Chaos.NaCl;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Utils;

namespace CSharp2nem.CryptographicFunctions
{
    /// <summary>
    /// Produces a signature for a given byte array using a given private key.
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Signature"/> class.
        /// </summary>
        /// <remarks>
        /// Uses twisted edwards elliptic curve, Ed25519, to produce signatures. 
        /// </remarks>
        /// <param name="data">The data bytes to sign.</param>
        /// <param name="privateKey">The private key used to sign the bytes.</param>
        /// <example> 
        /// This sample shows how to create a new instance of the <see cref="Signature"/> class and produce a signature.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {                
        ///         byte[] bytes = new byte[10];
        ///         
        ///         Signature sig = new Signature(bytes, new PrivateKey("0705c2634de7e58325dabc58c4a794559be4d55d102d3aafcb189acb2e596add"));
        /// 
        ///         string signature = sig._Signature;
        ///     }
        /// }
        /// </code>
        /// </example>
        public Signature(byte[] data, PrivateKey privateKey)
        {
            var sig = new byte[64];

            try
            {
                var sk = new byte[64];
                Array.Copy(CryptoBytes.FromHexString(StringUtils.ConvertToUnsecureString(privateKey.Raw)), sk, 32);
                Array.Copy(
                   CryptoBytes.FromHexString(
                        new PublicKey(PublicKeyConversion.ToPublicKey(privateKey)).Raw), 0,
                    sk, 32, 32);
                Ed25519.crypto_sign2(sig, data, sk, 32);
                CryptoBytes.Wipe(sk);
            }
            finally
            {
                _Signature = sig;
            }
        }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <value>
        /// The signature.
        /// </value>
        public byte[] _Signature { get; private set; }
    }
}