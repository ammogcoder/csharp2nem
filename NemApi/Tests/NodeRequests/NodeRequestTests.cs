using Microsoft.VisualStudio.TestTools.UnitTesting;
using NemApi;

namespace Tests.NodeRequests
{
    [TestClass]
    public class NodeRequestTests
    {
        [TestMethod]
        public void CanRetrieveActivePeerList()
        {
            var con = new Connection();

            var nodeRequests = new Node(con);

            var response = nodeRequests.ActivePeerList();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveExtendedNodeInfo()
        {
            var con = new Connection();

            var nodeRequests = new Node(con);

            var response = nodeRequests.ExtendedNodeInfo();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveNodeInfo()
        {
            var con = new Connection();

            var nodeRequests = new Node(con);

            var response = nodeRequests.Info();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveMaxHeight()
        {
            var con = new Connection();

            var nodeRequests = new Node(con);

            var response = nodeRequests.MaxHeight();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrievePeerList()
        {
            var con = new Connection();

            var nodeRequests = new Node(con);

            var response = nodeRequests.PeerList();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveReachablePeerList()
        {
            var con = new Connection();

            var nodeRequests = new Node(con);

            var response = nodeRequests.ReachablePeerList();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveSuperNodeList()
        {
            var con = new Connection();

            var nodeRequests = new Node(con);

            var response = nodeRequests.SuperNodeList();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveUnlockedInfo()
        {
            var con = new Connection();

            var nodeRequests = new Node(con);

            var response = nodeRequests.UnlockedInfo();

            Assert.IsNotNull(response);
        }
    }
}
