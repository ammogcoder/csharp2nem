using System;
using System.Threading.Tasks;
using Chaos.NaCl;
using CSharp2nem.Async;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Verifiable account contains all API's / transactions that require
     * a signature. Verifiable account inherits access to API's
     * contained in Unverifiable account.
     * 
     * 
     */
    public class VerifiableAccount : UnverifiableAccount
    {
        /*
         * Create a Verifiable Account from a given connection and private
         * key. Use AccountFactory to return a Verifiable account.
         * 
         * @connection { Connection } The connection to use for the account
         * @privateKey { PrivateKey } The private key used to create the account
         * 
         * Return: A Verifiable account object
         */
        internal VerifiableAccount(Connection connection, PrivateKey privateKey)
            : base(connection, new PublicKey(CryptoBytes.ToHexStringLower(privateKey.ToPublicKey())))
        {
            if (!privateKey.Raw.OnlyHexInString() || privateKey.Raw.Length == 64 && privateKey.Raw.Length == 66)
                    throw new ArgumentException("invalid private key");

            if (null == connection)
                throw new ArgumentNullException(nameof(connection));

            PrivateKey = privateKey;
        }

        public PrivateKey PrivateKey { get; internal set; }

        /*
         * Boots a local node. 
         * 
         * NB: This API sends the private key.
         *     Do not use this API with a 
         *     remote node unless the private key 
         *     is for an account where its safe
         *     to expose the key. eg. mutisig or delegate
         * 
         * http://bob.nem.ninja/docs/#booting-the-local-node
         * 
         * @name { string } The name you wish to give the node
         * 
         */
        public async void BootNodeAsync(string name)
        {
            const string path = "/node/boot";

            var nodeData = new BootNodeRootObject
            {
                MetaData = {Application = "NIS"},
                Identity =
                {
                    Name = name,
                    PrivateKey = PrivateKey.Raw.ConvertToUnsecureString()
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

        /*
         * Send a transaction from the current specified account
         * 
         * http://bob.nem.ninja/docs/#gathering-data-for-the-signature
         * 
         * @transactionData { TransferTransactionData } The transaction data to be sent to nis
         * 
         * Note: signing is handled locally thus it is safe to use this API with a remote node
         * 
         * Return: The response object that provides details on the status of the transaction 
         *         ie. returns code 1 if successful with message success
         */
        public async Task<NemAnnounceResponse.Response> SendTransactionAsync(TransferTransactionData transactionData)
        {
            var transfer = new TransferTransaction(Connection, PublicKey, PrivateKey, transactionData);

            return await new Prepare(Connection, PrivateKey).Transaction(transfer.GetTransferBytes());
        }

        /*
         * Transfer importance to another account ie. delegate
         * When this transaction is performed, your harvesting power is 
         * "lent" to the other account until you revoke it allowing you
         * to harvest safely and securely.
         * 
         * http://bob.nem.ninja/docs/#gathering-data-for-the-signature
         * 
         * @data { ImportanceTransferData } The transfer data required to initiate the transfer
         * 
         * Return: The response object that provides details on the status of the transaction
         * ie. returns code 1 if successful with message success
         * 
         * Note: All the following transactions return code 1 or message success for a successful transaction
         *       See http://bob.nem.ninja/docs/#appendix-B:-NIS-errors for all other error messages 
         * 
         */
        public async Task<NemAnnounceResponse.Response> ImportanceTransferAsync(ImportanceTransferData data)
        {
            var transfer = new ImportanceTransfer(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(transfer.GetBytes());
        }

        /*
         * Initiate an aggregate multisig modification transaction.
         * This API allows the modification of existing multisig accounts.
         * You can add or remove signatories, or increase or decrease the 
         * minimum signature requirement ie. delta
         * 
         * http://bob.nem.ninja/docs/#gathering-data-for-the-signature
         * 
         * @data { AggregateModificationData } The data required to initiate the transaction
         * 
         */
        public async Task<NemAnnounceResponse.Response> AggregateMultisigModificationAsync(
            AggregateModificationData data)
        {
            var aggregateModification = new AggregateModificatioList(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(aggregateModification.GetBytes());
        }

        /*
         * Provision name space. This API allows the given account to provision a namespace
         * 
         * http://bob.nem.ninja/docs/#gathering-data-for-the-signature
         * 
         * @data { ProvisionNameSpaceData } The data required to provision a namespace
         * 
         */
        public async Task<NemAnnounceResponse.Response> ProvisionNamespaceAsync(ProvisionNameSpaceData data)
        {
            var nameSpace = new ProvisionNamespace(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(nameSpace.GetBytes());
        }

        /*
         * Create mosaic. This API allows the given account to create a mosaic.
         * A mosaic must reside under a namespace or sub-namespace
         * 
         * http://bob.nem.ninja/docs/#gathering-data-for-the-signature
         * 
         * @data { MosaicCreationData } The data required to provision a namespace
         * 
         */
        public async Task<NemAnnounceResponse.Response> CreateMosaicAsync(MosaicCreationData data)
        {
            var model = new MosaicDefinition(data, data?.MultisigAccount ?? PublicKey);

            var mosaic = new CreateMosaic(Connection, PublicKey, model);

            return await new Prepare(Connection, PrivateKey).Transaction(mosaic.GetBytes());
        }

        /*
         * Change supply of a mosaic. This API allows the given account to change the supply of a mosaic.
         * A mosaic must have the property supply mutable set to true during creation for this API to 
         * be applicable
         * 
         * http://bob.nem.ninja/docs/#gathering-data-for-the-signature
         * 
         * @data { MosaicSupplyChange } The data required to chage mosaic supply
         * 
         */
        public async Task<NemAnnounceResponse.Response> MosaicSupplychangeAsync(MosaicSupplyChangeData data)
        {
            var change = new SupplyChange(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(change.GetBytes());
        }

        /*
         * Multisig signature transaction. This API allows a cosignatory to sign a pending transaction
         * that requires the cosignatories signature.
         * 
         * http://bob.nem.ninja/docs/#gathering-data-for-the-signature
         * 
         * @data { MultisigSignatureTransaction } The data required to sign the pending multisig transaction
         * 
         */
        public async Task<NemAnnounceResponse.Response> SignMultisigTransaction(MultisigSignatureTransactionData data)
        {
            var signature = new MultisigSignature(Connection, PublicKey, data);

            return await new Prepare(Connection, PrivateKey).Transaction(signature.GetBytes());
        }
    }
}