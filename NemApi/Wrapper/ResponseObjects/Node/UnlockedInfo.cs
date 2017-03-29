using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class BootNodeMetaData
    {
        public string Application { get; set; }
    }

    public class BootNodeEndpoint
    {
        public string Protocol { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
    }

    public class BootNodeIdentity
    {
        public string PrivateKey { get; set; }
        public string Name { get; set; }
    }

    public class BootNodeRootObject
    {
        public BootNodeMetaData MetaData { get; set; }
        public BootNodeEndpoint Endpoint { get; set; }
        public BootNodeIdentity Identity { get; set; }
    }

    public class UnlockedInfo
    {
        [JsonProperty("num-unlocked")]
        public int NumUnlocked { get; set; }

        [JsonProperty("max-unlocked")]
        public int MaxUnlocked { get; set; }
    }

    public class Height
    {
        public int height { get; set; }
    }

    public class MetaData
    {
        public string Application { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
    }

    public class Endpoint
    {
        public string Protocol { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
    }

    public class Identity
    {
        public string Name { get; set; }

        [JsonProperty("private-key")]
        public string PrivateKey { get; set; }
    }

    public class NodeList
    {
        public List<NodeData> Data { get; set; }
    }

    public class NodeData
    {
        public MetaData MetaData { get; set; }
        public Endpoint Endpoint { get; set; }
        public Identity Identity { get; set; }
    }

    public class NisInfo
    {
        public int CurrentTime { get; set; }
        public string Application { get; set; }
        public int StartTime { get; set; }
        public string Version { get; set; }
        public string Signer { get; set; }
    }

    public class NodeAndNis
    {
        public NodeData Node { get; set; }
        public NisInfo NisInfo { get; set; }
    }
}