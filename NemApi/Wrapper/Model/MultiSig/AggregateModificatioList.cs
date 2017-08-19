using System;
using Chaos.NaCl;
using CSharp2nem.Constants;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.DataModels;
using CSharp2nem.Model.Transfer;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.MultiSig
{
    /*
     * Prepares a list aggregate modifications and populates the base class of transaction.
     * The base class creates the common part of the transaction
     * 
     * 
     * 
     */
    internal class AggregateModificatioList : Transaction
    {
        internal AggregateModificatioList(Connectivity.Connection connection, PublicKey sender, AggregateModificationData data)
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
            UpdateFee(TransactionFee.MultisigAggMod);
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

        private void AppendMultisig(Connectivity.Connection con)
        {
            var multisig = new MultiSigTransaction(con, PublicKey, Data.Deadline, TotalBytesLength);

            ModificationBytes = ByteUtils.ConcatonatetBytes(multisig.GetBytes(), ModificationBytes);
        }
    }
}