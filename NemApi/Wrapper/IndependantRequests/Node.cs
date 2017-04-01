using System;
using System.Threading.Tasks;
using CSharp2nem.Async;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class NodeClient
    {
        /*
         * Constructs the NodeClient
         * 
         * @connection { Connection } The connection to use
         * 
         */
        public NodeClient(Connection connection)
        {
            if (null == connection)
                throw new ArgumentNullException(nameof(connection));

            Connection = connection;
        }

        /*
         * Constructs the NodeClient
         * 
         * Uses the default connection
         * 
         */
        public NodeClient()
        {
            Connection = new Connection();
        }

        public Connection Connection { get; set; }

        /*
         * Get info about a node
         * 
         * http://bob.nem.ninja/docs/#basic-node-information
         * 
         * Return: { NodeData } Basic information about the node
         * 
         */
        public async Task<NodeData> Info()
        {
            return await new AsyncConnector.GetAsync<NodeData>(Connection).Get("/node/info");
        }

        /*
         * Get a list of all supernodes
         * 
         * No documentation, links to https://supernodes.nem.io/
         * 
         * Return: { SuperNode.NodeList } A list of all supernodes
         * 
         */
        public async Task<SuperNodes.NodeList> SuperNodeList()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var uriTemp = new UriBuilder("http://199.217.113.179");
            
            return await new AsyncConnector.GetAsync<SuperNodes.NodeList>(new Connection(uriTemp)).Get("/nodes");
        }

        /*
         * Get the last round of tests for a given supernode
         * 
         * No documentation, links to https://supernodes.nem.io/
         * 
         * Return: { SuperNodes.SuperNodeTestDetails } The test details for the last round of tests
         * 
         */
        public async Task<SuperNodes.SuperNodeTestDetails> SuperNodeResultById(string id)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var uriTemp = new UriBuilder("https://supernodes.nem.io");

            return await new AsyncConnector.GetAsync<SuperNodes.SuperNodeTestDetails>(new Connection(uriTemp)).Get("resultInfo/" + id);

        }

        /*
         * Get the supernode at a specific IP
         * 
         * No documentation, links to https://supernodes.nem.io/
         * 
         * Return: { SuperNode.Node } The supernode at the given IP
         * 
         */
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

        /*
         * Gets an array of all known nodes in the neighborhood.
         * 
         * http://bob.nem.ninja/docs/#complete-neighborhood
         * 
         */
        public async Task<NodeList> PeerList()
        {
            return await new AsyncConnector.GetAsync<NodeList>(Connection).Get("/node/peer-list/all");
        }

        /*
         * Gets extended information about a node. Using IP 127.0.0.1 gets extended information about the local node.
         * 
         * http://bob.nem.ninja/docs/#extended-node-information
         * 
         * 
         */
        public async Task<NodeAndNis> ExtendedNodeInfo()
        {
            return await new AsyncConnector.GetAsync<NodeAndNis>(Connection).Get("/node/extended-info");
        }

        /*
         * Gets an array of active nodes in the neighborhood that are selected for broadcasts.
         * 
         * http://bob.nem.ninja/docs/#active-neighborhood
         * 
         */
        public async Task<NodeList> ActivePeerList()
        {
            return await new AsyncConnector.GetAsync<NodeList>(Connection).Get("/node/peer-list/active");
        }

        /*
         * Gets an array of all nodes with status 'active' in the neighborhood.
         * 
         * http://bob.nem.ninja/docs/#reachable-neighborhood
         * 
         */
        public async Task<NodeList> ReachablePeerList()
        {
            return await new AsyncConnector.GetAsync<NodeList>(Connection).Get("/node/peer-list/reachable");
        }

        /*
         * Get the unlock info about the maximum number of allowed harvesters and how many harvesters are already using the node.
         * 
         * http://bob.nem.ninja/docs/#retrieving-the-unlock-info
         * 
         */
        public async Task<UnlockedInfo> UnlockedInfo()
        {
            return await new AsyncConnector.PostAsync(Connection).Post("account/unlocked/info");
        }

        /*
         * Get the chain height from every node in the active node list (described in Active neighborhood) and returns the maximum height seen.
         * 
         * http://bob.nem.ninja/docs/#maximum-chain-height-in-the-active-neighborhood
         * 
         */
        public async Task<Height> MaxHeight()
        {
            return await new AsyncConnector.GetAsync<Height>(Connection).Get("/node/active-peers/max-chain-height");
        }
    }
}