using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharp2nem.ResponseObjects.Node
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

    public class Endpoint
    {
        /// <summary>
        /// The protocol used for the communication (HTTP or HTTPS).
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// The port used for the communication.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The IP address of the endpoint.
        /// </summary>
        public string Host { get; set; }
    }
    public class Endpoint2
    {
        /// <summary>
        /// The protocol used for the communication (HTTP or HTTPS).
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// The port used for the communication.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The IP address of the endpoint.
        /// </summary>
        public string Host { get; set; }
    }
    public class Endpoint3
    {
        /// <summary>
        /// The protocol used for the communication (HTTP or HTTPS).
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// The port used for the communication.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The IP address of the endpoint.
        /// </summary>
        public string Host { get; set; }
    }
    public class Endpoint4
    {
        /// <summary>
        /// The protocol used for the communication (HTTP or HTTPS).
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// The port used for the communication.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The IP address of the endpoint.
        /// </summary>
        public string Host { get; set; }
    }

    public class NodeList
    {
        public List<NodeData> Data { get; set; }
    }

    public class NodeData
    {
        /// <summary>
        /// Denotes the beginning of the meta data substructure.
        /// </summary>
        public MetaData metaData { get; set; }

        /// <summary>
        /// Denotes the beginning of the endpoint substructure.
        /// </summary>
        public Endpoint endpoint { get; set; }

        /// <summary>
        /// Denotes the beginning of the identity substructure.
        /// </summary>
        public Identity identity { get; set; }
    }

    /// <summary>
    /// The application meta data object supplies additional information about the application running on a node.
    /// </summary>
    public class ApplicationMetaData
    {
        /// <summary>
        /// The current network time, i.e. the number of seconds that have elapsed since the creation of the nemesis block.
        /// </summary>
        public int CurrentTime { get; set; }
        /// <summary>
        /// The name of the application running on the node.
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// The network time when the application was started.
        /// </summary>
        public int StartTime { get; set; }
        /// <summary>
        /// The application version.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// The signer of the certificate used by the application.
        /// </summary>
        public string Signer { get; set; }
    }

    public class NodeAndNis
    {
        public NodeData Node { get; set; }
        public ApplicationMetaData NisInfo { get; set; }
    }

    
    
    public class Identity
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The public key used to identify the node.
        /// </summary>
        [JsonProperty("public-key")]
        public string publickey { get; set; }
    }

    public class Identity2
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The public key used to identify the node.
        /// </summary>
        [JsonProperty("public-key")]
        public string publickey { get; set; }
    }

    public class Identity3
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The public key used to identify the node.
        /// </summary>
        [JsonProperty("public-key")]
        public string publickey { get; set; }
    }

    public class Identity4
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The public key used to identify the node.
        /// </summary>
        [JsonProperty("public-key")]
        public string publickey { get; set; }
    }


    public class Inactive
    {
        /// <summary>
        /// Denotes the beginning of the meta data substructure.
        /// </summary>
        public MetaData metaData { get; set; }

        /// <summary>
        /// Denotes the beginning of the endpoint substructure.
        /// </summary>
        public Endpoint endpoint { get; set; }

        /// <summary>
        /// Denotes the beginning of the identity substructure.
        /// </summary>
        public Identity identity { get; set; }
    }

    public class Active
    {
        /// <summary>
        /// Denotes the beginning of the meta data substructure.
        /// </summary>
        public MetaData2 metaData { get; set; }
        /// <summary>
        /// Denotes the beginning of the endpoint substructure.
        /// </summary>
        public Endpoint2 endpoint { get; set; }
        /// <summary>
        /// Denotes the beginning of the identity substructure.
        /// </summary>
        public Identity2 identity { get; set; }
    }

    public class Failure
    {
        /// <summary>
        /// Denotes the beginning of the meta data substructure.
        /// </summary>
        public MetaData3 metaData { get; set; }
        /// <summary>
        /// Denotes the beginning of the endpoint substructure.
        /// </summary>
        public Endpoint3 endpoint { get; set; }
        /// <summary>
        /// Denotes the beginning of the identity substructure.
        /// </summary>
        public Identity3 identity { get; set; }
    }

    public class Busy
    {
        /// <summary>
        /// Denotes the beginning of the meta data substructure.
        /// </summary>
        public MetaData4 metaData { get; set; }

        /// <summary>
        /// Denotes the beginning of the endpoint substructure.
        /// </summary>
        public Endpoint4 endpoint { get; set; }

        /// <summary>
        /// Denotes the beginning of the identity substructure.
        /// </summary>
        public Identity4 identity { get; set; }
    }

    public class MetaData
    {
        /// <summary>
        /// The number of features the nodes has.
        /// </summary>
        public int features { get; set; }
        /// <summary>
        /// The name of the application that is running the node.
        /// </summary>
        public object application { get; set; }
        /// <summary>
        /// The network id. The following network ids are supported:
        /// </summary>
        /// <remarks>
        /// 104 (hex 0x68): The main network id.
        /// 152 (hex 0x98): The test network id.
        /// </remarks>
        public int networkId { get; set; }
        /// <summary>
        /// The version of the application.
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// The underlying platform (OS, java version).
        /// </summary>
        public string platform { get; set; }
    }

    public class MetaData2
    {
        /// <summary>
        /// The number of features the nodes has.
        /// </summary>
        public int features { get; set; }
        /// <summary>
        /// The name of the application that is running the node.
        /// </summary>
        public object application { get; set; }
        /// <summary>
        /// The network id. The following network ids are supported:
        /// </summary>
        /// <remarks>
        /// 104 (hex 0x68): The main network id.
        /// 152 (hex 0x98): The test network id.
        /// </remarks>
        public int networkId { get; set; }
        /// <summary>
        /// The version of the application.
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// The underlying platform (OS, java version).
        /// </summary>
        public string platform { get; set; }
    }

    public class MetaData3
    {
        /// <summary>
        /// The number of features the nodes has.
        /// </summary>
        public int features { get; set; }
        /// <summary>
        /// The name of the application that is running the node.
        /// </summary>
        public object application { get; set; }
        /// <summary>
        /// The network id. The following network ids are supported:
        /// </summary>
        /// <remarks>
        /// 104 (hex 0x68): The main network id.
        /// 152 (hex 0x98): The test network id.
        /// </remarks>
        public int networkId { get; set; }
        /// <summary>
        /// The version of the application.
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// The underlying platform (OS, java version).
        /// </summary>
        public string platform { get; set; }
    }

    public class MetaData4
    {
        /// <summary>
        /// The number of features the nodes has.
        /// </summary>
        public int features { get; set; }
        /// <summary>
        /// The name of the application that is running the node.
        /// </summary>
        public object application { get; set; }
        /// <summary>
        /// The network id. The following network ids are supported:
        /// </summary>
        /// <remarks>
        /// 104 (hex 0x68): The main network id.
        /// 152 (hex 0x98): The test network id.
        /// </remarks>
        public int networkId { get; set; }
        /// <summary>
        /// The version of the application.
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// The underlying platform (OS, java version).
        /// </summary>
        public string platform { get; set; }
    }

   
    

    public class NodeCollection
    {
        /// <summary>
        /// A connection to the node cannot be established.
        /// </summary>
        public List<Inactive> inactive { get; set; }
        /// <summary>
        /// A fatal error occurs when trying to establish a connection or the node couldn't authenticate itself correctly.
        /// </summary>
        public List<Failure> failure { get; set; }
        /// <summary>
        /// A connection can be established but the node cannot provide information within the timeout limits.
        /// </summary>
        public List<Busy> busy { get; set; }
        /// <summary>
        /// A connection can be established and the remote node responds in a timely manner.
        /// </summary>
        public List<Active> active { get; set; }
    }
}