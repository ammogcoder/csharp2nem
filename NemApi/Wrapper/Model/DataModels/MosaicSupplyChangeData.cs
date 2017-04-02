// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Mosaic Supply Change Data
     * 
     * @MultisigAccount { PublicKey } The public key of the multisig account for which the transaction should be initiated
     * @Deadline { int } The deadline by which the transaction must be accepted
     * @NameSpaceId { string } The namespace underwhich the mosaic resides
     * @MosaicName { string } The name of the mosaic for which the supply should be changed
     * @SupplyChangeType { int } Indicates whether the supply should be increased or decreased by the given amount/delta
     * @Delta { long } The amount by which the supply should change
     * 
     */
    public class MosaicSupplyChangeData
    {
        public PublicKey MultisigAccount { get; set; }
        public int Deadline { get; set; }
        public string NameSpaceId { get; set; }
        public string MosaicName { get; set; }
        public int SupplyChangeType { get; set; }
        public long Delta { get; set; }
    }
}