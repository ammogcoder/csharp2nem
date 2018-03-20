using System;
using System.Text;
using CSharp2nem.Connectivity;
using CSharp2nem.Model.DataModels;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects;
using Newtonsoft.Json;

namespace CSharp2nem.RequestClients
{


    public class SupernodeClient
    {
        public SupernodeClient()
        {
            Connection = new Connection(new UriBuilder
            {
                Host = "199.217.113.179",
                Port = 7782
            }) {AutoHost = false};

        }
      
        public Connection Connection { get; set; }

        internal ASCIIEncoding encoding = new ASCIIEncoding();

        public ManualAsyncResult BeginGetNearestNodesInfo(Action<ResponseAccessor<SupernodeResponseData.NearestNodes>> callback, SupernodeStructures.Location location)
        {
            if (location.numNodes > 25)
                throw new ArgumentException("Number of nodes must be 25 or less");

            var asyncResult = new ManualAsyncResult
            {
                Path = "/nodes/nearest",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(location))
            };
           
            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        public ManualAsyncResult BeginGetNearestNodesInfo(SupernodeStructures.Location location)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/nodes/nearest",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(location))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }


        public SupernodeResponseData.NearestNodes EndGetNearestNodes(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<SupernodeResponseData.NearestNodes>(result);
        }

        public ManualAsyncResult BeginGetSupernodes(Action<ResponseAccessor<SupernodeResponseData.Supernodes>> callback, int status)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/nodes",
                Bytes = encoding.GetBytes("{\"status\": " + status + "}")
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        public ManualAsyncResult BeginGetSupernodes(int status)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/nodes",
                Bytes = encoding.GetBytes("{\"status\": " + status + "}")
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }


        public SupernodeResponseData.Supernodes EndGetSupernodes(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<SupernodeResponseData.Supernodes>(result);
        }

        public ManualAsyncResult BeginGetTestResults(Action<ResponseAccessor<SupernodeResponseData.TestResults>> callback, SupernodeStructures.TestResultRequestData tests)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/node/testResults",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(tests))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        public ManualAsyncResult BeginGetTestResults(SupernodeStructures.TestResultRequestData tests)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/node/testResults",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(tests))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }


        public SupernodeResponseData.TestResults EndGetTestResults(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<SupernodeResponseData.TestResults>(result);
        }
    }
}
