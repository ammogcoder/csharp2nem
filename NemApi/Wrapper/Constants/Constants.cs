

namespace CSharp2nem.Constants
{
    /*
    * Contains sets of default/constant values.
    *
    * 
    

    * The transaction Type to use when preparing transactions
    * 
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

    /*
     * The transaction version to use for transfer transactions
     * 
     * V1 for transactions without mosaics
     * V2 for transfers with mosaics
     * 
     * note: V2 can be used for both V2 and V1 transfers
     *       and that is what this wrapper does
     */
    internal static class TransactionVersion
    {
        internal const int VersionOne = 0x01;
        internal const int VersionTwo = 0x02;
    }

    /*
     * The fixed byte length of transaction types with 
     * out variable data eg. message payload.
     * 
     */
    internal static class StructureLength
    {
        internal const int MultisigSignature = 0x54;
        internal const int MessageStructure = 0x08;
        internal const int AggregateModification = 0x28;
        internal const int RelativeChange = 0x04;
        internal const int TransactionCommon = 0x3c;
        internal const int MultiSigHeader = 0x40;
        internal const int TransferTransaction = 0x3c;
        internal const int ProvisionNameSpace = 0x38;
        internal const int ImportnaceTransfer = 0x28;
        internal const int TransactionHash = 0x24;
        internal const int MosaicObject = 0x14;
        internal const int NoOfPropertyBytes = 0x04;
        internal const int LevyStructureBytes = 0x08;
        internal const int CreationFeeSinkBytesAfterLevy = 0x2c;
        internal const int MosaicLevy = 0x3c;
    }

    /*
     * Fixed byte legnth of data to reduce use of hard coded values
     * 
     */
    internal static class ByteLength
    {
        internal const int PublicKeyLength = 0x20;
        internal const int HashLength = 0x20;
        internal const int AddressLength = 0x28;
        internal const int FourBytes = 0x04;
        internal const int EightBytes = 0x08;
        internal const int Zero = 0x00;
    }

    /*
     * Default byte values to reduce use of hard coded values
     * 
     * 
     */
    internal static class DefaultBytes
    {
        internal static byte[] MaxByteValue = {0xff, 0xff, 0xff, 0xff};
        internal static byte[] ZeroByteValue = {0x00, 0x00, 0x00, 0x00};
        internal static byte[] Activate = {0x01, 0x00, 0x00, 0x00};
        internal static byte[] Deactivate = {0x02, 0x00, 0x00, 0x00};
    }

    /*
     * Default values for transactions
     * 
     * mainnet fee sink is the account to which namespace rental fees are sent
     * mainnet fee sink is the account to which namespace creation fees are sent
     */
    internal static class DefaultValues
    {
        internal const string MainNetRentalFeeSinkPublicKey =
            "3e82e1c1e4a75adaa3cba8c101c3cd31d9817a2eb966eb3b511fb2ed45b8e262";

        internal const string MainNetCreationFeeSink =
            "53e140b5947f104cabc2d6fe8baedbc30ef9a0609c717d9613de593ec2a266d3";

        internal const string EmptyString = "";
        internal const int ZeroValuePlaceHolder = 0;
    }

    /*
     * Fixed aditional fees for transaction types
     * 
     */
    internal static class Fee
    {
        internal const long SubSpaceRental = 0x989680;
        internal const long Rental = 0x5F5E100;
        internal const long Creation = 0x989680;
        internal const int Absolute = 0x01;
        internal const int Percentile = 0x02;
    }

    /*
     * Fixed fees for transaction types
     * 
     */
    internal static class TransactionFee
    {
        internal const int SupplyChange = 0x249F0;
        internal const int ProvisionNameSpace = 0x249F0;
        internal const int MosaicDefinitionCreation = 0x249F0;

        internal const int MultisigAggMod = 0x7A120;
        internal const int MultisigSignature = 0x249F0;
        internal const int MultisigWrapper = 0x249F0;
        internal const int ImportanceTransfer = 0x249F0;
    }
}