using System;
using System.Text;
using CSharp2nem.Constants;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.Transfer.Mosaics;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.Mosaics
{
    /// <summary>
    /// Creates the mosaic levy that can be included when creating a new mosaic.
    /// </summary>
    public class MosaicLevy
    {
        private readonly Serializer Serializer = new Serializer();

        /// <summary>
        /// Constructs an instance of the <see cref="MosaicLevy"/> class.
        /// </summary>
        /// <param name="feeBeneficiary"> The beneficiary <see cref="Address"/> of the Levy</param>
        /// <param name="mosaic">The <see cref="Mosaic"/> definition to create</param>
        /// <param name="feeType">The fee type for the levy.</param>
        /// <exception cref="ArgumentException">invalid fee type. must be a value of 1 or 2</exception>
        /// <remarks>
        /// For the levy fee type, 1 denotes absolute fee. 2 denotes percentile fee. The qauntity field of the Mosaic isntance, denotes the fee that should be paid in that mosaic anytime the mosaic to be created is transferred.
        /// </remarks>
        /// <example> 
        /// This sample shows how to create an instance of a <see cref="MosaicLevy"/> instance.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {  
        ///         Address beneficiary = new Address("");
        /// 
        ///         Mosaic mosaic = new Mosaic("namespace.sub", "mosaic", 1000);       
        /// 
        ///         MosaicLevy levy = new MosaicLevy(beneficiary, mosaic, 1);
        ///     }
        /// }
        /// </code>
        /// </example>
        public MosaicLevy(Address feeBeneficiary, Mosaic mosaic, int feeType)
        {
            if (feeType != 1 && feeType != 2)
                throw new ArgumentException("invalid fee type");

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

            LevyBytes = ByteUtils.TruncateByteArray(Serializer.GetBytes(), Length);
        }

        internal byte[] GetBytes()
        {
            return LevyBytes;
        }
    }
}