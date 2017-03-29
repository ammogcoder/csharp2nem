using System;
using System.Text;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    internal class SupplyChange : Transaction
    {
        private readonly Serializer _serializer = new Serializer();

        internal SupplyChange(Connection con, PublicKey sender, MosaicSupplyChangeData data)
            : base(con, data.MultisigAccount ?? sender, data.Deadline)
        {
            PublicKey = sender;
            Data = data;
            NameSpaceId = Encoding.Default.GetBytes(data.NameSpaceId);
            MosaicName = Encoding.Default.GetBytes(data.MosaicName);
            Length = 24 + NameSpaceId.Length + MosaicName.Length;

            Serialize();

            Bytes = _serializer.GetBytes().TruncateByteArray(Length);

            finalize();
            AppendMultisig(con);
        }

        private MosaicSupplyChangeData Data { get; }
        private PublicKey PublicKey { get; }
        private byte[] SupplyChangeBytes { get; set; }
        private byte[] Bytes { get; }
        private byte[] NameSpaceId { get; }
        private byte[] MosaicName { get; }
        internal int Length { get; set; }

        private void Serialize()
        {
            _serializer.WriteInt(ByteLength.EightBytes + NameSpaceId.Length + MosaicName.Length);

            _serializer.WriteInt(NameSpaceId.Length);

            _serializer.WriteBytes(NameSpaceId);

            _serializer.WriteInt(MosaicName.Length);

            _serializer.WriteBytes(MosaicName);

            _serializer.WriteInt(Data.SupplyChangeType);

            _serializer.WriteLong(Data.Delta);
        }

        private void finalize()
        {
            UpdateFee(TransactionFee.SupplyChange);
            UpdateTransactionType(TransactionType.MosiacSupplyChange);
            UpdateTransactionVersion(TransactionVersion.VersionOne);

            var tempBytes = new byte[GetCommonTransactionBytes().Length + Length];

            Array.Copy(GetCommonTransactionBytes(), tempBytes, GetCommonTransactionBytes().Length);
            Array.Copy(Bytes, 0, tempBytes, GetCommonTransactionBytes().Length, Length);

            Length = tempBytes.Length;
            SupplyChangeBytes = tempBytes;
        }

        private void AppendMultisig(Connection con)
        {
            if (Data.MultisigAccount == null) return;

            var multisig = new MultiSigTransaction(con, PublicKey, Data.Deadline, Length);

            SupplyChangeBytes = multisig.GetBytes().ConcatonatetBytes(SupplyChangeBytes);
        }

        internal byte[] GetBytes()
        {
            return SupplyChangeBytes;
        }
    }
}