using System;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    internal class ImportanceTransfer : Transaction
    {
        internal ImportanceTransfer(Connection con, PublicKey sender, ImportanceTransferData data)
            : base(con, data.MultisigAccount ?? sender, data.Deadline)
        {
            if (!data.DelegatedAccount.Raw.OnlyHexInString() || data.DelegatedAccount.Raw.Length != 64)
                throw new ArgumentNullException(nameof(con));

            Data = data;
            Serializer = new Serializer();
            PublicKey = data.DelegatedAccount;
            TransferMode = data.Activate ? DefaultBytes.Activate : DefaultBytes.Deactivate;

            Serialize();
            TransferBytes = Serializer.GetBytes().TruncateByteArray(StructureLength.ImportnaceTransfer);

            finalize();
            AppendMultisig(con);
        }

        private ImportanceTransferData Data { get; }
        private PublicKey PublicKey { get; }
        private byte[] TransferMode { get; }
        private byte[] TransferBytes { get; }
        private byte[] ImportanceBytes { get; set; }
        internal int Length { get; set; }
        private Serializer Serializer { get; }

        internal void finalize()
        {
            UpdateFee(TransactionFee.ImportanceTransfer);
            UpdateTransactionType(TransactionType.ImportanceTransfer);
            UpdateTransactionVersion(TransactionVersion.VersionOne);

            var bytes = new byte[GetCommonTransactionBytes().Length + StructureLength.ImportnaceTransfer];

            Array.Copy(GetCommonTransactionBytes(), bytes, GetCommonTransactionBytes().Length);
            Array.Copy(TransferBytes, 0, bytes, GetCommonTransactionBytes().Length, StructureLength.ImportnaceTransfer);

            Length = bytes.Length;
            ImportanceBytes = bytes;
        }

        internal byte[] GetBytes()
        {
            return ImportanceBytes;
        }

        private void Serialize()
        {
            Serializer.WriteBytes(TransferMode);

            Serializer.WriteInt(ByteLength.PublicKeyLength);

            Serializer.WriteBytes(CryptoBytes.FromHexString(PublicKey.Raw));
        }

        private void AppendMultisig(Connection con)
        {
            if (Data.MultisigAccount == null) return;

            var multisig = new MultiSigTransaction(con, PublicKey, Data.Deadline, Length);

            ImportanceBytes = multisig.GetBytes().ConcatonatetBytes(ImportanceBytes);
        }
    }
}