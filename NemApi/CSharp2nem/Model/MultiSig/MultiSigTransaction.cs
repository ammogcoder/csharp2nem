using System;
using Chaos.NaCl;
using CSharp2nem.Constants;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.MultiSig
{
    internal class MultiSigTransaction
    {
        internal MultiSigTransaction(Connectivity.Connection connection, PublicKey publicKey, int deadline, int length)

        {
            if (null == connection)
                throw new ArgumentNullException(nameof(connection));
            if (null == publicKey)
                throw new ArgumentNullException(nameof(publicKey));

            InnerLength = length;

            Serializer = new Serializer();

            NetworkVersion = connection.GetNetworkVersion();

            TimeStamp = TimeDateUtils.EpochTimeInSeconds();

            Deadline = deadline == 0 ? TimeStamp + 1000 : TimeStamp + deadline;

            PublicKey = publicKey;

            Fee = TransactionFee.MultisigWrapper;

            Serialize();

            MultiSigBytes = ByteUtils.TruncateByteArray(Serializer.GetBytes(), StructureLength.MultiSigHeader);

            MultiSigBytes[7] = NetworkVersion;           
        }

        private Serializer Serializer { get; }
        private PublicKey PublicKey { get; }
        private int Deadline { get; }
        private byte[] MultiSigBytes { get; }
        private byte NetworkVersion { get; }
        private int TimeStamp { get; }
        private long Fee { get; }
        private int InnerLength { get; }

        internal byte[] GetBytes()
        {
            return MultiSigBytes;
        }

        private void Serialize()
        {
            // transaction type. Set as null/empty bytes as it will be 
            // updated when serializing the different transaction types.
            Serializer.WriteInt(TransactionType.MultisigTransaction);

            // version
            Serializer.WriteInt(TransactionVersion.VersionOne);

            // timestamp
            Serializer.WriteInt(TimeStamp);

            //pubKey len
            Serializer.WriteInt(ByteLength.PublicKeyLength);

            // pub key
           
            Serializer.WriteBytes(CryptoBytes.FromHexString(PublicKey.Raw));

            // fee
            Serializer.WriteLong(Fee);

            // deadline
            Serializer.WriteInt(Deadline);

            Serializer.WriteInt(InnerLength);
        }
    }
}