using System;
using System.Threading.Tasks;
using CSharp2nem.Async;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
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

        public Connection Connection { get; set; }

        public async Task<NodeData> Info()
        {
            return await new AsyncConnector.GetAsync<NodeData>(Connection).Get("/node/info");
        }

        public async Task<SuperNodes.NodeList> SuperNodeList()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var uriTemp = new UriBuilder("http://199.217.113.179");
            
            return await new AsyncConnector.GetAsync<SuperNodes.NodeList>(new Connection(uriTemp)).Get("/nodes");
        }

        public async Task<SuperNodes.SuperNodeTestDetails> SuperNodeResultById(string id)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var uriTemp = new UriBuilder("https://supernodes.nem.io");

            return await new AsyncConnector.GetAsync<SuperNodes.SuperNodeTestDetails>(new Connection(uriTemp)).Get("resultInfo/" + id);

        }
        public async Task<SuperNodes.Node> SuperNodeByIp(string ip)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var uriTemp = new UriBuilder("http://199.217.113.179");

            var result = await new AsyncConnector.GetAsync<SuperNodes.NodeList>(new Connection(uriTemp)).Get("/nodes");

            var n = new SuperNodes.Node();
            
            foreach (var node in result.Nodes)
            {
                n = node;
                if (node.Ip != ip) continue;
                n.TestResults = await SuperNodeResultById(node.Id);
                break;
            }

            return n;
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
            return await new AsyncConnector.PostAsync(Connection).Post("account/unlocked/info");
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