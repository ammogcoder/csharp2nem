using System;
using System.Text;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class MosaicLevy
    {
        private readonly Serializer Serializer = new Serializer();

        public MosaicLevy(Address feeBeneficiary, Mosaic mosaic, int feeType)
        {
            if (feeType != 1 && feeType != 2)
                throw new ArgumentException("invalid fee");

            FeeBeneficiary = feeBeneficiary;
            FeeType = feeType == 1 ? Fee.Absolute : Fee.Percentile;
            Mosaic = mosaic;
            Length = 72
                     + Mosaic.LengthOfNameSpaceId
                     + Mosaic.LengthOfMosaicName;


            SerializeLevy();
        }

        internal Address FeeBeneficiary { get; set; }
        internal Mosaic Mosaic { get; set; }
        internal int FeeType { get; set; }
        internal int Length { get; set; }
        internal byte[] LevyBytes { get; set; }

        internal void SerializeLevy()
        {
            // length of levy structure
            Serializer.WriteInt(Length - 4);

            // fee type
            Serializer.WriteInt(FeeType);

            // length of recipient address
            Serializer.WriteInt(ByteLength.AddressLength);


            // address of beneficiary            
            Serializer.WriteString(FeeBeneficiary.Encoded);

            // length of mosaic id structure
            Serializer.WriteInt(Mosaic.LengthOfMosaicIdStructure);

            // length of name space
            Serializer.WriteInt(Mosaic.LengthOfNameSpaceId);

            // name space
            Serializer.WriteBytes(Encoding.Default.GetBytes(Mosaic.NameSpaceId));

            // length of mosaic name
            Serializer.WriteInt(Mosaic.LengthOfMosaicName);

            // mosaic name
            Serializer.WriteBytes(Encoding.Default.GetBytes(Mosaic.MosaicName));

            // fee amount
            Serializer.WriteLong(Mosaic.Quantity);

            LevyBytes = Serializer.GetBytes().TruncateByteArray(Length);
        }

        internal byte[] GetBytes()
        {
            return LevyBytes;
        }
    }
}