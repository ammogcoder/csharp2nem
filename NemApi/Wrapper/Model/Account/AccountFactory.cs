using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Chaos.NaCl;
using Org.BouncyCastle.Crypto.Digests;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class AccountFactory
    {
        public AccountFactory(Connection connection)
        {
            if (null == connection) throw new ArgumentNullException(nameof(connection));
            Connection = connection;
        }

        public AccountFactory()
        {
            Connection = new Connection();
        }

        private Connection Connection { get; }

        public VerifiableAccount FromsNewDataPrivateKey(string data)
        {
            SecureString sk;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                var digestSha3 = new KeccakDigest(256);
                var dataBytes = Encoding.Default.GetBytes(data);
                var pkBytes = new byte[32];

                digestSha3.BlockUpdate(dataBytes, 0, 32);
                digestSha3.DoFinal(pkBytes, 0);
                sk = CryptoBytes.ToHexStringLower(pkBytes).ToSecureString();
            }

            return FromPrivateKey(new PrivateKey(sk));
        }

        public VerifiableAccount FromsNewDataPrivateKey(SecureString data)
        {
            SecureString sk;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                var digestSha3 = new KeccakDigest(256);
                var dataBytes = Encoding.Default.GetBytes(data.ConvertToUnsecureString());
                var pkBytes = new byte[32];

                digestSha3.BlockUpdate(dataBytes, 0, 32);
                digestSha3.DoFinal(pkBytes, 0);
                sk = CryptoBytes.ToHexStringLower(pkBytes).ToSecureString();
            }

            return FromPrivateKey(new PrivateKey(sk));
        }

        public VerifiableAccount FromNewPrivateKey()
        {
            SecureString sk;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                sk = CryptoBytes.ToHexStringLower(tokenData).ToSecureString();
            }

            return FromPrivateKey(new PrivateKey(sk));
        }


        public VerifiableAccount FromPrivateKey(string key)
        {
            if (!key.OnlyHexInString() || key.Length != 64 && key.Length != 66)
                throw new ArgumentException("invalid private key");
            return new VerifiableAccount(Connection, new PrivateKey(key));
        }

        public UnverifiableAccount FromPublicKey(string publicKey)
        {
            if (!publicKey.OnlyHexInString() || publicKey.Length != 64 && publicKey.Length != 66)
                throw new ArgumentException("invalid public key");
            return new UnverifiableAccount(Connection, new PublicKey(publicKey));
        }

        public VerifiableAccount FromPrivateKey(PrivateKey key)
        {
            if (!key.Raw.OnlyHexInString() || key.Raw.Length != 64 && key.Raw.Length != 66)
                throw new ArgumentException("invalid private key");
            return new VerifiableAccount(Connection, key);
        }

        public UnverifiableAccount FromPublicKey(PublicKey publicKey)
        {
            if (!publicKey.Raw.OnlyHexInString() || publicKey.Raw.Length != 64 && publicKey.Raw.Length != 66)
                throw new ArgumentException("invalid public key");
            return new UnverifiableAccount(Connection, publicKey);
        }

        public UnverifiableAccount FromEncodedAddress(string encodedAddress)
        {
            return new UnverifiableAccount(Connection, new Address(encodedAddress.GetResultsWithoutHyphen()));
        }

        public UnverifiableAccount FromEncodedAddress(Address encodedAddress)
        {
            return new UnverifiableAccount(Connection, encodedAddress);
        }
    }
}