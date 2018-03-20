using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CSharp2nem.Model.DataModels;
using CSharp2nem.RequestClients;

namespace CSharp2nem.Connectivity
{
    /// <summary>
    /// Connection class contains a number of methods for manipulating the nodes to connect to, as well as the network version.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <remarks>
        /// Uses the IP address used when creating the UriBuilder as default with a list of pretrusted developer controlled nodes as back up.
        /// Sets the Autohost property to true. When set to true, the fasted host based on ping time will be selected from the list of hosts if the currently connected node fails to respond.
        /// </remarks>
        /// <param name="uri">The URI. A custom URI to use to create a connection. Specify the host and port to connect to.</param>
        /// <param name="networkVersion">The network version. Default main net. Use 0x98 for testnet.</param>
        /// <example> 
        /// This sample shows how to create an instance of the <see cref="Connection"/> class.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {
        ///         UriBuilder Uri = new UriBuilder();
        ///         Uri.Host = "host ip address here";
        ///         Uri.Port = "7890";
        ///         
        ///         Connection connection = new Connection(Uri, 0x98);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Connection(UriBuilder uri, byte networkVersion = 0x68)
        {
            
            AutoHost = false;

            Uri = uri;    

            NetworkVersion = networkVersion;

            MaxRequests = 5;

            RequestTimeout = 200;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <remarks>
        ///  Defaults to port 7890. Defaults Autohost to true. When AutoHost is set to true, the next fasted node is selected to re-perform the request. Defaults to main net. Sets the node pool to a list of pretrusted developer controlled nodes. Defaults to main net.
        /// </remarks>
        /// <example> 
        /// This sample shows how to create an instance of the <see cref="Connection"/> class.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///     }
        /// }
        /// </code>
        /// </example>
        public Connection()
        {
            Uri = new UriBuilder
            {
                Port = 7890
            };

            AutoHost = true;

            NetworkVersion = 0x68;

            SetLivenetPretrustedHostList();

            MaxRequests = 5;

            RequestTimeout = 200;

        }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public UriBuilder Uri { get; set; }
        
        /// <summary>
        /// Gets or sets the maximum failed requests for a given request before an exception is thrown
        /// </summary>
        public int MaxRequests { get; set; }
        /// <summary>
        /// Gets or sets the timeout length for requests performed using a given connection
        /// </summary>
        /// <remarks>
        /// Nodes that do not meet a ping time of at least this value will be rejected as a connection when setting the fastest node. Default value: 200
        /// </remarks>
        public int RequestTimeout { get; set; }

        private List<string> PreTrustedNodes { get; set; }
        private byte NetworkVersion { get; set; }
        private static bool Connected { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the connection should automatically find the next fastest node should a request fail.
        /// </summary>
        /// <remarks>
        /// An exception will be thrown if 5 nodes fail to accept the request in a row. In most scenarios this should indicate a lack of internet connectivity, or that every node in the node pool is currently down.
        /// </remarks>
        /// <value>
        ///   <c>true</c> if [automatic host]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoHost { get; set; }

        internal bool CheckForInternetConnection()
        {
           
            try
            {
                var myPing = new Ping();
                var host = "google.com";
                var buffer = new byte[32];
                var timeout = 1000;
                var pingOptions = new PingOptions();
                var reply = myPing.Send(host, timeout, buffer, pingOptions);
                return reply?.Status == IPStatus.Success;
                
            }
            catch
            {    
                return false;
            }
        }

        /// <summary>
        /// Pings the host.
        /// </summary>
        /// <param name="nameOrAddress">The name or address to ping.</param>
        /// <param name="elapsed">The time in Timer.Ticks it took to ping the node.</param>
        /// <returns>true if node is pingable; Otherwise false. out value returns the Timer.Ticks taken to ping the host; Otherwise 10000000000000</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="PingHost"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        /// 
        ///         long ellapsed = 0;
        /// 
        ///         connection.PingHost(connection.GetHost(), ellapsed);
        ///     }
        /// }
        /// </code>
        /// </example>


        public bool PingHost(string nameOrAddress, out long elapsed)
        {

            var pinger = new Ping();

            try
            {
                var sw = new Stopwatch();

                sw.Start();

                var reply = pinger.Send(nameOrAddress, RequestTimeout);

                sw.Stop();

                var pingable = reply?.Status == IPStatus.Success;

                if (pingable)
                {
                    var con = new Connection();

                    con.SetHost(nameOrAddress);

                    con.AutoHost = false;

                    var client = new NisClient(con);

                    client.BeginGetStatus(ar =>
                    {
                        if (ar?.Content?.Code != 6)
                        {
                            pingable = false;
                        }
                    });
                }

                elapsed = pingable ? sw.Elapsed.Ticks : 10000000000000;

                return pingable;
            }
            catch (PingException pe)
            {

                elapsed = 10000000000000;

                return false;
            }
        }
        /// <summary>
        /// Delegate used for running synchronous methods asynchronously. 
        /// </summary>
        /// <remarks>
        /// Primarily used  internally to execute the SetFasterNode method.
        /// </remarks>
        public delegate void AsyncMethodCaller();


        /// <summary>
        /// Sets the connection to use the fastest host in the pool of nodes based on ping time.  
        /// </summary>
        /// <remarks>
        ///  This function is time consuming if the node pool is large. Consider running asynchronously using the <see cref="AsyncMethodCaller"/> delegate.
        /// </remarks>
        /// <example> 
        /// This sample shows how to use the <see cref="SetFastestHost"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        /// 
        ///         connection.SetFasterHost();
        ///         
        ///         // or to run asynchronously
        ///         
        ///         Connection connection = new Connection();
        /// 
        ///         AsyncMethodCaller caller = connection.SetFastestHost;
        ///         
        ///         IAsyncResult result = caller.BeginInvoke(null, null);
        ///     }
        /// }
        /// </code>
        /// </example>
        public async Task SetFastestHost()
        {

            if (!CheckForInternetConnection())
                throw new Exception("no connectivity");
            
            var fastestHost = "";
            
            var minPingTime = 10000000000000;
            
            foreach (var host in PreTrustedNodes)
            {
                long hostPingTime;
            
                var pingable = PingHost(host, out hostPingTime);
            
                if (!pingable || hostPingTime >= minPingTime) continue;
            
                fastestHost = host;
            
                minPingTime = hostPingTime;
            }
            
            if (fastestHost != "") SetHost(fastestHost);
            else throw new Exception("no connectivity");
        }

        public void SetNearestNode(SupernodeStructures.Location location)
        {
            var client = new SupernodeClient();

            if(location.numNodes > 25)
                throw new ArgumentException("invalid number of nodes");

            client.BeginGetNearestNodesInfo(ar =>
            {
                for (var i = 0; i < ar.Content.data.Count; i++)
                {
                    if (ar.Content.data[i].status != 1) continue;
                    SetHost(ar.Content.data[i].ip);
                    break;
                }                            
            }, location).AsyncWaitHandle.WaitOne();        
        }

        /// <summary>
        /// Sets the connection to use test net.
        /// </summary>
        /// <remarks>
        /// Use the AsyncWaitHandle.WaitOne extension method to wait for the fastest node to be selected.
        /// </remarks>
        /// <param name="autoHost">Indicates whether the fasted node in the list should be used.</param>
        /// <param name="runAsync">Indicates whether the fastest node should be selected synchronously or asynchronously</param>
        /// <returns>
        /// IAsyncResult, the result of running SetFastedNode Asynchronously
        /// </returns>
        /// <example> 
        /// This sample shows how to use the <see cref="SetTestnet"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        /// 
        ///         IAsyncResult result = connection.SetTestnet();
        ///         
        ///         result.AsyncWaitHandle.WaitOne(); // To prevent a race before the connection is used.
        ///     }
        /// }
        /// </code>
        /// </example>
        public void SetTestnet(bool autoHost = true, bool runAsync = true)
        {
            NetworkVersion = 0x98;

            SetTestnetPretrustedHostList();

            if (!autoHost) return;

            if (!runAsync)
            {
                SetFastestHost().RunSynchronously();
            }
            else SetFastestHost();
        }

        /// <summary>
        /// Sets the connection to use main net. Sets the node pool to a list of trusted developer noes.
        /// </summary>
        /// <remarks>
        /// Use the AsyncWaitHandle.WaitOne extension method to wait for the fastest node to be selected.
        /// </remarks>
        /// <param name="autoHost">Indicates whether the fasted node in the list should be used.</param>
        /// <returns>
        /// IAsyncResult, the result of running SetFastedNode Asynchronously
        /// </returns>
        /// <example> 
        /// This sample shows how to use the <see cref="SetMainnet"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        /// 
        ///         IAsyncResult result = connection.SetMainnet();
        ///         
        ///         result.AsyncWaitHandle.WaitOne(); // To prevent a race before the connection is used.
        ///     }
        /// }
        /// </code>
        /// </example>
        public void SetMainnet(bool autoHost = true, bool runAsync = true)
        {
            NetworkVersion = 0x68;

            SetLivenetPretrustedHostList();

            if (!autoHost) return;

            if (!runAsync)
            {
                SetFastestHost().RunSynchronously();
            }
            else SetFastestHost();
        }

        
        private void SetMijinMainNet(List<string> nodes, bool autoHost = true)
        {
            NetworkVersion = 0x60;

            PreTrustedNodes = nodes;
        }

        
        private void SetMijinTestNet(List<string> nodes, bool autoHost = true)
        {
            NetworkVersion = 0x60;

            PreTrustedNodes = nodes;
        }

        /// <summary>
        /// Sets the custom host list to a given list of hosts.
        /// </summary>
        /// <remarks>
        /// Replaces the Pre-Trusted-Node list. Automatically selects the fasted node from the given list to connect to based on ping time.
        /// </remarks>
        /// <param name="hosts">The host pool to use for connections.</param>
        /// <param name="autoHost">Indicates whether the fasted node in the list should be used.</param>
        /// <returns>IAsyncResult, returned from running the SetFastestNode method asyncronously if autoHost parameter is set to true; Otherwise returns null</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="SetCustomHostList"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        /// 
        ///         IAsyncResult result = connection.SetCustomHostList(new List&lt;string&gt;(){
        ///                   "1.0.0.0",
        ///                   "1.0.0.0"
        ///               });
        ///         
        ///         result.AsyncWaitHandle.WaitOne(); // To prevent a race before the connection is used if .
        ///     }
        /// }
        /// </code>
        /// </example>
        public void SetCustomHostList(List<string> hosts, bool autoHost = true, bool runAsync = true)
        {
            PreTrustedNodes = hosts;

            if (!autoHost) return;

            if (!runAsync)
            {
                SetFastestHost().RunSynchronously();
            }
            else SetFastestHost();
        }

        /// <summary>
        /// Sets this connection to use the provided host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <example> 
        /// This sample shows how to use the <see cref="SetHost"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        /// 
        ///         connection.SetHost("1.0.0.0");
        ///     }
        /// }
        /// </code>
        /// </example>
        public void SetHost(string host)
        {
            Uri.Host = host;
        }

        /// <summary>
        /// Gets the host the connection is currently using.
        /// </summary>
        /// <returns>The host IP address</returns>
        public string GetHost()
        {
            return Uri.Host;
        }

        /// <summary>
        /// Gets the network version byte.
        /// </summary>
        /// <remarks>
        /// 0x98 denotes testnet. 0x68 denotes mainnet. Additional info: Testnet addresses start with 'T'. Mainnet addresses start with 'N'
        /// </remarks>
        /// <returns>The byte code for the network version</returns>
        public byte GetNetworkVersion()
        {
            return NetworkVersion;
        }
   
        internal UriBuilder GetUri()
        {
            return Uri;
        }
        
        internal UriBuilder GetUri(string path)
        {
            Uri.Path = path;

            Uri.Query = null;

            return Uri;
        }

        /// <summary>
        /// Gets the URI used to make requests.
        /// </summary>
        /// <remarks>
        /// To build your own API strings, provide the path and query for the api. The host and port will be inferred from the connection that initiates this call.
        /// </remarks>
        /// <param name="path">The path.</param>
        /// <param name="query">The query.</param>
        /// <returns>The URI object containing host, port, path and query.</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="GetNetworkVersion"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        /// 
        ///         UriBuilder uriBuilder = connection.GetUri("/account/get","address=TBOBBSXX7BESJXDWGLP5Z7FM5HSTKUH5WIMPW562");
        ///     }
        /// }
        /// </code>
        /// </example>
        public UriBuilder GetUri(string path, string query)
        {
            Uri.Path = path;

            Uri.Query = query;

            return Uri;
        }

        /// <summary>
        /// Sets the node pool to use main net pretrusted developer nodes.
        /// </summary>
        /// <remarks>
        /// The nodes provided have normally have the highest rate of uptime.
        /// </remarks>        
        public void SetLivenetPretrustedHostList()
        {
            PreTrustedNodes = new List<string>
            {
               "108.61.168.86",             
               "108.61.182.27",            
               "104.238.161.61",
               "62.75.163.236",
               "209.126.98.204",
               "104.238.161.61",
               "jusan.nem.ninja",
               "nijuichi.nem.ninja"
            };

            SetHost(PreTrustedNodes[0]);
        }

        public void SetNewHost()
        {            
            foreach (var host in PreTrustedNodes)
            {
                int index = PreTrustedNodes.IndexOf(GetHost());
                
                if (index + 1 < PreTrustedNodes.Count)   
                {
                    if (PreTrustedNodes[index + 1] != null)
                    {
                        SetHost(PreTrustedNodes[index + 1]);
                        break;
                    }   
                }
                
                SetHost(PreTrustedNodes[0]);
            }

           
        }
        /// <summary>
        /// Sets the node pool to use test net pretrusted developer nodes.
        /// </summary>
        /// <remarks>
        /// The nodes provided have normally have the highest rate of uptime.
        /// </remarks>       
        public void SetTestnetPretrustedHostList()
        {
            PreTrustedNodes = new List<string>
            {
                
                "23.228.67.85",
                "13.70.154.110",
                "150.95.145.157",
                "37.120.188.83"

            };

           SetHost(PreTrustedNodes[0]);
        }
    }
}