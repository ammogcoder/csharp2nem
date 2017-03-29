using System;
using System.Text;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    internal class ProvisionNamespace : Transaction
    {
        private readonly Serializer _serializer = new Serializer();

        internal ProvisionNamespace(Connection connection, PublicKey sender, ProvisionNameSpaceData data)
            : base(connection, data.MultisigAccount ?? sender, data.Deadline)
        {
            SenderPublicKey = sender;
            Con = connection;
            Data = data;

            if (Data.Parent == null) Data.Parent = DefaultValues.EmptyString;

            LengthOfNewPart = Encoding.Default.GetBytes(Data.NewPart).Length;
            LengthOfParent = Encoding.Default.GetBytes(Data.Parent).Length;

            Serialize();
            finalize();
            AppendMultisig();
        }

        private ProvisionNameSpaceData Data { get; }
        private Connection Con { get; }
        private PublicKey SenderPublicKey { get; }
        private int LengthOfNewPart { get; }
        private int LengthOfParent { get; }
        private byte[] ProvisionNamespaceBytes { get; set; }
        internal int TotalBytesLength { get; set; }

        private void Serialize()
        {
            TotalBytesLength += StructureLength.ProvisionNameSpace + LengthOfNewPart;
            _serializer.WriteInt(ByteLength.AddressLength);
            _serializer.WriteString(AddressEncoding.ToEncoded(Con.GetNetworkVersion(),
                new PublicKey(DefaultValues.MainNetRentalFeeSinkPublicKey)));

            _serializer.WriteLong(Data.Parent == DefaultValues.EmptyString ? Fee.Rental : Fee.SubSpaceRental);
            _serializer.WriteInt(LengthOfNewPart);
            _serializer.WriteBytes(Encoding.Default.GetBytes(Data.NewPart));

            if (string.Empty != Data.Parent)
            {
                TotalBytesLength += ByteLength.FourBytes + LengthOfParent;
                _serializer.WriteInt(LengthOfParent);
                _serializer.WriteBytes(Encoding.Default.GetBytes(Data.Parent));
            }
            else
            {
                TotalBytesLength += ByteLength.FourBytes;
                _serializer.WriteBytes(DefaultBytes.MaxByteValue);
            }

            ProvisionNamespaceBytes = _serializer.GetBytes().TruncateByteArray(TotalBytesLength);
        }

        private void finalize()
        {
            UpdateFee(TransactionFee.ProvisionNameSpace);
            UpdateTransactionType(TransactionType.ProvisionNamespace);
            UpdateTransactionVersion(TransactionVersion.VersionOne);

            var tempBytes = new byte[GetCommonTransactionBytes().Length + TotalBytesLength];

            Array.Copy(GetCommonTransactionBytes(), tempBytes, GetCommonTransactionBytes().Length);
            Array.Copy(ProvisionNamespaceBytes, 0, tempBytes, GetCommonTransactionBytes().Length, TotalBytesLength);

            TotalBytesLength = tempBytes.Length;
            ProvisionNamespaceBytes = tempBytes;


        }

        private void AppendMultisig()
        {
            if (Data.MultisigAccount == null) return;

            var multisig = new MultiSigTransaction(Con, SenderPublicKey, Data.Deadline, TotalBytesLength);

            ProvisionNamespaceBytes = multisig.GetBytes().ConcatonatetBytes(ProvisionNamespaceBytes);
        }

        internal byte[] GetBytes()
        {
            return ProvisionNamespaceBytes;
        }
    }
}