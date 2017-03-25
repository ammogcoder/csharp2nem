using System;
using System.Threading.Tasks;
using Chaos.NaCl;
using NemApi.internals;

namespace NemApi.Async
{
    internal class Prepare
    {
        internal Prepare(Connection connection, PrivateKey privateKey)
        {
            Connection = connection;
            PrivateKey = privateKey;
        }

        private Connection Connection { get; }
        private PrivateKey PrivateKey { get; }

        internal async Task<NemAnnounceResponse.Response> Transaction(byte[] bytes)
        {
            if (null == bytes)
                throw new ArgumentNullException(nameof(bytes));

            var r = CryptoBytes.ToHexStringUpper(new Signature(bytes, PrivateKey).LongSignature);

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