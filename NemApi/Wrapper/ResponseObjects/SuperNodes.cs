using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace NemApi
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
        }

        public class RootObject
        {
            public List<Node> Nodes { get; set; }
        }
    }
}