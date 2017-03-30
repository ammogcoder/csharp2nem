using System;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
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
           

            TransferBytes = _serializer.GetBytes().TruncateByteArray(MessageLength + MosaicLength);

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
            // declare and serialize message if not null
            Message serializedMessage = null;
            if (Data.Message != null)
                serializedMessage = SerializeMessagePart();
            
            // declare and serialize mosaics if not null
            MosaicList serializedMosaicList = null;
            if (Data.ListOfMosaics != null)
                serializedMosaicList = SerializeMosaicPart();
           
            // get the fee for transfer amount
            var transferFee = Math.Max(1, Math.Min((long)Math.Ceiling((decimal)Data.Amount / 10000000000), 25)) * 1000000;

            // get the fee for message part, 0 if its null
            var messageFee = serializedMessage?.GetFee() ?? 0;

            // get the fee for mosaic part, 0 if null
            var mosaicFee = serializedMosaicList?.GetFee() ?? 0;

            // if fee is to be deducted from amount, calculate accordingly
            if (Data.FeeDeductedFromAmount)
            {
                // minus total fees from original amount.
                long newAmount;

                long newFee;

                do
                {
                    newAmount = Data.Amount - (messageFee + mosaicFee + transferFee);

                    // calculate new fee based on new fee deducted amount
                    newFee = Math.Max(1, Math.Min((long)Math.Ceiling((decimal)newAmount / 10000000000), 25)) * 1000000;

                    // check that the fee hasnt been reduced due to a lower amount
                    if (newAmount + newFee != Data.Amount)
                    {

                        // increase amount to compensate if it has
                        newAmount += 1000000;
                    }
                    // check that 
                } while (newAmount + newFee != Data.Amount);

                // reset the amount to new fee adjusted amount
                Data.Amount = newAmount;

                // add new transfer fee to total amount
                Fee += newFee + messageFee + mosaicFee;
            }
            else
            {
                // if fee is not to be deducted, add individual fees to total fee without adjustment
                Fee += transferFee + messageFee + mosaicFee;
            }

            // write the transfer bytes
            WriteTransferBytes();

            // write message bytes if they are not null.
            if (serializedMessage != null)
                WriteMessageBytes(serializedMessage);
            else _serializer.WriteInt(ByteLength.Zero);

            // write mosaic bytes if they are not null.
            if (serializedMosaicList != null)
                WriteMosaicBytes(serializedMosaicList);
            else _serializer.WriteInt(ByteLength.Zero);

        }

        private void WriteTransferBytes()
        {
            // write address length to byte array
            _serializer.WriteInt(ByteLength.AddressLength);

            // write recipient address to byte array
            _serializer.WriteString(Data.Recipient.Address.Encoded);

            // write amount to byte array
            _serializer.WriteLong(Data.Amount);
        }      

        private Message SerializeMessagePart()
        {
            return new Message(Con, SenderPrivateKey, Data.Recipient, Data.Message, Data.Encrypted);
        }

        private void WriteMessageBytes(Message serializeMessage)
        {
            // write message bytes to byte array
            _serializer.WriteBytes(serializeMessage.GetMessageBytes());

            // store message bytes length
            MessageLength += serializeMessage.Length;
        }

        private MosaicList SerializeMosaicPart()
        {
            return new MosaicList(Data.ListOfMosaics, Con, SenderPublicKey);

        }
        private void WriteMosaicBytes(MosaicList mosaicList)
        {
            // write mosaic bytes to byte array
            _serializer.WriteBytes(mosaicList.GetMosaicListBytes());

            // store mosaic bytes length
            MosaicLength = mosaicList.GetMosaicListBytes().Length;
        }

        private void AppendMultisig()
        {
            if (Data.MultisigAccount == null) return;

            // create multisig object and serialize
            var multisig = new MultiSigTransaction(Con, SenderPublicKey, Data.Deadline, TransferMessageMosaicBytes.Length);

            // store multig bytes to be concatonated later (in verifiable account)
            TransferMessageMosaicBytes = multisig.GetBytes().ConcatonatetBytes(TransferMessageMosaicBytes);
        }
    }
}