

using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.Mosaics;
using CSharp2nem.RequestClients;

namespace CSharp2nem.Model.DataModels
{
    /// <summary>
    /// The data used to initiate a mosaic creation transaction. See: <see cref="PrivateKeyAccountClient.BeginCreateMosaicAsync"/>
    /// </summary>
    public class MosaicCreationData
    {
        /// <summary>
        /// The multisig account underwhich to create the mosaic. 
        /// </summary>
        /// <remarks>
        /// The multisig account will be the issuer of the mosaic. A co-signatory of the given multisig account must be the initiator of the transaction when this property is initialised.
        /// </remarks>
        public PublicKey MultisigAccount { get; set; }

        /// <summary>
        /// The deadline by which the transaction must be accepted before it is rejected by the network.
        /// </summary>
        public int Deadline { get; set; }

        /// <summary>
        /// The namespace underwhich the mosaic should be created. To create a mosaic under a sub-namespace, concatonate the parent and child namespace with a period eg. Name.Space.
        /// </summary>
        public string NameSpaceId { get; set; }

        /// <summary>
        /// The name to give the mosaic.
        /// </summary>
        /// <remarks>
        /// Mosaic name valid characters are: a, b, c, ..., z, A, B, C, ..., Z, 0, 1, 2, ..., 9, _ , -
        /// </remarks>
        public string MosaicName { get; set; } // todo: check exact restrictions of mosaic names

        /// <summary>
        /// The desctiption to give the mosaic.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The divisibility that the mosaic should have.
        /// </summary>
        /// <remarks>
        /// The number of decimal places the mosaic quantity can have. Quantity of mosaics transfered are counted as the smallest divisible part. Transfering 1000 mosaics with a divisibility of 2 gives a total quantity of 10000    
        /// </remarks>
        public int Divisibility { get; set; }

        /// <summary>
        /// The initial supply the mosaic should have.
        /// </summary>
        public long InitialSupply { get; set; }

        /// <summary>
        /// Supply is mutable when set to true. Otherwise supply is immutable. 
        /// </summary>
        public bool SupplyMutable { get; set; }

        /// <summary>
        /// The mosaic can be transferred by secondary accounts when set to true. Otherwise, only the issuer can distribute the mosaic, while the recipients cannot.
        /// </summary>
        public bool Transferable { get; set; }


        /// <summary>
        /// The levy to impose on transfers of the mosaic. Any other mosaic can be designated as the levy fee including NEM*XEM. See <see cref="MosaicLevy"/> for further information.
        /// </summary>
        public MosaicLevy MosaicLevy { get; set; }
    }
}