using System;
using System.Collections.Generic;
using System.Net.Http;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class Connection
    {
        internal HttpClient Client = new HttpClient();

        public Connection(UriBuilder uri, byte networkVersion = 0x68)
        {

            Uri = uri;
            ShouldFindNewHostIfRequestFails = true;
            NetworkVersion = networkVersion;
            SetLivenetPretrustedHostList();
        }

        public Connection()
        {
            Uri = new UriBuilder
            {
                Port = 7890
            };
            ShouldFindNewHostIfRequestFails = true;
            NetworkVersion = 0x68;
            SetLivenetPretrustedHostList();
            SetNewHost();
        }

        //internal WebClient Client = new WebClient(); // .NET 3.0 compatible
        public UriBuilder Uri { get; set; }
        private List<string> PreTrustedNodes { get; set; }
        public bool ShouldFindNewHostIfRequestFails { get; set; }
        private byte NetworkVersion { get; set; }

        public void SetTestNet()
        {
            NetworkVersion = 0x98;
            SetTestnetPretrustedHostList();
            SetNewHost();
        }

        public void SetMainnet()
        {
            NetworkVersion = 0x68;
            SetLivenetPretrustedHostList();
            SetNewHost();
        }

        public void SetMijinMainNet(List<string> nodes)
        {
            NetworkVersion = 0x60;
            PreTrustedNodes = nodes;
            SetNewHost();
        }

        public void SetMijinTestNet(List<string> nodes)
        {
            NetworkVersion = 0x60;
            PreTrustedNodes = nodes;
            SetNewHost();
        }

        public void SetCustomHostList(List<string> hosts)
        {
            PreTrustedNodes = hosts;
        }

        public void SetHost(string host)
        {
            Uri.Host = host;
        }

        public string GetHost()
        {
            return Uri.Host;
        }

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

        internal UriBuilder GetUri(string path, string query)
        {
            Uri.Path = path;
            Uri.Query = query;
            return Uri;
        }

        public void SetNewHost()
        {
            var rnd = new Random();
            var r = rnd.Next(PreTrustedNodes.Count);
            Uri.Host = PreTrustedNodes[r];
        }

        internal void SetLivenetPretrustedHostList()
        {
            PreTrustedNodes = new List<string>
            {
                "85.25.36.97",
                "85.25.36.92",
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