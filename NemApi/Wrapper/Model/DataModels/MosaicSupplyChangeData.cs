// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class MosaicSupplyChangeData
    {
        public PublicKey MultisigAccount { get; set; }
        public int Deadline { get; set; }
        public string NameSpaceId { get; set; }
        public string MosaicName { get; set; }
        public int SupplyChangeType { get; set; }
        public int Delta { get; set; }
    }
}