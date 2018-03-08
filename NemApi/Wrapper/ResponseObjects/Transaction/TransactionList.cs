using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Transaction
{
    public class Transactions
    {
        public class InnerHash
        {
            public string data { get; set; }
        }
        public class Hash
        {
            public string data { get; set; }
        }
        public class Meta
        {
            public string data { get; set; }
            public InnerHash innerHash { get; set; }
            public int id { get; set; }
            public Hash hash { get; set; }
            public long height { get; set; }
        }

        public class OtherHash
        {
            public string data { get; set; }
        }

        public class Signature
        {
            public int timeStamp { get; set; }
            public OtherHash otherHash { get; set; }
            public string otherAccount { get; set; }
            public string signature { get; set; }
            public long fee { get; set; }
            public int type { get; set; }
            public long deadline { get; set; }
            public int version { get; set; }
            public string signer { get; set; }
        }

        public class Message
        {
            public string payload { get; set; }
            public int type { get; set; }
        }

        public class Message2
        {
            public string payload { get; set; }
            public int type { get; set; }
        }

        public class MosaicId
        {
            public string namespaceId { get; set; }
            public string name { get; set; }
        }

        public class Mosaic
        {
            public long quantity { get; set; }
            public MosaicId mosaicId { get; set; }
        }

        public class MinCosignatories
        {
            public int relativeChange { get; set; }
        }

        public class Modification
        {
            public int modificationType { get; set; }
            public string cosignatoryAccount { get; set; }
        }
        

        public class Property
        {
            public string name { get; set; }
            public string value { get; set; }
        }
        public class Levy
        {
            public int type { get; set; }
            public string recipient { get; set; }
            public MosaicId mosaicId { get; set; }
            public long fee { get; set; }
        }
        public class MosaicDefinition
        {
            public string creator { get; set; }
            public string description { get; set; }
            public MosaicId id { get; set; }
            public List<Property> properties { get; set; }
            public Levy levy { get; set; }
        }

        public class Transaction
        {
            public int timeStamp { get; set; }
            public string signature { get; set; }
            public long fee { get; set; }
            public int type { get; set; }
            public long deadline { get; set; }
            public int version { get; set; }
            public List<Signature> signatures { get; set; }
            public string signer { get; set; }
            public OtherTrans otherTrans { get; set; }
            public long amount { get; set; }
            public string recipient { get; set; }
            public Message2 message { get; set; }
            public List<Mosaic> mosaics { get; set; }
            public MinCosignatories minCosignatories { get; set; }
            public List<Modification> modifications { get; set; }
        }

        public class OtherTrans
        {
            public long creationFee { get; set; }
            public string creationFeeSink { get; set; }
            public long timeStamp { get; set; }
            public long amount { get; set; }
            public long fee { get; set; }
            public string recipient { get; set; }
            public int type { get; set; }
            public long deadline { get; set; }
            public Message message { get; set; }
            public int version { get; set; }
            public string signer { get; set; }
            public List<Mosaic> mosaics { get; set; }
            public MosaicDefinition mosaicDefinition { get; set; }
        }

        public class TransactionData
        {
            public Meta meta { get; set; }
            public Transaction transaction { get; set; }
        }

        public class All
        {
            public List<TransactionData> data { get; set; }
        }
    }
}