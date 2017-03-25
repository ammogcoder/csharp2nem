using Microsoft.VisualStudio.TestTools.UnitTesting;
using NemApi;
namespace Tests.Account
{
    [TestClass]
    public class UnverifiableAccountRequests
    {
        [TestMethod]
        public void CanRetrieveAccountInfo()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetAccountInfoAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveAllTransactions()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetAllTransactionsAsync().Result;
            Assert.IsTrue(response.data.Count > 0);
        }

        [TestMethod]
        public void CanRetrieveUnconfirmedTransactions()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetUnconfirmedTransactionsAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveOutGoingTransactions()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetOutgoingTransactionsAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveIncomingTransactions()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetIncomingTransactionsAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveAccountStatus()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetAccountStatusAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveDelegatedAccountRoot()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetDelegatedAccountRootAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveHarvestingInfo()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetHarvestingInfoAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveMosaicDefinition()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetMosaicsAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveMosaicOwned()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetMosaicsOwnedAsync().Result;

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveNameSpaceMosaicDefinition()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetMosaicsByNameSpaceAsync("jabo38").Result;
            
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void CanRetrieveNameSpaces()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetNamespacesAsync().Result;

            Assert.IsNotNull(response);
        }
    }
}
