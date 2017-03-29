using System;
using System.Text;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    internal class MultisigSignature : Transaction
    {
        internal MultisigSignature(Connection connection, PublicKey senderPublicKey,
            MultisigSignatureTransactionData data)
            : base(connection, senderPublicKey, data.Deadline)
        {
            Data = data;
            Serialize();         
            finalize();
        }

        private readonly Serializer Serializer = new Serializer();
        internal byte[] SignatureBytes { get; set; }
        private MultisigSignatureTransactionData Data { get; }

        

        internal void finalize()
        {
            UpdateFee(TransactionFee.MultisigSignature);
            UpdateTransactionType(TransactionType.MultisigSignatureTransaction);
            UpdateTransactionVersion(TransactionVersion.VersionOne);

            SignatureBytes = new byte[GetCommonTransactionBytes().Length + StructureLength.MultisigSignature];

            Array.Copy(GetCommonTransactionBytes(), SignatureBytes, GetCommonTransactionBytes().Length);
            Array.Copy(Serializer.GetBytes().TruncateByteArray(StructureLength.MultisigSignature), 0, SignatureBytes, GetCommonTransactionBytes().Length, StructureLength.MultisigSignature);
        }

        internal byte[] GetBytes()
        {
            return SignatureBytes;
        }

        private void Serialize()
        {
            Serializer.WriteInt(0x24);

            Serializer.WriteInt(0x20);

            Serializer.WriteBytes(CryptoBytes.FromHexString(Data.TransactionHash));

            Serializer.WriteInt(0x28);

            Serializer.WriteBytes(Encoding.UTF8.GetBytes(Data.MultisigAddress.Encoded));
        }
    }
}