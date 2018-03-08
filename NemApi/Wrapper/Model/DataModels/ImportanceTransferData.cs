

using CSharp2nem.Model.AccountSetup;
using CSharp2nem.RequestClients;

namespace CSharp2nem.Model.DataModels
{
    /// <summary>
    /// The data required to initiate an <see cref="PrivateKeyAccountClient.BeginImportanceTransferAsync"/> transaction.
    /// </summary>
    /// <remarks>
    /// <see cref="ImportanceTransferData"/> should be initialised with data required to initiate an importance transfer transaction.
    /// </remarks>
    public class ImportanceTransferData
    {

        /// <summary>
        /// The multisig account that should transfer its importance to a delegated account. 
        /// </summary>
        /// <remarks>
        /// A co-signatory of the given multisig account must be the initiator of the transaction when this property is initialised. When not initialised, the transfer is done on behalf of the signing account.
        /// </remarks>
        public PublicKey MultisigAccount { get; set; }

        /// <summary>
        /// The account to transfer importance to. This should be the account that is used for delegated harvesting and should not contain XEM.
        /// </summary>
        public PublicKey DelegatedAccount { get; set; }

        /// <summary>
        /// Set true to activate an importance transfer to a delegated account. Set to false to revoke an importance transfer from a delegated account.
        /// </summary>
        public bool Activate { get; set; }

        /// <summary>
        /// The deadline when the transction must be accepted by before it is rejected by the network.
        /// </summary>
        public int Deadline { get; set; }
    }
}