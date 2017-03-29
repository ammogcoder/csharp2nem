using System;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    internal class CreateMosaic : Transaction
    {
        private readonly Serializer _serializer = new Serializer();

        internal CreateMosaic(Connection connection, PublicKey signer, MosaicDefinition data)
            : base(connection, data.Model.MultisigAccount ?? signer, data.Model.Deadline)
        {
            Con = connection;
            Signer = signer;
            MosaicDefinition = data;

            Levy = data.Model.MosaicLevy;

            Serialize();

            Bytes = _serializer.GetBytes().TruncateByteArray(Length);

            finalize();
            AppendMultisig();
        }

        private PublicKey Signer { get; }
        private MosaicLevy Levy { get; }
        private Connection Con { get; }
        private MosaicDefinition MosaicDefinition { get; }
        internal int Length { get; set; }
        private byte[] Bytes { get; }
        private byte[] MosaicCreationBytes { get; set; }

        private void finalize()
        {
            UpdateFee(TransactionFee.MosaicDefinitionCreation);
            UpdateTransactionType(TransactionType.MosaicDefinitionCreation);
            UpdateTransactionVersion(TransactionVersion.VersionOne);

            var tempBytes = new byte[GetCommonTransactionBytes().Length + Length];

            Array.Copy(GetCommonTransactionBytes(), tempBytes, GetCommonTransactionBytes().Length);
            Array.Copy(Bytes, 0, tempBytes, GetCommonTransactionBytes().Length, Length);

            Length = tempBytes.Length;
            MosaicCreationBytes = tempBytes;
        }

        private void Serialize()
        {
            // all mosaic definition bytes inclusive of properties
            _serializer.WriteBytes(MosaicDefinition.GetBytes());

            Length += MosaicDefinition.Length + 56;

            if (Levy != null)
            {
                // all levy bytes if not null
                _serializer.WriteBytes(Levy.GetBytes());
                Length += Levy.Length - 4;
            }
            else
            {
                _serializer.WriteInt(DefaultValues.ZeroValuePlaceHolder);
            }

            // fee sink address length
            _serializer.WriteInt(ByteLength.AddressLength);

            // fee sink address
            _serializer.WriteString(AddressEncoding.ToEncoded(Con.GetNetworkVersion(),
                new PublicKey(DefaultValues.MainNetCreationFeeSink)));

            // creation fee
            _serializer.WriteLong(Fee.Creation);
        }

        private void AppendMultisig()
        {
            if (MosaicDefinition.Model.MultisigAccount == null) return;

            var multisig = new MultiSigTransaction(Con, Signer, MosaicDefinition.Model.Deadline, Length);

            MosaicCreationBytes = multisig.GetBytes().ConcatonatetBytes(MosaicCreationBytes);
        }

        internal byte[] GetBytes()
        {
            return MosaicCreationBytes;
        }
    }
}