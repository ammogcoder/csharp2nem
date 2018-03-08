using System.Collections.Generic;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.MultiSig;
using CSharp2nem.RequestClients;

namespace CSharp2nem.Model.DataModels
{

    /// <summary>
    /// The data required to initiate an aggregate multisig modification transaction. See: <see cref="PrivateKeyAccountClient.BeginAggregateMultisigModificationAsync"/>
    /// </summary>
    /// <remarks>
    /// An aggregate modification transaction must be initiated by a cosignatory of the multisig account to be modified. 
    /// </remarks>
    public class AggregateModificationData
    {
        /// <summary>
        /// Gets or sets the modifications.
        /// </summary>
        /// <value>
        /// The modifications.
        /// </value>
        /// <remarks>
        /// Modifications is a list of modifications to make to a multisignature account. See the <see cref="AggregateModification"/> for further details.
        /// </remarks>
        public List<AggregateModification> Modifications { get; set; }

        /// <summary>
        /// Gets or sets the multisig account.
        /// </summary>
        /// <value>
        /// The multisig account that should me modified.
        /// </value>
        public PublicKey MultisigAccount { get; set; }

        /// <summary>
        /// Gets or sets the relative change.
        /// </summary>
        /// <value>
        /// The relative change in the number of minimum cosignatories. '1' denotes an increase of the current minimum signatures required by 1. '-1' denotes an increase of the current minimum signatures required by -1.
        /// </value>
        public int RelativeChange { get; set; }

        /// <summary>
        /// Gets or sets the deadline.
        /// </summary>
        /// <value>
        /// The deadline by which the transaction should be included. Max deadline value allowed is 86400, or 24 hours.
        /// </value>
        public int Deadline { get; set; }
    }
}