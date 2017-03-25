// ReSharper disable once CheckNamespace

namespace NemApi
{
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