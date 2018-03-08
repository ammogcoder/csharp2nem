using CSharp2nem.Model.AccountSetup;
using CSharp2nem.RequestClients;

namespace CSharp2nem.Model.DataModels
{
    /// <summary>
    /// The data used to initiate a mosaic supply change transaction. See: <see cref="PrivateKeyAccountClient.BeginMosaicSupplyChangeAsync"/>
    /// </summary>
    public class MosaicSupplyChangeData
    {
        /// <summary>
        /// The multisig account that owns the mosaic. 
        /// </summary>
        /// <remarks>
        /// A co-signatory of the given multisig account must be the initiator of the transaction when this property is initialised. When not initialised, the supply change is done on behalf of the signing account.
        /// </remarks>
        public PublicKey MultisigAccount { get; set; }

        /// <summary>
        /// The deadline by which the transaction must be accepted before it is rejected by the network.
        /// </summary>
        public int Deadline { get; set; }

        /// <summary>
        /// The namespace underwhich the mosaic was created.
        /// </summary>
        public string NameSpaceId { get; set; }

        /// <summary>
        /// The name of the mosaic for which the supply should be changed.
        /// </summary>
        public string MosaicName { get; set; }

        /// <summary>
        /// Indicates whether the supply should be increased or decreased by the given amount/delta. 
        /// </summary>
        /// <remarks>
        /// A type of 1 indicates that the supply should be increased. A type of 2 indicates that the supply should be decreased.
        /// </remarks>
        public int SupplyChangeType { get; set; }

        /// <summary>
        /// The amount in units by which the supply should change.
        /// </summary>
        public long Delta { get; set; }
    }
}