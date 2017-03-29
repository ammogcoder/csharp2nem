using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharp2nem;
namespace Tests.Account
{
    [TestClass]
    public class UnverifiableAccountRequests
    {
        [TestMethod]
        public void CanSignatoryRetrieveAccountInfo()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetAccountInfoAsync().Result;
          
            Assert.IsNotNull(response.Account.Address);         
            Assert.IsNotNull(response.Account.Balance);
            Assert.IsNotNull(response.Account.HarvestedBlocks);
            Assert.IsNotNull(response.Account.Importance);
            Assert.IsNull(response.Account.Label);
            Assert.IsNotNull(response.Account.PublicKey);            
            Assert.IsNotNull(response.Meta.Status);          
            Assert.IsNotNull(response.Meta.CosignatoryOf);           
            Assert.IsNotNull(response.Meta.CosignatoryOf);           
            Assert.IsNull(response.Meta.Cosignatories);

            Trace.WriteLine(response.Account.Address);
            Trace.WriteLine(response.Account.Balance);
            Trace.WriteLine(response.Account.HarvestedBlocks);
            Trace.WriteLine(response.Account.Importance);
            Trace.WriteLine(response.Account.Label == null);
            Trace.WriteLine(response.Account.PublicKey);
            Trace.WriteLine(response.Meta.Status);
            Trace.WriteLine(response.Meta.CosignatoryOf);
            Trace.WriteLine(response.Meta.CosignatoryOf);

        }

        [TestMethod]
        public void CanRetrieveAllTransactions()
        {
            var con = new Connection();
            con.SetTestNet();
            var key = new PublicKey(TestConstants.PubKey);
            var account = new AccountFactory(con).FromPublicKey(key);

            var response = account.GetAllTransactionsAsync().Result;
            Assert.IsNotNull(response.data[0].transaction.amount);
            Assert.IsNotNull(response.data[0].transaction.deadline);
            Assert.IsNotNull(response.data[0].transaction.fee);
            Assert.IsNotNull(response.data[0].transaction.recipient);
            Assert.IsNotNull(response.data[0].transaction.signature);
            Assert.IsNotNull(response.data[0].transaction.signer);
            Assert.IsNotNull(response.data[0].transaction.timeStamp);
            Assert.IsNotNull(response.data[0].transaction.type);
            Assert.IsNotNull(response.data[0].transaction.version);



            foreach (var t in response.data)
            {
                
                if (t.transaction.message != null)
                {
                    Assert.IsNotNull(response.data[0].transaction.message.payload);
                    Assert.IsNotNull(response.data[0].transaction.message.type);
                }
                if (t.transaction.otherTrans != null)
                {
                    Assert.IsNotNull(response.data[0].transaction.message.payload);
                    Assert.IsNotNull(response.data[0].transaction.message.type);
                }
            }
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
