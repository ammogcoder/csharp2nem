using Microsoft.VisualStudio.TestTools.UnitTesting;
using NemApi;

namespace Tests.NisRequests
{
    [TestClass]
    public class NisRequestTests
    {
        [TestMethod]
        public void CanRetrieveHeartBeat()
        {
            var con = new Connection();

            var nisRequests = new Nis(con);

            var response = nisRequests.HeartBeat();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveNetworkTime()
        {
            var con = new Connection();

            var nisRequests = new Nis(con);

            var response = nisRequests.NetworkTime();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveStatus()
        {
            var con = new Connection();

            var nisRequests = new Nis(con);

            var response = nisRequests.Status();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveTimeSync()
        {
            var con = new Connection();

            var nisRequests = new Nis(con);

            var response = nisRequests.TimeSync();

            Assert.IsNotNull(response);
        }
    }
}
