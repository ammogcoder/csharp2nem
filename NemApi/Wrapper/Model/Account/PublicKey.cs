using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class PublicKey
    {
        /*
         * Creates a public key
         * 
         * @key { string } The key to use to create the object
         * 
         */
        public PublicKey(string key)
        {
            Raw = key;
        }

        /*
         * Creates a public key
         * 
         * @key { PrivateKey } The private key from which to derive a public key.
         * 
         */
        public PublicKey(PrivateKey key)
        {
            Raw = CryptoBytes.ToHexStringLower(key.ToPublicKey());
        }

        public string Raw { get; private set; }
    }
}