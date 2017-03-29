using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.NetworkInformation;
using CSharp2nem;

namespace Tests
{
    [TestClass]
    public class ConnectionTest
    {
        [TestMethod]
        public void CanReachAutoConnectedHost()
        {
            var con = new Connection();

            Assert.IsTrue(PingHost(con.GetHost()));
        }

        public static bool PingHost(string nameOrAddress)
        {
            var pingable = false;
            var pinger = new Ping();
            try
            {
                var reply = pinger.Send(nameOrAddress);
                if (reply != null) pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                pingable = false;
            }
            return pingable;
        }

    }
}
