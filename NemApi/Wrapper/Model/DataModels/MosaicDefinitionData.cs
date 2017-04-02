// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Mosaic Creation Data
     * 
     * @MultisigAccount { PublicKey } The multisig account underwhich to create the mosaic
     * @Deadline { int } The deadline by which the transaction must be accepted
     * @NameSpaceId { string } The namespace underwhich the mosaic should be created. 
     *                         note: to create a mosaic under a sub-space, concatonate the 
     *                         parent and chile name space with a period eg. Name.Space
     *                         
     * @MosaicName { string } The name to give the mosaic
     * @Description { string } The desctiption to give the mosaic
     * @Divisibility { int } The number of decimal places the mosaic quantity can be broken down to
     *                       note:quantity of mosaics transfered are counted as the smallest divisible part
     *                       Transfering 1000 mosaics with a divisibility of 2 gives a total quantity of 10000    
     * @InitialSupply { long } The initial supply the mosaic should have
     * @SupplyMutable { bool } Indicates whether the supply is fixed or modifiable.
     * @Transferable { bool } Indicates whether the mosaic shoud be transferable
     * @MosaicLevy { MosaicLevy } Indicates whether an additional fee in the form of another mosaic should be paid
     *                            and to who it should be paid
     */
    public class MosaicCreationData
    {
        public PublicKey MultisigAccount { get; set; }
        public int Deadline { get; set; }
        public string NameSpaceId { get; set; }
        public string MosaicName { get; set; }
        public string Description { get; set; }
        public int Divisibility { get; set; }
        public long InitialSupply { get; set; }
        public bool SupplyMutable { get; set; }
        public bool Transferable { get; set; }
        public MosaicLevy MosaicLevy { get; set; }
    }
}