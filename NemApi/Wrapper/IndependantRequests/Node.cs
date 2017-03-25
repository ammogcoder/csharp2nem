using System;
using System.Threading.Tasks;
using NemApi.Async;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class Node
    {
        public Node(Connection connection)
        {
            if (null == connection)
                throw new ArgumentNullException(nameof(connection));

            Connection = connection;
        }

        public Node()
        {
            Connection = new Connection();
        }

        private Connection Connection { get; }

        public async Task<NodeData> Info()
        {
            return await new AsyncConnector.GetAsync<NodeData>(Connection).Get("/node/info");
        }

        public async Task<SuperNodes.RootObject> SuperNodeList()
        {
            var uriTemp = new UriBuilder("http://199.217.113.179/nodes");

            return await new AsyncConnector.GetAsync<SuperNodes.RootObject>(new Connection(uriTemp)).Get("/nodes");
        }

        public async Task<NodeList> PeerList()
        {
            return await new AsyncConnector.GetAsync<NodeList>(Connection).Get("/node/peer-list/all");
        }

        public async Task<NodeAndNis> ExtendedNodeInfo()
        {
            return await new AsyncConnector.GetAsync<NodeAndNis>(Connection).Get("/node/extended-info");
        }

        public async Task<NodeList> ActivePeerList()
        {
            return await new AsyncConnector.GetAsync<NodeList>(Connection).Get("/node/peer-list/active");
        }

        public async Task<UnlockedInfo> UnlockedInfo()
        {
            return await new AsyncConnector.GetAsync<UnlockedInfo>(Connection).Get("account/unlocked/info");
        }

        public async Task<NodeList> ReachablePeerList()
        {
            return await new AsyncConnector.GetAsync<NodeList>(Connection).Get("/node/peer-list/reachable");
        }

        public async Task<Height> MaxHeight()
        {
            return await new AsyncConnector.GetAsync<Height>(Connection).Get("/node/active-peers/max-chain-height");
        }
    }
}