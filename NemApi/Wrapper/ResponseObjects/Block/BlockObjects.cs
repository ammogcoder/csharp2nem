using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    /*
    * Block related data
    */

    public class BlockData
    {
        public class PrevBlockHash
        {
            public string Data { get; set; }
        }

        public class Block
        {
            public int TimeStamp { get; set; }
            public string Signature { get; set; }
            public PrevBlockHash PrevBlockHash { get; set; }
            public int Type { get; set; }
            public List<TransactionDatum> Transactions { get; set; }
            public int Version { get; set; }
            public string Signer { get; set; }

            public int Height { get; set; }
        }

        public class BlockDatum
        {
            public long Difficulty { get; set; }
            public List<ExplorerTransferViewModel> Txes { get; set; }
            public Block Block { get; set; }
            public string Hash { get; set; }
        }

        public class BlockList
        {
            public List<BlockDatum> Data { get; set; }
        }

        public class Height
        {
            public int height { get; set; }
        }

        public class Hash
        {
            public string Data { get; set; }
        }

        public class Meta
        {
            public int Id { get; set; }
            public int Height { get; set; }
            public Hash Hash { get; set; }
        }

        /*
        * Block transaction related data
        */

        public class Score
        {
            public string score { get; set; }
        }

        public class Message
        {
            public string Payload { get; set; }
            public int Type { get; set; }
        }

        public class Transaction
        {
            public int TimeStamp { get; set; }
            public long Amount { get; set; }
            public string Signature { get; set; }
            public int Fee { get; set; }
            public string Recipient { get; set; }
            public int Type { get; set; }
            public int Deadline { get; set; }
            public Message Message { get; set; }
            public int Version { get; set; }
            public string Signer { get; set; }
        }

        public class TransactionDatum
        {
            public Meta Meta { get; set; }
            public Transaction Transaction { get; set; }
        }

        public class ExplorerTransferViewModel
        {
            public TransactionDatum Transaction { get; set; }
            public string Hash { get; set; }
            public string InnerHash { get; set; }
        }
    }
}