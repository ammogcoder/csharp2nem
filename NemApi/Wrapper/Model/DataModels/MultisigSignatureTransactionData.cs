using CSharp2nem.Model.AccountSetup;
using CSharp2nem.RequestClients;

namespace CSharp2nem.Model.DataModels
{
    /// <summary>
    /// The data used to initiate a multisig signature transaction. See: <see cref="PrivateKeyAccountClient.BeginSignatureTransactionAsync"/>
    /// </summary>
    /// <remarks>
    /// Used to sign a pending multisignature transaction
    /// </remarks>
    public class MultisigSignatureTransactionData
    {
        /// <summary>
        /// The transaction hash of the transaction to sign.
        /// </summary>
        public string TransactionHash { get; set; }

        /// <summary>
        /// The deadline by which the transaction should be accepted.
        /// </summary>
        public int Deadline { get; set; }

        /// <summary>
        /// The multisig account address that contains the pending transaction.
        /// </summary>
        public Address MultisigAddress { get; set; }
    }
}