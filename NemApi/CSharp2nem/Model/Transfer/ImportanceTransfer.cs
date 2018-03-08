using System;
using Chaos.NaCl;
using CSharp2nem.Constants;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.DataModels;
using CSharp2nem.Model.MultiSig;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.Transfer
{
    internal class ImportanceTransfer : Transaction
    {
        internal ImportanceTransfer(Connectivity.Connection con, PublicKey sender, ImportanceTransferData data) : base(con, data.MultisigAccount ?? sender, data.Deadline)
        {
            if (!StringUtils.OnlyHexInString(data.DelegatedAccount.Raw) || data.DelegatedAccount.Raw.Length != 64)
                throw new ArgumentNullException(nameof(con));
         
            TransferMode = data.Activate ? DefaultBytes.Activate : DefaultBytes.Deactivate;

            Serialize(data.DelegatedAccount);

            TransferBytes = ByteUtils.TruncateByteArray(Serializer.GetBytes(), StructureLength.ImportnaceTransfer);

            finalize();

            AppendMultisig(con, data);
        }

        private Serializer Serializer = new Serializer();
        private byte[] TransferMode { get; }
        private byte[] TransferBytes { get; }
        private byte[] ImportanceBytes { get; set; }

        internal void finalize()
        {
            UpdateFee(TransactionFee.ImportanceTransfer);

            UpdateTransactionType(TransactionType.ImportanceTransfer);

            UpdateTransactionVersion(TransactionVersion.VersionOne);

            var bytes = new byte[GetCommonTransactionBytes().Length + StructureLength.ImportnaceTransfer];

            Array.Copy(GetCommonTransactionBytes(), bytes, GetCommonTransactionBytes().Length);

            Array.Copy(TransferBytes, 0, bytes, GetCommonTransactionBytes().Length, StructureLength.ImportnaceTransfer);

            ImportanceBytes = bytes;
        }

        internal byte[] GetBytes()
        {
            return ImportanceBytes;
        }

        private void Serialize(PublicKey PublicKey)
        {
            Serializer.WriteBytes(TransferMode);

            Serializer.WriteInt(ByteLength.PublicKeyLength);

            Serializer.WriteBytes(CryptoBytes.FromHexString(PublicKey.Raw));
        }

        private void AppendMultisig(Connectivity.Connection con, ImportanceTransferData data)
        {
            if (data.MultisigAccount == null) return;

            var multisig = new MultiSigTransaction(con, data.DelegatedAccount , data.Deadline, ImportanceBytes.Length);

            ImportanceBytes = ByteUtils.ConcatonatetBytes(multisig.GetBytes(), ImportanceBytes);
        }
    }
}