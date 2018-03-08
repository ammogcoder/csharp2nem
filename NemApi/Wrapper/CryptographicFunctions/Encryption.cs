using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Chaos.NaCl;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Utils;
using Org.BouncyCastle.Security;

namespace CSharp2nem.CryptographicFunctions
{

    public class Encryption
    {
        public static string Encode(string text, PrivateKey sk, string pk)
        {
            SecureRandom random = new SecureRandom();

            var salt = new byte[32];
            random.NextBytes(salt);

            var ivData = new byte[16];
            random.NextBytes(ivData);

            return _Encode(
                CryptoBytes.FromHexString(StringUtils.ConvertToUnsecureString(sk.Raw)),
                CryptoBytes.FromHexString(pk),
                text, 
                ivData,
                salt);
        }

        public static string Decode(string text, PrivateKey sk, string pk)
        {
                return _Decode(
                    CryptoBytes.FromHexString(StringUtils.ConvertToUnsecureString(sk.Raw)),
                    CryptoBytes.FromHexString(pk),
                    CryptoBytes.FromHexString(text));
        }

        internal static string _Encode(byte[] privateKey, byte[] pubKey, string msg, byte[] iv, byte[] salt)
        {
            var shared = new byte[32];

            Ed25519.key_derive(
                shared,
                salt,
                privateKey,
                pubKey);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = shared;

                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(msg);
                        }

                        return CryptoBytes.ToHexStringLower(salt) + CryptoBytes.ToHexStringLower(iv) + CryptoBytes.ToHexStringLower( msEncrypt.ToArray());
                    }
                }
            }
        }
        public static byte[] Take(byte[] bytes, int start, int length)
        {
            var tempBytes = new byte[length];

            Array.Copy(bytes, start, tempBytes, 0, length);

            return tempBytes;
        }
        internal static string _Decode(byte[] privKey, byte[] pubKey, byte[] data)
        {
            var salt = Take(data, 0, 32).ToArray();
            var iv = Take(data,32, 16);
            var payload = Take(data, 48 , data.Length - 48);
            var shared = new byte[32];

            Ed25519.key_derive(
                shared,
                salt,
                privKey,
                pubKey);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = shared;
                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
 
                using (var msDecrypt = new MemoryStream(payload))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {                          
                            var a = srDecrypt.ReadToEnd();
                           
                            return a;
                        }
                    }
                }
            }
        }
    }
}