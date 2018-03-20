namespace WrapperRewrite
{
    /*
    * Contains sets of default/constant values.
    */
    internal static class TransactionType
    {
        internal const int TransferTransaction = 0x0101;
        internal const int ImportanceTransfer = 0x0801;
        internal const int MultisigAggregateModification = 0x1001;
        internal const int MultisigSignatureTransaction = 0x1002;
        internal const int MultisigTransaction = 0x1004;
        internal const int ProvisionNamespace = 0x2001;
        internal const int MosaicDefinitionCreation = 0x4001;
        internal const int MosiacSupplyChange = 0x4002;

    }
    internal static class Network
    {
        internal const byte LiveNet = 0x68;
        internal const byte TestNet = 0x68;
    }
    internal static class TransactionVersion
    {
        internal const int VersionOne = 0x01;
        internal const int VersionTwo = 0x02;
    }
    internal static class StructureLength
    {
        internal const int AggregateModification = 0x28;
        internal const int RelativeChange = 0x04;
        internal const int TransactionCommon = 0x3c;
        internal const int TransferTransaction = 0x3c;
        internal const int ProvisionNameSpace = 0x38;
        internal const int ImportnaceTransfer = 0x28;
        internal const int MultisigWrapper = 0x6c;
        internal const int TransactionHash = 0x24;
        internal const int MosaicObject = 0x14;
        internal const int MosaicDefinitionAndPropertiesFixedByteLength = 0x20;
        internal const int NoOfPropertyBytes = 0x04;
        internal const int LevyStructureBytes = 0x08;
        internal const int CreationFeeSinkBytesAfterLevy = 0x2c;
        internal const int MosaicLevy = 0x3c;
    }

    internal static class ByteLength
    {
        internal const int PublicKeyLength = 0x20;
        internal const int HashLength = 0x20;
        internal const int AddressLength = 0x28;
        internal const int FourBytes = 0x04;
        internal const int EightBytes = 0x08;
        internal const int ZERO = 0x00;
        
    }

    internal static class DefaultBytes
    {
        internal static byte[] MaxByteValue = new byte[]{  0xff, 0xff, 0xff, 0xff };
        internal static byte[] ZeroByteValue = new byte[] { 0x00, 0x00, 0x00, 0x00 };
        internal static byte[] Activate = new byte[] { 0x01, 0x00, 0x00, 0x00 };
        internal static byte[] Deactivate = new byte[] { 0x02, 0x00, 0x00, 0x00 };

    }
    internal static class DefaultValues
    {
        internal const string RentalFeeSinkPublicKey = "3e82e1c1e4a75adaa3cba8c101c3cd31d9817a2eb966eb3b511fb2ed45b8e262";
        internal const string CreationFeeSink = "53e140b5947f104cabc2d6fe8baedbc30ef9a0609c717d9613de593ec2a266d3";
        internal const string EmptyString = "";
        internal const int ZeroValuePlaceHolder = 0;
    }
    internal static class Fee
    {
        internal const long Rental = 0xBA43B7400;
        internal const long Creation = 0xBA43B7400;
        internal const int Absolute = 0x01;
        internal const int Percentile = 0x02;
    }
}
