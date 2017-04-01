using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Chaos.NaCl;
using Org.BouncyCastle.Crypto.Digests;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Produces accounts of type Verifiable or Unverifiable
     * 
     * Verifiable accounts hold private keys and have access to 
     * api's for sending transactions
     * 
     * Unverifiable accounts have only access to api's that request 
     * information from the blockchain
     * 
     * Both types of accounts store a public key and encoded address
     * If an Unverifiable account is created using an encoded address
     * via the "FromEncodedAddress" api, the public key is retrieved 
     * if possible, if not available ie. not known to the network,
     * the public key is null.
     *  
     */
    public class AccountFactory
    {
        /*
         * Constructs the account factory
         * 
         * @connection { Connection } The connection to use
         * 
         */
        public AccountFactory(Connection connection)
        {
            if (null == connection) throw new ArgumentNullException(nameof(connection));
            Connection = connection;
        }

        /*
         * Constructs the account factory
         * 
         * Uses the default connection
         * 
         */
        public AccountFactory()
        {
            Connection = new Connection();
        }

        private Connection Connection { get; }

        /*
         * Creates a verifiable account from any string of data
         * eg. google id, salted password etc.
         * 
         * @data { string } The data to use to produce an account
         * 
         * Return: A VerifiableAccount
         * 
         */
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

        /*
         * Creates a Verifiable account from any string of data
         * 
         * @data { SecureString } The data to use to produce an account
         * 
         * Return: A VerifiableAccount
         * 
         */
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

        /*
         * Creates a Verifiable account from a generated private key
         * 
         * Return: A VerifiableAccount
         * 
         */
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

        /*
         * Creates a Verifiable account from a given private key
         * 
         * @key { string } The private key in string format
         * 
         * Return: A VerifiableAccount
         * 
         */
        public VerifiableAccount FromPrivateKey(string key)
        {
            if (!key.OnlyHexInString() || key.Length != 64 && key.Length != 66)
                throw new ArgumentException("invalid private key");
            return new VerifiableAccount(Connection, new PrivateKey(key));
        }

        /*
        * Creates a Verifiable account from a given private key
        * 
        * @key { PrivateKey } The private key in PrivateKey Object format
        * 
        * Return: A VerifiableAccount
        * 
        */
        public VerifiableAccount FromPrivateKey(PrivateKey key)
        {
            if (!key.Raw.OnlyHexInString() || key.Raw.Length != 64 && key.Raw.Length != 66)
                throw new ArgumentException("invalid private key");
            return new VerifiableAccount(Connection, key);
        }


        /*
         * Creates a Unverifiable account from a given public key
         * 
         * @key { string } The public key
         * 
         * Return: A VerifiableAccount
         * 
         */
        public UnverifiableAccount FromPublicKey(string publicKey)
        {
            if (!publicKey.OnlyHexInString() || publicKey.Length != 64 && publicKey.Length != 66)
                throw new ArgumentException("invalid public key");
            return new UnverifiableAccount(Connection, new PublicKey(publicKey));
        }

        /*
         * Creates an Unverifiable account from a given public key
         * 
         * @key { PublicKey } The public key in Object format
         * 
         * Return: A UnverifiableAccount
         * 
         */
        public UnverifiableAccount FromPublicKey(PublicKey publicKey)
        {
            if (!publicKey.Raw.OnlyHexInString() || publicKey.Raw.Length != 64 && publicKey.Raw.Length != 66)
                throw new ArgumentException("invalid public key");
            return new UnverifiableAccount(Connection, publicKey);
        }

        /*
         * Creates an Unverifiable account from an encoded address
         * 
         * @encodedAddress { string } The address to create the account from
         * 
         * Return: UnerifiableAccount
         * 
         */
        public UnverifiableAccount FromEncodedAddress(string encodedAddress)
        {
            return new UnverifiableAccount(Connection, new Address(encodedAddress.GetResultsWithoutHyphen()));
        }

        /*
         * Creates an Unverifiable account from an encoded address Object
         * 
         * @encodedAddress { Address } The address to create the account from in Object format
         * 
         * Return: UnerifiableAccount
         * 
         */
        public UnverifiableAccount FromEncodedAddress(Address encodedAddress)
        {
            return new UnverifiableAccount(Connection, encodedAddress);
        }
    }
}