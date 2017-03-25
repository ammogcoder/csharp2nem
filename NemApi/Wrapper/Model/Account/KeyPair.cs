using System.Security;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class KeyPair
    {
        public KeyPair(string sk)
        {
            PrivateKey = new PrivateKey(sk);
            PublicKey = new PublicKey(PrivateKey);
        }

        public KeyPair(SecureString sk)
        {
            PrivateKey = new PrivateKey(sk);
            PublicKey = new PublicKey(PrivateKey);
        }

        public KeyPair()
        {
            PrivateKey = new PrivateKey("");
            PublicKey = new PublicKey(PrivateKey);
        }

        public PrivateKey PrivateKey { get; set; }
        public PublicKey PublicKey { get; set; }
        public string Address { get; set; }
    }
}