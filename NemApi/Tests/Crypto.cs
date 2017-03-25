using Microsoft.VisualStudio.TestTools.UnitTesting;
using NemApi;
using System.Collections.Generic;
using System.Linq;


namespace Tests
{
    [TestClass]
    public class Crypto
    {
        [TestMethod]
        public void CanConvertPrivateKeyToPublicKey()
        {
            var privateKey = "abf4cf55a2b3f742d7543d9cc17f50447b969e6e06f5ea9195d428ab12b7318d";

            var expected   = "8a558c728c21c126181e5e654b404a45b4f0137ce88177435a69978cc6bec1f4";

            var result = new PublicKey(new PrivateKey(privateKey)).Raw;
           
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CanConvertPublicKeyToMainNetAddress()
        {
            var privateKey = "abf4cf55a2b3f742d7543d9cc17f50447b969e6e06f5ea9195d428ab12b7318d";

            var result = AddressEncoding.ToEncoded(0x68, new PublicKey(new PrivateKey(privateKey)));

            var expected = "NAZQOWGZJ5PKR3QJEUZEMS6MXQX3WZZZAKKTLPZT";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CanConvertPublicKeyToTestNetAddress()
        {


            var privateKey = "abf4cf55a2b3f742d7543d9cc17f50447b969e6e06f5ea9195d428ab12b7318d";

            var result = AddressEncoding.ToEncoded(0x98, new PublicKey(new PrivateKey(privateKey)));

            var expected = "TAZQOWGZJ5PKR3QJEUZEMS6MXQX3WZZZAJJDHP3H";

            Assert.AreEqual(expected, result);
        }

       //[TestMethod]
       //public void CanVerifySignature()
       //{
       //    var sig = new byte[64];
       //    var sk = new byte[64];
       //
       //    var privateKey = "abf4cf55a2b3f742d7543d9cc17f50447b969e6e06f5ea9195d428ab12b7318d";
       //    var publicKey = "8a558c728c21c126181e5e654b404a45b4f0137ce88177435a69978cc6bec1f4";
       //    var expected = "d9cec0cc0e3465fab229f8e1d6db68ab9cc99a18cb0435f70deb6100948576cd5c0aa1feb550bdd8693ef81eb10a556a622db1f9301986827b96716a7134230c";
       //    var data = CryptoBytes.FromHexString("8ce03cd60514233b86789729102ea09e867fc6d964dea8c2018ef7d0a2e0e24bf7e348e917116690b9");
       //    Array.Copy(CryptoBytes.FromHexString(privateKey), sk, 32);
       //    Array.Copy(GetKeyBytes(publicKey), 0, sk, 32, 32);
       //
       //    Ed25519.crypto_sign2(sig, data, sk, 32);
       //    Assert.IsTrue( Ed25519.Verify(sig, data, GetKeyBytes(publicKey)));
       //    Assert.AreEqual(expected, CryptoBytes.ToHexStringLower(sig));
       //}

        internal byte[] GetKeyBytes(string raw)
        {
            var set = Split(raw, 2);
            var bytes = new byte[32];
            var y = 0;
            foreach (var s in set)
            {
                var x = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
                bytes[y] = (byte)x;
                y++;
            }

            return bytes;
        }

        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
