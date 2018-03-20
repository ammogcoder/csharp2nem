using System;
using System.Text;
using CSharp2nem.Connectivity;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects.Block;
using CSharp2nem.ResponseObjects.Node;


namespace CSharp2nem.RequestClients
{
    public class NodeClient
    {
        public NodeClient(Connection connection)
        {
            if (null == connection)
                throw new ArgumentNullException(nameof(connection));

            Connection = connection;

        }
        public NodeClient()
        {
            Connection = new Connection();
        }

        public Connection Connection { get; set; }
        internal ASCIIEncoding encoding = new ASCIIEncoding();
        #region Getting Node Info

        public ManualAsyncResult BeginGetNodeInfo(Action<ResponseAccessor<NodeData>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/node/info");
        }

        public ManualAsyncResult BeginGetNodeInfo()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/node/info");
        }

        public NodeData EndGetNodeInfo(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<NodeData>(result);
        }

        #endregion

        #region Getting Peer List

        public ManualAsyncResult BeginGetPeerList(Action<ResponseAccessor<NodeCollection>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/node/peer-list/all");
        }

        public ManualAsyncResult BeginGetPeerList()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/node/peer-list/all");
        }

        public NodeCollection EndGetPeerList(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<NodeCollection>(result);
        }

        #endregion

        #region Getting Extended Node Info

        public ManualAsyncResult BeginGetExtendedNodeInfo(Action<ResponseAccessor<NodeAndNis>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/node/extended-info");
        }
        public ManualAsyncResult BeginGetExtendedNodeInfo()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/node/extended-info");
        }

        public NodeAndNis EndGetExtendedNodeInfo(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<NodeAndNis>(result);
        }

        #endregion

        #region Getting Active Peer List

        public ManualAsyncResult BeginGetActivePeerList(Action<ResponseAccessor<NodeList>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/node/peer-list/active");
        }

        public ManualAsyncResult BeginGetActivePeerList()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/node/peer-list/active");
        }

        public NodeList EndGetActivePeerList(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<NodeList>(result);
        }

        #endregion

        #region Getting reachable Neighbourhood

        public ManualAsyncResult BeginGetReachablePeerList(Action<ResponseAccessor<NodeList>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/node/peer-list/reachable");
        }

        public ManualAsyncResult BeginGetReachablePeerList()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/node/peer-list/reachable");
        }

        public NodeList EndGetReachablePeerList(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<NodeList>(result);
        }

        #endregion

        #region Getting Max Chain Height

        public ManualAsyncResult BeginGetMaxHeight(Action<ResponseAccessor<BlockData.BlockHeight>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/node/active-peers/max-chain-height");

        }

        public ManualAsyncResult BeginGetMaxHeightAsync()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/node/active-peers/max-chain-height");
        }

        public BlockData.BlockHeight EndGetMaxHeightList(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<BlockData.BlockHeight>(result);
        }

        #endregion

        #region Getting Unlocked info

        public ManualAsyncResult BeginGetUnlockedInfo(Action<ResponseAccessor<UnlockedInfo>> callback)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/account/unlocked/info",
                Bytes = new ASCIIEncoding().GetBytes("{\"Headers\":[{\"Key\":\"Content-Type\",\"Value\":[\"text/plain; charset=utf-8\"]}]}")
            };       

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        public ManualAsyncResult BeginGetUnlockedInfo()
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/account/unlocked/info",
                Bytes = new ASCIIEncoding().GetBytes("{\"Headers\":[{\"Key\":\"Content-Type\",\"Value\":[\"text/plain; charset=utf-8\"]}]}")
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }

        public UnlockedInfo EndGetUnlockedInfo(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompletePostRequest<UnlockedInfo>(result);
        }

        #endregion
    }
}