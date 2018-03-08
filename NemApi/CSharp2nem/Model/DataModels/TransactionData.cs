using System.Collections.Generic;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.Transfer.Mosaics;
using CSharp2nem.RequestClients;

namespace CSharp2nem.Model.DataModels
{
    /// <summary>
    /// The data required to initiate a transfer transaction. See: <see cref="PrivateKeyAccountClient.BeginSendTransactionAsync"/>
    /// </summary>
    public class TransferTransactionData
    {
        /// <summary>
        /// The multisig account that should send the transaction.
        /// </summary>
        /// <remarks>
        /// A co-signatory of the given multisig account must be the initiator of the transaction when this property is initialised. When not initialised, the transaction is sent by the signing account.
        /// </remarks>
        public PublicKey MultisigAccountKey { get; set; }

        /// <summary>
        /// The recipient of the transfer.
        /// </summary>
        public string RecipientAddress { get; set; }

        /// <summary>
        /// The amount of XEM to transfer.
        /// </summary>
        /// <remarks>
        /// The amount field is denominated in microXEM. ie. 1000000 denotes a transfer of 1 whole XEM.
        /// </remarks>
        public long Amount { get; set; }

        /// <summary>
        /// The list of mosaics to send.
        /// </summary>
        /// <remarks>
        /// One or more mosaics can be transferred at one time. The mosaics must be formatted in a List of <see cref="Mosaic"/> instances, whether one or more are sent.
        /// </remarks>
        public List<Mosaic> ListOfMosaics { get; set; }

        /// <summary>
        /// The message to attach to the transfer.
        /// </summary>
        /// <remarks>
        /// A message may be a maximum of x characters long. TODO: find out max length.
        /// </remarks>
        public string Message { get; set; }

        /// <summary>
        /// Denotes whether the message should be encrypted.
        /// </summary>
        public bool Encrypted { get; set; }
        /// <summary>
        /// The deadline by which the transfer should be sent.
        /// </summary>
        /// <remarks>
        /// MAximum value is 86000, or 24 hours.
        /// </remarks>
        public int Deadline { get; set; }

        /// <summary>
        /// Denotes whether the fee for transfer should be deducted from the amount. 
        /// </summary>
        /// <remarks>
        /// If set to true, the fee will be deducted from the amount. If the fee decreases as a result of the amount to be sent being lowered, the amount and fee will be recalibrated to account for the reduced fee.
        /// </remarks>
        public bool FeeDeductedFromAmount { get; set; }
    }
}