using System;
using System.Text;
using CSharp2nem.Constants;

namespace CSharp2nem.Model.Transfer.Mosaics
{
    /// <summary>
    /// Creates an instance of a Mosaic that should be transferred. 
    /// </summary>
    public class Mosaic
    {
        /// <summary>
        /// Constructs an instance of the <see cref="Mosaic"/> class.
        /// </summary>
        /// <remarks>
        /// Holds and prepares the data for a transfer transaction. The quantity should be in the smallest possible units of the mosaic. For example: to send 10 whole mosaics of a mosaic with divisibility of 3, set the quantity to 10000. When transferring mosaics, Mosaic instances should be in the form of a list.
        /// </remarks>
        /// <param name="nameSpaceId">The namespace under which the mosaic to be transferred was created.</param>
        /// <param name="mosaicName">The name of the mosaic to be transferred.</param>
        /// <param name="quantity">The quantity to be transferred</param>
        /// <exception cref="ArgumentNullException">namespaceId cannot be null.</exception>
        /// <exception cref="ArgumentNullException">mosaicName cannot be null.</exception>
        public Mosaic(string nameSpaceId, string mosaicName, long quantity)
        {
            if (nameSpaceId == null) throw new ArgumentNullException(nameof(nameSpaceId));
            if (mosaicName == null) throw new ArgumentNullException(nameof(mosaicName));

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
        public string NameSpaceId { get; set; }
        public string MosaicName { get; set; }
        public long Quantity { get; set; }
    }
}