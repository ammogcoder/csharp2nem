using System;
using Chaos.NaCl;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Utils;

namespace CSharp2nem.CryptographicFunctions
{
    /// <summary>
    /// Derives a public key from a given private key.
    /// </summary>
    /// <remarks>
    /// Additionally supports 66 char negative private keys.
    /// </remarks>
    public static class PublicKeyConversion
    {

        /// <summary>
        /// Produces a public key from a given private key.
        /// </summary>
        /// <param name="privateKey">The private key to derive a public key from.</param>
        /// <remarks>
        /// As well as 64 char private keys, 66 char negative private keys are supported also supported. This does not affect the public key produced.
        /// </remarks>
        /// <returns>The derived public key string</returns>
        /// <exception cref="ArgumentException">invalid private key. Exacption bounds: Must be only hex string. Must be equal to 64 or 66 chars in length.</exception>
        /// <example> 
        /// This sample shows how to use the <see cref="ToPublicKey"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {                
        ///         string publicKey = PublicKeyConversion.ToPublicKey(new PrivateKey("0705c2634de7e58325dabc58c4a794559be4d55d102d3aafcb189acb2e596add"));
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string ToPublicKey(PrivateKey privateKey)
        {
            if (!StringUtils.OnlyHexInString(privateKey.Raw) ||
                privateKey.Raw.Length == 64 && privateKey.Raw.Length == 66)
                throw new ArgumentException("invalid private key");

            var privateKeyArray = CryptoBytes.FromHexString(StringUtils.ConvertToUnsecureString(privateKey.Raw));

            Array.Reverse(privateKeyArray);

            return CryptoBytes.ToHexStringLower(Ed25519.PublicKeyFromSeed(privateKeyArray));
        }
    }
}