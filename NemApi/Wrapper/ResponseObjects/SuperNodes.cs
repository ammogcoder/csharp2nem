using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects
{
    public class SuperNodes
    {
        public class Node
        {
            public string Id { get; set; }
            public string Alias { get; set; }
            public string Ip { get; set; }
            public int NisPort { get; set; }
            public string PubKey { get; set; }
            public int ServantPort { get; set; }
            public int Status { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string PayoutAddress { get; set; }
            public SuperNodeTestDetails TestResults { get; set; }
        }

        public class NodeDetail
        {
            public string id { get; set; }
            public string dateAndTime { get; set; }
            public string nodeId { get; set; }
            public bool nodeVersionTestOk { get; set; }
            public bool chainHeightTestOk { get; set; }
            public bool chainPartTestOk { get; set; }
            public bool responsivenessTestOk { get; set; }
            public bool bandwidthTestOk { get; set; }
            public bool computingPowerTestOk { get; set; }
            public bool pingTestOk { get; set; }
            public int round { get; set; }
            public bool nodeBalanceTestOk { get; set; }
        }

        public class SuperNodeTestDetails
        {
            public string nodeAlias { get; set; }
            public List<NodeDetail> nodeDetails { get; set; }
        }

        public class NodeList
        {
            public List<Node> Nodes { get; set; }
        }
    }
}