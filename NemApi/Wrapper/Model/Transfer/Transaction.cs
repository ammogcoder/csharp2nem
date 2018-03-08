using Chaos.NaCl;
using CSharp2nem.Constants;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.Transfer
{
    internal class Transaction
    {
        

        internal Transaction(Connectivity.Connection connection, PublicKey publicKey, int deadline)
        {
            Serializer = new Serializer();
            NetworkVersion = connection.GetNetworkVersion();
            TimeStamp = TimeDateUtils.EpochTimeInSeconds();
            Deadline = RoundUp(deadline == 0 ? TimeStamp + 3600 : TimeStamp + deadline);          
            PublicKey = publicKey;
            Serialize();
        }

        private Serializer Serializer { get; } 
        private PublicKey PublicKey { get; }
        private int NetworkVersion { get; }
        private int TimeStamp { get; }
        private int Deadline { get; }

        internal void UpdateTransactionType(int type)
        {
            Serializer.UpdateNthFourBytes(0, type);
        }

        internal void UpdateTransactionVersion(byte version)
        {
            Serializer.UpdateNthByte(4, version);
        }

        internal void UpdateFee(long fee)
        {
            Serializer.UpdateNthEightBytes(48, fee);
        }

        internal byte[] GetCommonTransactionBytes()
        {
            return ByteUtils.TruncateByteArray(Serializer.GetBytes(), StructureLength.TransactionCommon);
        }

        private void Serialize()
        {
            // transaction type. Set as null/empty bytes as it will be 
            // updated when serializing the different transaction types.
            Serializer.WriteInt(DefaultValues.ZeroValuePlaceHolder);

            // version
            Serializer.WriteCustomBytes(DefaultValues.ZeroValuePlaceHolder, NetworkVersion);

            // timestamp
            Serializer.WriteInt(TimeStamp);

            //pubKey len
            Serializer.WriteInt(ByteLength.PublicKeyLength);

            // public key
            Serializer.WriteBytes(CryptoBytes.FromHexString(PublicKey.Raw));

            // fee
            Serializer.WriteLong(DefaultValues.ZeroValuePlaceHolder);

            // deadline
            Serializer.WriteInt(Deadline);
        }
        int RoundUp(int toRound)
        {
            return (60 - toRound % 60) + toRound;
        }
    }
}