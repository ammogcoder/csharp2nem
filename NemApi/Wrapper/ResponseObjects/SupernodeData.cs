using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects
{
    public class SupernodeResponseData
    {
        public class Test
        {
            public string dateAndTime { get; set; }
            public int round { get; set; }
            public int testResult { get; set; }
        }

        public class TestResults
        {
            public List<Test> data { get; set; }
        }

        public class NodeInfo
        {
            public double distance { get; set; }
            public int servantPort { get; set; }
            public string ip { get; set; }
            public double latitude { get; set; }
            public int maxUnlocked { get; set; }
            public int numUnlocked { get; set; }
            public int nisPort { get; set; }
            public string alias { get; set; }
            public int id { get; set; }
            public string payoutAddress { get; set; }
            public string pubKey { get; set; }
            public int status { get; set; }
            public double longitude { get; set; }
        }

        public class NearestNodes
        {
            public List<NodeInfo> data { get; set; }
        }

        public class Nodes
        {
            public int servantPort { get; set; }
            public string ip { get; set; }
            public double latitude { get; set; }
            public string alias { get; set; }
            public int id { get; set; }
            public string payoutAddress { get; set; }
            public int nisPort { get; set; }
            public string pubKey { get; set; }
            public int status { get; set; }
            public double longitude { get; set; }
        }

        public class Supernodes
        {
            public List<Nodes> data { get; set; }
        }
    }
}
