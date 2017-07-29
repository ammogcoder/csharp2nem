using System;
using Chaos.NaCl;
using CSharp2nem.CryptographicFunctions;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.ResponseObjects;

namespace CSharp2nem.PrepareHttpRequests
{
    internal class Prepare
    {
        /*
         * Prepare the transaction for announcement
         * 
         * @Connection The connection to use
         * @ PrivateKey The private key used to sign the transaction
         */
        internal Prepare(Connectivity.Connection connection, PrivateKey privateKey)
        {
            Connection = connection;
            PrivateKey = privateKey;
        }

        private Connectivity.Connection Connection { get; }
        private PrivateKey PrivateKey { get; }

        internal ByteArrayWtihSignature Transaction(byte[] bytes)
        {
            if (null == bytes)
                throw new ArgumentNullException(nameof(bytes));
           
            // produce the signature
            var r = CryptoBytes.ToHexStringUpper(new Signature(bytes, PrivateKey)._Signature);

            ByteArrayWtihSignature byteArrayWithSignature = new ByteArrayWtihSignature
            {
                data = CryptoBytes.ToHexStringUpper(bytes),
                signature = r.ToUpper()
            };
       
            return byteArrayWithSignature;
        }

    }
}