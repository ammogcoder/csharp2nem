using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharp2nem;

namespace Tests.BlockChainRequests
{
    [TestClass]
    public class RoundTripBlockChainRequestTests
    {
        [TestMethod]
        public void CanRetrieveLastBlock()
        {
            var con = new Connection();

            var blockRequests = new Block(con);

            var response =  blockRequests.Last().Result;
            Trace.Write(response.Height);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveBlockByHeight()
        {
            var con = new Connection();

            var blockRequests = new Block(con);

            var response =  blockRequests.ByHeight(400000).Result;
            Debug.Write(response.Height);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveChainHeight()
        {
            var con = new Connection();

            var blockRequests = new Block(con);

            var response =  blockRequests.ChainHeight().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveChainPart()
        {
            var con = new Connection();

            var blockRequests = new Block(con);

            var response =  blockRequests.ChainPart(400000).Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveChainScore()
        {
            var con = new Connection();

            var blockRequests = new Block(con);

            var response =  blockRequests.ChainScore().Result;

            Assert.IsNotNull(response);
        }
    }
}
