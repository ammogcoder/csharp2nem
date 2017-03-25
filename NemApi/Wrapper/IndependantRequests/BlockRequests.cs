using System.Threading.Tasks;
using NemApi.Async;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class Block
    {
        public Block(Connection connection)
        {
            Connection = connection;
        }

        public Block()
        {
            Connection = new Connection();
        }

        private Connection Connection { get; }

        public async Task<BlockData.Score> ChainScore()
        {
            return await new AsyncConnector.GetAsync<BlockData.Score>(Connection).Get("chain/score");
        }

        public async Task<BlockData.Height> ChainHeight()
        {
            return await new AsyncConnector.GetAsync<BlockData.Height>(Connection).Get("/chain/height");
        }

        public async Task<BlockData.Block> ByHash(string hash)
        {
            var path = "/block/get";

            var query = string.Concat("hash=", hash);

            return await new AsyncConnector.GetAsync<BlockData.Block>(Connection).Get(path, query);
        }

        public async Task<BlockData.Block> Last()
        {
            return await new AsyncConnector.GetAsync<BlockData.Block>(Connection).Get("/chain/last-block");
        }

        public async Task<BlockData.Block> ByHeight(int height)
        {
            return await new AsyncConnector.GetAsync<BlockData.Block>(Connection).Get("/block/at/public",
                new BlockData.Height {height = height});
        }

        public async Task<BlockData.BlockList> ChainPart(int height)
        {
            return await new AsyncConnector.GetAsync<BlockData.BlockList>(Connection).Get("/local/chain/blocks-after",
                new BlockData.Height {height = height});
        }
    }
}