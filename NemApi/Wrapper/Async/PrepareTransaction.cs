using System;
using System.Threading.Tasks;
using Chaos.NaCl;
using CSharp2nem.internals;

namespace CSharp2nem.Async
{
    internal class Prepare
    {
        /*
         * Prepare the transaction for announcement
         * 
         * @Connection The connection to use
         * @ PrivateKey The private key used to sign the transaction
         */
        internal Prepare(Connection connection, PrivateKey privateKey)
        {
            Connection = connection;
            PrivateKey = privateKey;
        }

        private Connection Connection { get; }
        private PrivateKey PrivateKey { get; }

        /*
         * Prepare the transaction for broadcast
         * 
         * @bytes The transaction bytes to sign
         */
        internal async Task<NemAnnounceResponse.Response> Transaction(byte[] bytes)
        {
            if (null == bytes)
                throw new ArgumentNullException(nameof(bytes));

            // produce the signature
            var r = CryptoBytes.ToHexStringUpper(new Signature(bytes, PrivateKey).LongSignature);

            // create the transaction and signature object to announce
            var transaction = new AsyncConnector.PostAsync(Connection)
            {
                Rpa = new ByteArrayWtihSignature
                {
                    data = CryptoBytes.ToHexStringUpper(bytes),
                    signature = r.ToUpper()
                }
            };

            return await transaction.Send();
        }
    }
}