using System;
using CSharp2nem.Constants;
using CSharp2nem.CryptographicFunctions;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.MultiSig;
using CSharp2nem.Model.Transfer;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.Mosaics
{
    internal class CreateMosaic : Transaction
    {
        // the serializer used to serialze the mosaic
        private readonly Serializer _serializer = new Serializer();

        internal CreateMosaic(Connectivity.Connection connection, PublicKey signer, MosaicDefinition data) : base(connection, data.Model.MultisigAccount ?? signer, data.Model.Deadline)
        {
            // set connection
            Con = connection;

            // set signer of the transaction
            Signer = signer;

            // set mosaic definition data
            MosaicDefinition = data;

            // set the mosaic levy (sometimes null)
            Levy = data.Model.MosaicLevy;

            // serialize the data
            Serialize();

            // truncate bytes
            Bytes = ByteUtils.TruncateByteArray(_serializer.GetBytes(), Length);

            // update fee, transaction type, transaction version and concatonate all the parts of the transaction data bytes
            finalize();

            // add multisig headers if transaction is multisig and concatonate bytes
            AppendMultisig();
        }

        // signer of the transaction
        private PublicKey Signer { get; }

        // the mosaic levy
        private MosaicLevy Levy { get; }

        // the connection to use
        private Connectivity.Connection Con { get; }

        // the mosaic definition to create
        private MosaicDefinition MosaicDefinition { get; }

        // the length of total bytes not inclusive of the common transaction part
        internal int Length { get; set; }

        // the transaction bytes not inclusive of the common transaction part.
        private byte[] Bytes { get; }

        // the mosaic creation specific bytes
        private byte[] MosaicCreationBytes { get; set; }

        private void finalize()
        {
            // update the fee in the common transaction part
            UpdateFee(TransactionFee.MosaicDefinitionCreation);

            // update the transaction type in the common transaction part
            UpdateTransactionType(TransactionType.MosaicDefinitionCreation);

            // update the transaction version
            UpdateTransactionVersion(TransactionVersion.VersionOne);

            // create an empty array to hold the common + transaction part.
            var tempBytes = new byte[GetCommonTransactionBytes().Length + Length];

            // copy common part to array
            Array.Copy(GetCommonTransactionBytes(), tempBytes, GetCommonTransactionBytes().Length);

            // copy transaction part to array
            Array.Copy(Bytes, 0, tempBytes, GetCommonTransactionBytes().Length, Length);

            // set the length
            Length = tempBytes.Length;

            // set the mosaic create bytes
            MosaicCreationBytes = tempBytes;
        }

        private void Serialize()
        {
            // all mosaic definition bytes inclusive of properties
            _serializer.WriteBytes(MosaicDefinition.GetBytes());

            // add mosaic definition part length plus length of transaction part
            Length += MosaicDefinition.Length + 56;

            if (Levy != null)
            {
                // all levy bytes if not null
                _serializer.WriteBytes(Levy.GetBytes());

                // add levy structure length minus 4 bytes that would otherwise have been a place holder in lue of a levy
                Length += Levy.Length - 4;
            }
            else
            {
                // if no levy, serialize 0 placeholder
                _serializer.WriteInt(DefaultValues.ZeroValuePlaceHolder);
            }

            // fee sink address length
            _serializer.WriteInt(ByteLength.AddressLength);

            // fee sink address
            _serializer.WriteString(AddressEncoding.ToEncoded(Con.GetNetworkVersion(), new PublicKey(DefaultValues.MainNetCreationFeeSink)));

            // creation fee
            _serializer.WriteLong(Fee.Creation);
        }

        private void AppendMultisig()
        {
            // if transaction isnt from a multisig account, return.
            if (MosaicDefinition.Model.MultisigAccount == null) return;

            // create multisig transaction header
            var multisig = new MultiSigTransaction(Con, Signer, MosaicDefinition.Model.Deadline, Length);

            // concatenate multisig bytes
            MosaicCreationBytes = ByteUtils.ConcatonatetBytes(multisig.GetBytes(), MosaicCreationBytes);
        }

        internal byte[] GetBytes()
        {
            return MosaicCreationBytes;
        }
    }
}