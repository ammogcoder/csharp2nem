using System;
using Chaos.NaCl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class Ed25519BlockCipher
    {
        public Ed25519BlockCipher(VerifiableAccount verifiableAccount, UnverifiableAccount unverifiableAccount)
        {
            UnverifiableAccount = unverifiableAccount;
            VerifiableAccount = verifiableAccount;
            Random = new SecureRandom();
        }

        private VerifiableAccount VerifiableAccount { get; }
        private UnverifiableAccount UnverifiableAccount { get; }
        private SecureRandom Random { get; }


        public byte[] Encrypt(byte[] input)
        {
            // Setup salt.
            var salt = new byte[ByteLength.PublicKeyLength];
            Random.NextBytes(salt);

            // Derive shared key.
            var sharedKey = GetSharedKey(VerifiableAccount.PrivateKey, UnverifiableAccount.PublicKey, salt);

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
            var sharedKey = GetSharedKey(VerifiableAccount.PrivateKey, UnverifiableAccount.PublicKey, salt);

            // Setup block cipher.
            var cipher = SetupBlockCipher(sharedKey, ivData, false);

            // Decode.
            return Transform(cipher, encData);
        }

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

        private static byte[] GetSharedKey(PrivateKey privateKey, PublicKey publicKey, byte[] salt)
        {
            var shared = new byte[32];

            var hash = Ed25519.key_derive( // TODO: find out why hash isnt used.
                shared,
                salt,
                CryptoBytes.FromHexString(publicKey.Raw),
                CryptoBytes.FromHexString(privateKey.Raw.ConvertToUnsecureString()));


            return shared;
        }
    }
}