using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class PublicKey
    {
        public PublicKey(string key)
        {
            Raw = key;
        }

        public PublicKey(PrivateKey key)
        {
            Raw = CryptoBytes.ToHexStringLower(PublicKeyConversion.ToPublicKey(key));
        }

        public string Raw { get; private set; }
    }
}