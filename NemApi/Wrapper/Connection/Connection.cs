using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * The connection to use
     * 
     */
    public class Connection
    {
        internal HttpClient Client = new HttpClient();

        /*
         * Construct the Connection object
         * 
         * @Url The URL to use 
         *       {
         *          Host,
         *          Port
         *       }
         * 
         * @networkVerion The Network version to use
         *       {
         *          Mainnet: 0x68
         *          Testnet: 0x98
         *          Mijin:   0x60
         *       }
         */
        public Connection(UriBuilder uri, byte networkVersion = 0x68)
        {

            Uri = uri;
           
            NetworkVersion = networkVersion;
            SetLivenetPretrustedHostList();
        }

        /*
         * Create connection with default values
         * 
         * Host: trusted mainnet dev nodes
         * Port: 7890, the default nis port
         * Should find new host if connection fails
         */
        public Connection()
        {
            Uri = new UriBuilder
            {
                Port = 7890
            };

            NetworkVersion = 0x68;

            SetLivenetPretrustedHostList();

            SetNewHost();
        }

        //internal WebClient Client = new WebClient(); // .NET 3.0 compatible
        public UriBuilder Uri { get; set; }
        private List<string> PreTrustedNodes { get; set; }    
        private byte NetworkVersion { get; set; }
        public bool ShouldFindNewHostIfRequestFails = true;
        /*
         * Set connection to use a testnet node
         * 
         */
        public void SetTestNet()
        {
            NetworkVersion = 0x98;

            SetTestnetPretrustedHostList();

            SetNewHost();
        }

        /*
         * Set the connection to use main net nodes
         * 
         */
        public void SetMainnet()
        {
            NetworkVersion = 0x68;

            SetLivenetPretrustedHostList();

            SetNewHost();
        }

        /*
         * Set the connection to use mijin main net nodes
         * 
         * @nodes The nodes to connect to
         */
        public void SetMijinMainNet(List<string> nodes)
        {
            NetworkVersion = 0x60;

            PreTrustedNodes = nodes;

            SetNewHost();
        }

        /*
         * Set the connection to use mijin testnet nodes
         * 
         * @nodes The nodes to connect to
         */ 
        public void SetMijinTestNet(List<string> nodes)
        {
            NetworkVersion = 0x60;

            PreTrustedNodes = nodes;

            SetNewHost();
        }

        /*
         * Set the connection to use a custom set of nodes
         * Replaces the trusted dev nodes so that connection 
         * doesnt need to be specified.
         * 
         * @hosts The hosts to connect to
         */
        public void SetCustomHostList(List<string> hosts)
        {
            PreTrustedNodes = hosts;
        }

        /*
         * Set the connection to a specific host
         * 
         * @host The host to connect to
         */
        public void SetHost(string host)
        {
            Uri.Host = host;
        }

        /*
         * Get the host currenctly connected to in this connection
         * 
         * Return: The network host for the current connection
         */
        public string GetHost()
        {
            return Uri.Host;
        }

        /*
         * Get the current network version for this connection
         * 
         * Return: The network version for the current connection
         */
        public byte GetNetworkVersion()
        {
            return NetworkVersion;
        }

        /*
         * Get the fully qualified Uri of the currect connection
         * 
         * Return: The Uri for the current connection
         */
        internal UriBuilder GetUri()
        {
            return Uri;
        }

        /*
         * Get the fully qualified Uri for this connection 
         * with a specified path
         * 
         * @path The path to include in the Uri
         * 
         * Return: The fully qualified Uri
         */
        internal UriBuilder GetUri(string path)
        {
            Uri.Path = path;

            Uri.Query = null;

            return Uri;
        }

        /*
         * Get the fully qualified Uri for this connection
         * with a specified path
         * 
         * @path The path to include in the Uri
         * @query The query to include in the Uri
         * 
         * Return: The fully qualified Uri
         */
        internal UriBuilder GetUri(string path, string query)
        {
            Uri.Path = path;
            Uri.Query = query;
            return Uri;
        }

        /*
         * Sets a new host for the current connection
         * from the list of pre trusted nodes
         * 
         */
        public void SetNewHost()
        {
            var rnd = new Random();
            var r = rnd.Next(PreTrustedNodes.Count);
            Uri.Host = PreTrustedNodes[r];
        }

        /*
         * Set the pretrusted nodes to the default 
         * dev nodes
         * 
         */
        internal void SetLivenetPretrustedHostList()
        {
            PreTrustedNodes = new List<string>
            {
                "85.25.36.97",             
                "199.217.112.135",
                "108.61.182.27",
                "108.61.168.86",
                "104.238.161.61",
                "62.75.171.41",
                "san.nem.ninja",
                "go.nem.ninja",
                "hachi.nem.ninja"
            };
        }

        /*
         * Set the connection to use the pre trusted 
         * testnet dev nodes
         * 
         */
        internal void SetTestnetPretrustedHostList()
        {
            PreTrustedNodes = new List<string>
            {
                "bob.nem.ninja",
                "104.128.226.60"
               
            };
        }
    }
}