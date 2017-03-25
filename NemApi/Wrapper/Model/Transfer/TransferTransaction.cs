using System;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    /*
    * Serializable transfer transaction data
    */

    internal class TransferTransaction : Transaction
    {
        private readonly Serializer _serializer = new Serializer();

        internal TransferTransaction(Connection connection, PublicKey senderPublicKey, PrivateKey senderPrivateKey,
            TransferTransactionData transactionData)
            : base(connection, transactionData.MultisigAccount ?? senderPublicKey, transactionData.Deadline)
        {
            SenderPublicKey = senderPublicKey;
            MessageLength = StructureLength.TransferTransaction;
            Data = transactionData;
            Con = connection;
            SenderPrivateKey = senderPrivateKey;

            SerializeTrasnferPart();
            SerializeAttachments();

            TransferBytes = ByteUtils.TruncateByteArray(_serializer.GetBytes(), MessageLength + MosaicLength);

            finalize();
            AppendMultisig();
        }

        private PrivateKey SenderPrivateKey { get; }
        private PublicKey SenderPublicKey { get; }
        private Connection Con { get; }
        private TransferTransactionData Data { get; }

        private int MessageLength { get; set; }
        private int MosaicLength { get; set; }
        private long Fee { get; set; }

        private byte[] TransferMessageMosaicBytes { get; set; }
        private byte[] TransferBytes { get; set; }

        private void SerializeAttachments()
        {
            if (Data.Message != null)
                SerializeMessagePart();
            else _serializer.WriteInt(ByteLength.Zero);

            if (Data.ListOfMosaics != null)
                SerializeMosaicPart();              
            else _serializer.WriteInt(ByteLength.Zero);
    
        }

        internal byte[] GetTransferBytes()
        {
            return TransferMessageMosaicBytes;
        }

        private void finalize()
        {
            UpdateFee(Fee);
            UpdateTransactionType(TransactionType.TransferTransaction);
            UpdateTransactionVersion(TransactionVersion.VersionTwo);

            TransferMessageMosaicBytes = new byte[GetCommonTransactionBytes().Length + MessageLength + MosaicLength];
            Array.Copy(GetCommonTransactionBytes(), TransferMessageMosaicBytes, GetCommonTransactionBytes().Length);
            Array.Copy(TransferBytes, 0, TransferMessageMosaicBytes, GetCommonTransactionBytes().Length, MessageLength + MosaicLength);
        }

        private void SerializeTrasnferPart()
        {
            _serializer.WriteInt(ByteLength.AddressLength);
            _serializer.WriteString(Data.Recipient.Address.Encoded);
            _serializer.WriteLong(Data.Amount);

            Fee += Math.Max(1, Math.Min((long) Math.Ceiling((decimal) Data.Amount / 10000000000), 25)) * 1000000;
        }

        private void SerializeMessagePart()
        {
            var serializeMessage = new Message(Con, SenderPrivateKey, Data.Recipient, Data.Message, Data.Encrypted);
            _serializer.WriteBytes(serializeMessage.GetMessageBytes());
            MessageLength += serializeMessage.Length;

            Fee += serializeMessage.GetFee();
        }

        private void SerializeMosaicPart()
        {
            var mosaicList = new MosaicList(Data.ListOfMosaics, Con, SenderPublicKey);
            _serializer.WriteBytes(mosaicList.GetMosaicListBytes());
            MosaicLength = mosaicList.GetMosaicListBytes().Length;

            Fee += mosaicList.GetFee();
        }

        private void AppendMultisig()
        {
            if (Data.MultisigAccount == null) return;

            var multisig = new MultiSigTransaction(Con, SenderPublicKey, Data.Deadline, TransferMessageMosaicBytes.Length);

            TransferMessageMosaicBytes = ByteUtils.ConcatonatatBytes(multisig.GetBytes(), TransferMessageMosaicBytes);
        }
    }
}