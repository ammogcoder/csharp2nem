using System.Security.Cryptography;
using Chaos.NaCl;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Utils;
using Org.BouncyCastle.Crypto.Digests;

namespace CSharp2nem.CryptographicFunctions
{
    public static class PrivateKeyCreator
    {
        public static PrivateKey Create()
        {
            var ng = RandomNumberGenerator.Create();
            var bytes = new byte[2048];
            ng.GetNonZeroBytes(bytes);

            var digestSha3 = new KeccakDigest(256);
            var stepOne = new byte[32];
            digestSha3.BlockUpdate(bytes, 0, 32);
            digestSha3.DoFinal(stepOne, 0);

            var pk = new PrivateKey(StringUtils.ToSecureString(CryptoBytes.ToHexStringLower(stepOne)));
            bytes = null;
            stepOne = null;
            return pk;
        }
    }
}
