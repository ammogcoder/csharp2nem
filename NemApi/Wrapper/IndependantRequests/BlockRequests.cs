using System.Threading.Tasks;
using CSharp2nem.Async;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class BlockClient
    {
        /*
         * Constructs a new block client to perform requests
         * 
         * @Connection { Connection } The connection to use
         */
        public BlockClient(Connection connection)
        {
            Connection = connection;
        }

        /*
         * Constructs a new block client to perform requests
         * 
         * Uses a default connection
         */
        public BlockClient()
        {
            Connection = new Connection();
        }

        /*
         * The connection to use for this client
         */
        private Connection Connection { get; }

        /*
         * Gets the current chain score
         * 
         * http://bob.nem.ninja/docs/#block-chain-score 
         * 
         * Return: The current chain score
         */
        public async Task<BlockData.Score> ChainScore()
        {
            return await new AsyncConnector.GetAsync<BlockData.Score>(Connection).Get("chain/score");
        }

        /*
         * Gets current block height
         * 
         * http://bob.nem.ninja/docs/#block-chain-height
         * 
         * Return: The block height
         */
        public async Task<BlockData.Height> ChainHeight()
        {
            return await new AsyncConnector.GetAsync<BlockData.Height>(Connection).Get("/chain/height");
        }

        /*
         * Get the last block
         * 
         * http://bob.nem.ninja/docs/#last-block-of-the-block-chain-score
         * 
         * Return: The last block
         */
        public async Task<BlockData.Block> Last()
        {
            return await new AsyncConnector.GetAsync<BlockData.Block>(Connection).Get("/chain/last-block");
        }

        /*
         * Get the block at the given height
         * 
         * http://bob.nem.ninja/docs/#getting-a-block-with-a-given-height
         * 
         * @height { int } The height for the block
         * 
         * Return: The block at the given height
         */
        public async Task<BlockData.Block> ByHeight(int height)
        {
            return await new AsyncConnector.GetAsync<BlockData.Block>(Connection).Get("/block/at/public",
                new BlockData.Height {height = height});
        }

        /*
         * Get part of the block chain, up to 10 blocks
         * 
         * http://bob.nem.ninja/docs/#getting-part-of-a-chain
         * 
         * @height { int } The height after which the blocks should be retrieved
         *         If the database contains less than 10 blocks after the 
         *         height, the remaining blocks will be returned
         * 
         * Return: The block at the given height
         */
        public async Task<BlockData.BlockList> ChainPart(int height)
        {
            return await new AsyncConnector.GetAsync<BlockData.BlockList>(Connection).Get("/local/chain/blocks-after",
                new BlockData.Height {height = height});
        }
    }
}