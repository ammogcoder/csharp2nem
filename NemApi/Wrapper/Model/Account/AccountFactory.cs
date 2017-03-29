using System;
using System.Security.Cryptography;
using Chaos.NaCl;

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

        public VerifiableAccount FromNewPrivateKey()
        {
            string sk;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                sk = CryptoBytes.ToHexStringLower(tokenData);
            }

            return FromPrivateKey(sk);
        }


        public VerifiableAccount FromPrivateKey(string key)
        {
            if (!StringUtils.OnlyHexInString(key) || key.Length != 64 && key.Length != 66)
                throw new ArgumentException("invalid private key");
            return new VerifiableAccount(Connection, new PrivateKey(key));
        }

        public UnverifiableAccount FromPublicKey(string publicKey)
        {
            if (!StringUtils.OnlyHexInString(publicKey) || publicKey.Length != 64 && publicKey.Length != 66)
                throw new ArgumentException("invalid public key");
            return new UnverifiableAccount(Connection, new PublicKey(publicKey));
        }

        public VerifiableAccount FromPrivateKey(PrivateKey key)
        {
            if (!StringUtils.OnlyHexInString(key.Raw) || key.Raw.Length != 64 && key.Raw.Length != 66)
                throw new ArgumentException("invalid private key");
            return new VerifiableAccount(Connection, key);
        }

        public UnverifiableAccount FromPublicKey(PublicKey publicKey)
        {
            if (!StringUtils.OnlyHexInString(publicKey.Raw) || publicKey.Raw.Length != 64 && publicKey.Raw.Length != 66)
                throw new ArgumentException("invalid public key");
            return new UnverifiableAccount(Connection, publicKey);
        }

        public UnverifiableAccount FromEncodedAddress(string encodedAddress)
        {
            return new UnverifiableAccount(Connection, new Address(StringUtils.GetResultsWithoutHyphen(encodedAddress)));
        }

        public UnverifiableAccount FromEncodedAddress(Address encodedAddress)
        {
            return new UnverifiableAccount(Connection, encodedAddress);
        }
    }
}