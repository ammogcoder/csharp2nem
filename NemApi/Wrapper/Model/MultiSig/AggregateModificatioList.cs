using System;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    internal class AggregateModificatioList : Transaction
    {
        internal AggregateModificatioList(Connection connection, PublicKey sender, AggregateModificationData data)
            : base(connection, data.MultisigAccount ?? sender, data.Deadline) // todo : fee
        {
            Data = data;
            PublicKey = sender;
            Serializer = new Serializer();

            Serialize();
            SetFee();
            UpdateTransactionTypeAndVersion();
            CombineBytes();
            AppendMultisig(connection);
        }

        private Serializer Serializer { get; }
        private AggregateModificationData Data { get; }
        private PublicKey PublicKey { get; }
        private byte[] ModificationBytes { get; set; }
        internal int TotalBytesLength { get; set; }

        private void SetFee()
        {
            var fee = (long) (Data.RelativeChange == 0
                ? (10 + 6 * Data.Modifications.Count) * 1000000
                : (10 + 6 * Data.Modifications.Count + 6) * 1000000);

            UpdateFee(fee);
        }

        private void UpdateTransactionTypeAndVersion()
        {
            UpdateTransactionType(TransactionType.MultisigAggregateModification);
            UpdateTransactionVersion(TransactionVersion.VersionTwo);
        }

        private void CombineBytes()
        {
            var tempBytes = new byte[GetCommonTransactionBytes().Length + TotalBytesLength];

            Array.Copy(GetCommonTransactionBytes(), tempBytes, GetCommonTransactionBytes().Length);
            Array.Copy(ByteUtils.TruncateByteArray(Serializer.GetBytes(), TotalBytesLength), 0, tempBytes,
                GetCommonTransactionBytes().Length, TotalBytesLength);

            TotalBytesLength = tempBytes.Length;
            ModificationBytes = tempBytes;
        }

        internal byte[] GetBytes()
        {
            return ModificationBytes;
        }

        private void Serialize()
        {
            TotalBytesLength += 4;
            Serializer.WriteInt(Data.Modifications.Count);

            foreach (var mod in Data.Modifications)
            {
                TotalBytesLength += StructureLength.AggregateModification + ByteLength.FourBytes;
                Serializer.WriteInt(StructureLength.AggregateModification);
                Serializer.WriteInt(mod.ModificationType);
                Serializer.WriteInt(ByteLength.PublicKeyLength);
                Serializer.WriteBytes(CryptoBytes.FromHexString(mod.PublicKey.Raw));
            }
            if (Data.RelativeChange != 0)
            {
                TotalBytesLength += StructureLength.RelativeChange + ByteLength.FourBytes;
                Serializer.WriteInt(StructureLength.RelativeChange);
                Serializer.WriteInt(Data.RelativeChange);
            }
            else
            {
                TotalBytesLength += ByteLength.FourBytes;
                Serializer.WriteInt(ByteLength.Zero);
            }
        }

        private void AppendMultisig(Connection con)
        {
            var multisig = new MultiSigTransaction(con, PublicKey, Data.Deadline, TotalBytesLength);

            ModificationBytes = ByteUtils.ConcatonatatBytes(multisig.GetBytes(), ModificationBytes);
        }
    }
}