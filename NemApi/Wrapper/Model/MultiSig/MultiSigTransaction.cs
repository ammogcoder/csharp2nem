using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Chaos.NaCl;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    internal class MultiSigTransaction
    {
        internal MultiSigTransaction(Connection connection, PublicKey publicKey, int deadline, int length)

        {
            if (null == connection)
                throw new ArgumentNullException(nameof(connection));
            if (null == publicKey)
                throw new ArgumentNullException(nameof(publicKey));

            InnerLength = length;

            Serializer = new Serializer();

            NetworkVersion = connection.GetNetworkVersion();

            TimeStamp = TimeDateUtils.EpochTimeInMilliSeconds();

            Deadline = deadline == 0 ? TimeStamp + 1000 : TimeStamp + deadline;

            PublicKey = publicKey;

            Fee = TransactionFee.MultisigWrapper;

            Serialize();

            MultiSigBytes = Serializer.GetBytes().TruncateByteArray(StructureLength.MultiSigHeader);

            MultiSigBytes[7] = NetworkVersion;           
        }

        private Serializer Serializer { get; }
        private PublicKey PublicKey { get; }
        private int Deadline { get; }
        private byte[] MultiSigBytes { get; }
        private byte NetworkVersion { get; }
        private int TimeStamp { get; }
        private long Fee { get; }
        private int InnerLength { get; }

        internal byte[] GetBytes()
        {
            return MultiSigBytes;
        }

        private void Serialize()
        {
            // transaction type. Set as null/empty bytes as it will be 
            // updated when serializing the different transaction types.
            Serializer.WriteInt(TransactionType.MultisigTransaction);

            // version
            Serializer.WriteInt(TransactionVersion.VersionOne);

            // timestamp
            Serializer.WriteInt(TimeStamp);

            //pubKey len
            Serializer.WriteInt(ByteLength.PublicKeyLength);

            // pub key
            var set = Split(PublicKey.Raw, 2);
            var bytes = new byte[32];
            var i = 0;

            foreach (var s in set)
            {
                var x = int.Parse(s, NumberStyles.HexNumber);
                bytes[i] = (byte) x;
                i++;
            }
            Serializer.WriteBytes(bytes);

            // fee
            Serializer.WriteLong(Fee);

            // deadline
            Serializer.WriteInt(Deadline);

            Serializer.WriteInt(InnerLength);
        }

        private static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}