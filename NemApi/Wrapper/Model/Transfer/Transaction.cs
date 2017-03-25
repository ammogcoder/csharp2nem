using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    internal class Transaction
    {
        internal Transaction(Connection connection, PublicKey publicKey, int deadline)
        {
            NetworkVersion = connection.GetNetworkVersion();
            TimeStamp = TimeDateUtils.EpochTimeInMilliSeconds();
            Deadline = deadline == 0 ? TimeStamp + 1000 : TimeStamp + deadline;
            PublicKey = publicKey;

            Serialize();
        }

        internal Serializer Serializer1 { get; set; } = new Serializer();
        private PublicKey PublicKey { get; }
        private int NetworkVersion { get; }
        private int TimeStamp { get; }
        private int Deadline { get; }

        internal void UpdateTransactionType(int type)
        {
            Serializer1.UpdateNthFourBytes(0, type);
        }

        internal void UpdateTransactionVersion(byte version)
        {
            Serializer1.UpdateNthByte(4, version);
        }

        internal void UpdateFee(long fee)
        {
            Serializer1.UpdateNthEightBytes(48, fee);
        }

        internal byte[] GetCommonTransactionBytes()
        {
            return ByteUtils.TruncateByteArray(Serializer1.GetBytes(), StructureLength.TransactionCommon);
        }

        private void Serialize()
        {
            // transaction type. Set as null/empty bytes as it will be 
            // updated when serializing the different transaction types.
            Serializer1.WriteInt(DefaultValues.ZeroValuePlaceHolder);

            // version
            Serializer1.WriteCustomBytes(DefaultValues.ZeroValuePlaceHolder, NetworkVersion);

            // timestamp
            Serializer1.WriteInt(TimeStamp);

            //pubKey len
            Serializer1.WriteInt(ByteLength.PublicKeyLength);

            // public key
            Serializer1.WriteBytes(CryptoBytes.FromHexString(PublicKey.Raw));

            // fee
            Serializer1.WriteLong(DefaultValues.ZeroValuePlaceHolder);

            // deadline
            Serializer1.WriteInt(Deadline);
        }
    }
}