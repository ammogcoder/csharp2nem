using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using NemApi;

/*
/ Note: when running these tests, run on testnet and 
/ credit the accounts in TestConstants with some XEM.
/ 
/ The second time they are run, you may need to uncomment
/ the commented tests and comment out the test above them
/ and vice versa for the third test. its the easiest way to
/ fully test that all transaction types dinifitely work.
*/

namespace Tests.Account
{
    [TestClass]
    public class VerifiableAccountRequestComponents
    {
        [TestMethod]
        public void CanSendTransaction()
        {

            var con = new Connection();
            con.SetTestNet();

            var key = new PrivateKey(TestConstants.PrivKey);

            var account = new AccountFactory(con).FromPrivateKey(key);
            var recip = new AccountFactory(con).FromPublicKey(TestConstants.PubKey);

            var data = new TransferTransactionData
            {
                Recipient = recip,
                Amount = 55000000000
            };

            var response = account.SendTransactionAsync(data).Result;
            Trace.WriteLine(response.Message);
            Trace.WriteLine(response.Code);
            Assert.AreEqual(1, response.Code);
        }
        
        [TestMethod]
        public void CanSendMultiSigTransaction()
        {
            var con = new Connection();
            var key = new PrivateKey(TestConstants.PrivKey);
            
            var account = new AccountFactory(con).FromPrivateKey(key);
            var account2 = new AccountFactory(con).FromEncodedAddress(TestConstants.Address);
            var recip = new AccountFactory(con).FromPublicKey(TestConstants.PubKey);

            var data = new TransferTransactionData
            {
                MultisigAccount = account2.PublicKey,
                Recipient = recip,
                Amount = 100
            };

            var response = account.SendTransactionAsync(data).Result;
            Trace.WriteLine(response.Message);
            Trace.WriteLine(response.Code);
            Assert.AreEqual(1, response.Code);
        }
        
        [TestMethod]
        public void CanModifiyMultisigAccount_add()
        {
            var con = new Connection();
            var key = new PrivateKey(TestConstants.PrivKey);

            var account = new AccountFactory(con).FromPrivateKey(key);
            
            var data = new AggregateModificationData
            {
                Modifications = new List<AggregateModification>
                {
                    new AggregateModification(TestConstants.ListOfPublicKeys[0], 2),
                },    
                RelativeChange = -1
            };
            
            var response = account.AggregateMultisigModificationAsync(data).Result;
            Trace.WriteLine(response.Message);
            Trace.WriteLine(response.Code);
            Assert.AreEqual(1, response.Code);
        }

        
        [TestMethod]
        public void CanModifiyMultisigAccount_remove()
        {
            var con = new Connection();
            var key = new PrivateKey("c83ce30fcb5b81a51ba58ff827ccbc0142d61c13e2ed39e78e876605da16d8d7"); // one of the cosignatories

            var account = new AccountFactory(con).FromPrivateKey(key);
            

            var data = new AggregateModificationData
            {
                Modifications = new List<AggregateModification>
                {
                    new AggregateModification(TestConstants.ListOfPublicKeys[0], 2),
                    new AggregateModification(TestConstants.ListOfPublicKeys[1], 2),
                    new AggregateModification(TestConstants.ListOfPublicKeys[2], 2),
                }
            };


            var response = account.AggregateMultisigModificationAsync(data).Result;
            Trace.WriteLine(response.Message);
            Trace.WriteLine(response.Code);
            Assert.AreEqual(1, response.Code);
        }
        

        [TestMethod]
        public void CanProvisionNameSpace()
        {
            var con = new Connection();
            var key = new PrivateKey(TestConstants.PrivKey);

            var account = new AccountFactory(con).FromPrivateKey(key);

            var data = new ProvisionNameSpaceData
            {
                Parent = "abcdeaaaa",
                NewPart = "abcdeaaaaa"
            };

            var response = account.ProvisionNamespaceAsync(data).Result;
            Trace.WriteLine(response.Message);
            Trace.WriteLine(response.Code);
            Assert.AreEqual(1, response.Code);
        }

        [TestMethod]
        public void CanTransferImportance()
        {
            var con = new Connection();
            var key = new PrivateKey(TestConstants.PrivKey);

            var account = new AccountFactory(con).FromPrivateKey(key);

            var data = new ImportanceTransferData
            {
                Activate = true,
                DelegatedAccount = TestConstants.ListOfPublicKeys[2],

            };

            var response = account.ImportanceTransferAsync(data).Result;
            Trace.WriteLine(response.Message);
            Trace.WriteLine(response.Code);
            Assert.AreEqual(1, response.Code);
        }
        
        [TestMethod]
        public void CanCancleImportanceTransfer()
        {
            var con = new Connection();
            var key = new PrivateKey(TestConstants.PrivKey);

            var account = new AccountFactory(con).FromPrivateKey(key);
           
            var data = new ImportanceTransferData
            {
                Activate = false,
                DelegatedAccount = TestConstants.ListOfPublicKeys[2]
            };

            var response = account.ImportanceTransferAsync(data).Result;
            Trace.WriteLine(response.Message);
            Trace.WriteLine(response.Code);
            Assert.AreEqual(1, response.Code);
        }       
    }
}
