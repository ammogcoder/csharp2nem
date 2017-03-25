using System;
using System.Threading.Tasks;
using Chaos.NaCl;
using NemApi.Async;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class VerifiableAccount : UnverifiableAccount
    {
        internal VerifiableAccount(Connection connection, PrivateKey privateKey)
            : base(connection, new PublicKey(CryptoBytes.ToHexStringLower(PublicKeyConversion.ToPublicKey(privateKey))))
        {
            if (!StringUtils.OnlyHexInString(privateKey.Raw) ||
                privateKey.Raw.Length == 64 && privateKey.Raw.Length == 66)
                throw new ArgumentException("invalid private key");
            if (null == connection)
                throw new ArgumentNullException(nameof(connection));

            PrivateKey = privateKey;
        }

        public PrivateKey PrivateKey { get; internal set; }

        public async void BootNodeAsync(string name)
        {
            const string path = "/node/boot";

            var nodeData = new BootNodeRootObject
            {
                MetaData = {Application = "NIS"},
                Identity =
                {
                    Name = name,
                    PrivateKey = StringUtils.ConvertToUnsecureString(PrivateKey.Raw)
                },
                Endpoint =
                {
                    Host = Connection.GetUri().Host,
                    Port = Connection.GetUri().Port,
                    Protocol = "http"
                }
            };

            await new AsyncConnector.PostAsync(Connection).Post(path, nodeData);
        }

        public async Task<NemAnnounceResponse.Response> SendTransactionAsync(TransferTransactionData transactionData)
        {
            var transfer = new TransferTransaction(Connection, PublicKey, PrivateKey, transactionData);

            return await new Prepare(Connection, PrivateKey).Transaction(transfer.GetTransferBytes());
        }

        public async Task<NemAnnounceResponse.Response> ImportanceTransferAsync(ImportanceTransferData data)
        {
            var transfer = new ImportanceTransfer(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(transfer.GetBytes());
        }

        public async Task<NemAnnounceResponse.Response> AggregateMultisigModificationAsync(
            AggregateModificationData data)
        {
            var aggregateModification = new AggregateModificatioList(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(aggregateModification.GetBytes());
        }

        public async Task<NemAnnounceResponse.Response> ProvisionNamespaceAsync(ProvisionNameSpaceData data)
        {
            var nameSpace = new ProvisionNamespace(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(nameSpace.GetBytes());
        }

        public async Task<NemAnnounceResponse.Response> CreateMosaicAsync(MosaicCreationData data)
        {
            var model = new MosaicDefinition(data, data?.MultisigAccount ?? PublicKey);

            var mosaic = new CreateMosaic(Connection, PublicKey, model);

            return await new Prepare(Connection, PrivateKey).Transaction(mosaic.GetBytes());
        }

        public async Task<NemAnnounceResponse.Response> MosaicSupplychangeAsync(MosaicSupplyChangeData data)
        {
            var change = new SupplyChange(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(change.GetBytes());
        }

        public async Task<NemAnnounceResponse.Response> SignMultisigTransaction(MultisigSignatureTransactionData data)
        {
            var signature = new MultisigSignature(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(signature.GetBytes());
        }
    }
}