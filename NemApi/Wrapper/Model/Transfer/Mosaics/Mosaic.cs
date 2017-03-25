using System.Text;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class Mosaic
    {
        public Mosaic(string nameSpaceId, string mosaicName, long quantity)
        {
            NameSpaceId = nameSpaceId;
            MosaicName = mosaicName;
            LengthOfMosaicName = Encoding.Default.GetBytes(mosaicName).Length;
            LengthOfNameSpaceId = Encoding.Default.GetBytes(nameSpaceId).Length;
            LengthOfMosaicIdStructure = LengthOfMosaicName + LengthOfNameSpaceId + ByteLength.EightBytes;
            LengthOfMosaicStructure = StructureLength.MosaicObject + LengthOfNameSpaceId + LengthOfMosaicName;
            Quantity = quantity;
        }

        internal int LengthOfMosaicStructure { get; set; }
        internal int LengthOfMosaicIdStructure { get; set; }
        internal int LengthOfNameSpaceId { get; set; }
        internal int LengthOfMosaicName { get; set; }
        internal string NameSpaceId { get; set; }
        internal string MosaicName { get; set; }
        internal long Quantity { get; set; }
    }
}