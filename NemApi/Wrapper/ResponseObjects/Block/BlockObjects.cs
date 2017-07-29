using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Block
{
    /*
    * Block related data
    */

    /// <summary>
    /// Contains a number of classes that contain block related properties.
    /// </summary>
    public class BlockData
    {
        /// <summary>
        /// The previous block hash
        /// </summary>
        public class PrevBlockHash
        {
            /// <summary>
            /// The block hash
            /// </summary>
            public string Data { get; set; }
        }

        /// <summary>
        /// Contains a number of properties for a block
        /// </summary>
        public class Block
        {
            /// <summary>
            /// The block time stamp in seconds since the first block.
            /// </summary>
            public int TimeStamp { get; set; }
            /// <summary>
            /// The signature of the block. All blocks in the chain are signed by the harvesters.
            /// </summary>
            /// <remarks>
            /// This way any node can check if the block has been altered by some evil entity.
            /// </remarks>
            public string Signature { get; set; }
            /// <summary>
            /// The previous block hash
            /// </summary>
            public PrevBlockHash PrevBlockHash { get; set; }
            /// <summary>
            /// The block type. 
            /// </summary>
            /// <remarks>
            /// There are currently two block types used:
            /// 
            /// -1: Nemesis block type. This block type appears only once in the chain.
            ///  1: Regular block type.All blocks with height > 1 have this type.
            /// </remarks>
            public int Type { get; set; }
            /// <summary>
            /// The array of transactions. 
            /// </summary>
            /// <remarks>
            /// See <see cref="TransactionDatum"/> for more details. A block can contain up to 120 transactions.
            /// </remarks>
            public List<TransactionDatum> Transactions { get; set; }
            /// <summary>
            /// The block version. The following versions are supported:
            /// </summary>
            /// <remarks>
            /// 0x68 &lt;&lt; 24 + 1 (1744830465 as 4 byte integer): the main network version
            /// 0x98 &lt;&lt; 24 + 1 (-1744830463 as 4 byte integer): the test network version
            /// </remarks>
            public int Version { get; set; }
            /// <summary>
            /// The public key of the harvester of the block as hexadecimal string.
            /// </summary>
            public string Signer { get; set; }

            /// <summary>
            /// The height of the block. Each block has a unique height. Subsequent blocks differ in height by 1.
            /// </summary>
            public int Height { get; set; }
        }

        /// <summary>
        /// The following structure is used by the NEM block chain explorer for convenience reason. The data is similar but not identical to that of a <see cref="Block"/>.
        /// </summary>
        public class ExplorerBlockViewModel
        {
            /// <summary>
            /// The block difficulty
            /// </summary>
            public long Difficulty { get; set; }
            /// <summary>
            /// List containing the transactions of the block. See <see cref="ExplorerTransferViewModel"/> for more details.
            /// </summary>
            public List<ExplorerTransferViewModel> Txes { get; set; }
            /// <summary>
            /// The <see cref="BlockData.Block"/> object.
            /// </summary>
            public Block Block { get; set; }
            /// <summary>
            /// The hash of the block as hexadecimal string.
            /// </summary>
            public string Hash { get; set; }
        }

        /// <summary>
        /// List of <see cref="ExplorerBlockViewModel"/> objects
        /// </summary>
        public class BlockList
        {
            /// <summary>
            /// List of <see cref="ExplorerBlockViewModel"/> objects
            /// </summary>
            public List<ExplorerBlockViewModel> Data { get; set; }
        }

        /// <summary>
        /// Block height object.
        /// </summary>
        /// <remarks>
        /// Typically used when sending the block height duing a request to get a block at a given height.
        /// </remarks>
        public class BlockHeight
        {
            /// <summary>
            /// The height of the block.
            /// </summary>
            public int height { get; set; }
        }

        /// <summary>
        /// The block hash
        /// </summary>
        public class Hash
        {
            /// <summary>
            /// The hash of the block as a hexidecimal string
            /// </summary>
            public string Data { get; set; }
        }

        /// <summary>
        /// Block meta data
        /// </summary>
        public class Meta
        {
            /// <summary>
            /// The database ID for the block
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// The height of the block
            /// </summary>
            public int Height { get; set; }
            /// <summary>
            /// The hash of the block
            /// </summary>
            public Hash Hash { get; set; }
        }

        /*
        * Block transaction related data
        */

        /// <summary>
        /// The block score
        /// </summary>
        public class BlockScore
        {
            /// <summary>
            /// The score for the block.
            /// </summary>
            /// <remarks>
            /// The score is an integer greater or equal to zero. It is submitted in hexadecimal format.
            /// </remarks>
            public string Score { get; set; }
        }

        /// <summary>
        /// The message attached to a transaction in a block
        /// </summary>
        public class Message
        {
            /// <summary>
            /// The payload of the message
            /// </summary>
            public string Payload { get; set; }
            /// <summary>
            /// The type of message
            /// </summary>
            /// <remarks>
            /// 1: plain test; 2: encrypted.
            /// </remarks>
            public int Type { get; set; }
        }

        /// <summary>
        /// The transaction data for a transaction in the block
        /// </summary>
        public class Transaction
        {
            /// <summary>
            /// The time stamp for the transaction
            /// </summary>
            public int TimeStamp { get; set; }
            /// <summary>
            /// The quantity of XEM transacted
            /// </summary>
            public long Amount { get; set; }
            /// <summary>
            /// The signature of the transaction in hexidecimal form
            /// </summary>
            public string Signature { get; set; }
            /// <summary>
            /// The total fee paid for this transaction
            /// </summary>
            public int Fee { get; set; }
            /// <summary>
            /// The transaction recipient address
            /// </summary>
            public string Recipient { get; set; }
            /// <summary>
            /// The type of transaction
            /// </summary>
            /// <remarks>
            /// See the transaction type section under the common transaction section of the documentation for further details. 
            /// http://bob.nem.ninja/docs/#gathering-data-for-the-signature 
            /// </remarks>
            public int Type { get; set; }
            /// <summary>
            /// The deadline by which the transaction should be included before it is refused by the network
            /// </summary>
            public int Deadline { get; set; }
            /// <summary>
            /// The message attachment. See <see cref="Message"/>
            /// </summary>
            public Message Message { get; set; }
            /// <summary>
            /// The transaction version.
            /// </summary>
            /// <remarks>
            /// Can be version 1 or 2. 1 is the old transaction version that does not support mosaic transfers.
            /// </remarks>
            public int Version { get; set; }
            /// <summary>
            /// The public key for the account that signed the transaction
            /// </summary>
            public string Signer { get; set; }
        }

        /// <summary>
        /// The transacton data
        /// </summary>
        public class TransactionDatum
        {
            /// <summary>
            /// The meta data for the transaction. See <see cref="Meta"/>
            /// </summary>
            public Meta Meta { get; set; }
            /// <summary>
            /// The transaction data. See <see cref="Transaction"/>
            /// </summary>
            public Transaction Transaction { get; set; }
        }

        /// <summary>
        /// Contains information about a transaction
        /// </summary>
        public class ExplorerTransferViewModel
        {
            /// <summary>
            /// The transaction data. See <see cref="TransactionDatum"/>
            /// </summary>
            public TransactionDatum Transaction { get; set; }
            /// <summary>
            /// The transaction hash
            /// </summary>
            public string Hash { get; set; }
            /// <summary>
            /// The inner hash if applicable. 
            /// </summary>
            /// <remarks>
            /// Applicable when the transaction is a multisignature transaction
            /// </remarks>
            public string InnerHash { get; set; }
        }
    }
}