using CSharp2nem.CryptographicFunctions;

namespace CSharp2nem.Model.AccountSetup
{
    /// <summary>
    /// PublicKey class stores public keys. Contains a constructor overload that takes a PrivateKey and produces a public key from it.
    /// </summary>
    public class PublicKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicKey"/> class.
        /// </summary>
        /// <param name="key">The public key.</param>
        public PublicKey(string key)
        {
            Raw = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicKey"/> class.
        /// </summary>
        /// <param name="key">The private key to derive the public key from.</param>
        public PublicKey(PrivateKey key)
        {
            Raw = PublicKeyConversion.ToPublicKey(key);
        }

        /// <summary>
        /// Gets the raw public key string.
        /// </summary>
        /// <value>
        /// The public key string
        /// </value>
        public string Raw { get; private set; }
    }
}