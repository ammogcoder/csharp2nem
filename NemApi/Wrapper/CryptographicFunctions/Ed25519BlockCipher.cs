using System;
using Chaos.NaCl;
using CSharp2nem.Constants;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.RequestClients;
using CSharp2nem.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace CSharp2nem.CryptographicFunctions
{
   
    /// <summary>
    /// Contains a number of methods used to encrypt or decrypt data
    /// </summary>
    /// <remarks>
    /// Currently broken.
    /// </remarks>
    public class Ed25519BlockCipher
    {
        /// <summary>
        /// Constructs an instance of the <see cref="Ed25519BlockCipher"/> class.
        /// </summary>
        /// <param name="PrivateKeyAccountClient">The PrivateKeyAccountClient used to encrypt or decrypt data</param>
        /// <param name="publicKey">The public key of the recipient of the encrypted data.</param>
        public Ed25519BlockCipher(PrivateKeyAccountClient PrivateKeyAccountClient, string publicKey)
        {
            PublicKey = publicKey;
            PrivateKeyAccount = PrivateKeyAccountClient;
            Random = new SecureRandom();
        }

        private PrivateKeyAccountClient PrivateKeyAccount { get; }
        private string PublicKey { get; }
        private SecureRandom Random { get; }


        /*
         *
         * private key, and the recipients public key
         * 
         * @input { byte[] } The data to be encrypted
         */
        /// <summary>
        ///  Encrypts data using the private key of the PrivateKeyAccountClient and the public key of the recipient.
        /// </summary>
        /// <param name="input">The data to encrypt.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] input)
        {
            // Setup salt.
            var salt = new byte[ByteLength.PublicKeyLength];
            Random.NextBytes(salt);

            // Derive shared key.
            var sharedKey = GetSharedKey(PrivateKeyAccount.PrivateKey, PublicKey, salt);

            // Setup IV.
            var ivData = new byte[16];

            Random.NextBytes(ivData);

            // Setup block cipher.
            var cipher = SetupBlockCipher(sharedKey, ivData, true);

            // Encode.
            var buf = Transform(cipher, input);
            if (null == buf)
                return null;

            var result = new byte[salt.Length + ivData.Length + buf.Length];
            Array.Copy(salt, 0, result, 0, salt.Length);
            Array.Copy(ivData, 0, result, salt.Length, ivData.Length);
            Array.Copy(buf, 0, result, salt.Length + ivData.Length, buf.Length);
            return result;
        }

        /// <summary>
        /// Decrypts data with recipiet private key.
        /// </summary>
        /// <param name="input">The data to decrypt</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] input)
        {
            if (input.Length < 64)
                return null;
            var salt = new byte[32];
            var ivData = new byte[16];
            var encData = new byte[input.Length - 48];

            Array.ConstrainedCopy(input, 0, salt, 0, 32);
            Array.ConstrainedCopy(input, 32, ivData, 0, 16);
            Array.ConstrainedCopy(input, 48, encData, 0, input.Length - 48);

            // Derive shared key.
            var sharedKey = GetSharedKey(PrivateKeyAccount.PrivateKey, PublicKey, salt);

            // Setup block cipher.
            var cipher = SetupBlockCipher(sharedKey, ivData, false);

            // Decode.
            return Transform(cipher, encData);
        }

        /*
         * No idea... doesnt work.. cant seem to fix it..
         * 
         * 
         */
        private static byte[] Transform(BufferedBlockCipher cipher, byte[] data)
        {
            var buf = new byte[cipher.GetOutputSize(data.Length)];
            var length = cipher.ProcessBytes(data, 0, data.Length, buf, 0);
            try
            {
                length += cipher.DoFinal(buf, length);
            }
            catch (InvalidCipherTextException)
            {
                return null;
            }
            var final = new byte[length];

            Array.Copy(buf, final, length);
            return final;
        }

        /*
         * No idea... doesnt work.. cant seem to fix it..
         * Lets not pretend i know what this does...
         * 
         */
        private static BufferedBlockCipher SetupBlockCipher(byte[] sharedKey, byte[] ivData, bool forEncryption)
        {
            // Setup cipher parameters with key and IV.
            var keyParam = new KeyParameter(sharedKey);
            var param = new ParametersWithIV(keyParam, ivData);

            // Setup AES cipher in CBC mode with PKCS7 padding.
            var padding = new Pkcs7Padding();

            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), padding);
            cipher.Reset();
            cipher.Init(forEncryption, param);
            return cipher;
        }

        /*
         * Still no idea... doesnt work either.. cant seem to fix it..
         * For some reason the produced hash isnt used and 
         * breaks even more when it is.. 0.o
         * 
         */
        private static byte[] GetSharedKey(PrivateKey privateKey, string publicKey, byte[] salt)
        {
            var shared = new byte[32];

            var hash = Ed25519.key_derive( // TODO: find out why hash isnt used.
                shared,
                salt,
                CryptoBytes.FromHexString(publicKey),
                CryptoBytes.FromHexString(StringUtils.ConvertToUnsecureString(privateKey.Raw)));


            return shared;
        }
    }
}