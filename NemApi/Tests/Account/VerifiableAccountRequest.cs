using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrapperRewrite;

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
    public class VerifiableAccountRequest
    {
        [TestMethod]
        public void CanSendTransaction()
        {
            Connection con = new Connection();
            PrivateKey key = new PrivateKey(TestConstants.privKey);

            VerifiableAccount account = new AccountFactory(con).FromPrivateKey(key);
            EncodedAddress recipient = new EncodedAddress(TestConstants.address);

            var response = account.SendTransaction(recipient, 100).Result;
            
            Assert.AreEqual(response.code, 1);
        }

        [TestMethod]
        public void CanSendMultiSigTransaction()
        {
            Connection con = new Connection();
            PrivateKey key = new PrivateKey(TestConstants.privKey);

            VerifiableAccount account = new AccountFactory(con).FromPrivateKey(key);
            EncodedAddress recipient = new EncodedAddress(TestConstants.address);
            
            var response = account.SendTransaction(recipient, recipient, 100).Result;

            Assert.AreEqual(response.code, 1);
        }

        [TestMethod]
        public void CanModifiyMultisigAccount_add()
        {
            Connection con = new Connection();
            PrivateKey key = new PrivateKey(TestConstants.privKey);

            VerifiableAccount account = new AccountFactory(con).FromPrivateKey(key);
            EncodedAddress recipient = new EncodedAddress(TestConstants.address);

                List<AggregateModification> modList = new List<AggregateModification>()
                {
                    new AggregateModification(TestConstants.listOfPublicKeys[0], 1),
                    new AggregateModification(TestConstants.listOfPublicKeys[1], 1),
                    new AggregateModification(TestConstants.listOfPublicKeys[2], 1),
                };

            var response = account.AggregateMultisigModification(modList).Result;

            Assert.AreEqual(response.code, 1);
        }

        /*
        [TestMethod]
        public void CanModifiyMultisigAccount_remove()
        {
            Connection con = new Connection();
            PrivateKey key = new PrivateKey("c83ce30fcb5b81a51ba58ff827ccbc0142d61c13e2ed39e78e876605da16d8d7"); // one of the cosignatories

            VerifiableAccount account = new AccountFactory(con).FromPrivateKey(key);
            EncodedAddress recipient = new EncodedAddress(TestConstants.address);

            List<AggregateModification> modList = new List<AggregateModification>()
                {
                    new AggregateModification(TestConstants.listOfPublicKeys[0], 2),
                    new AggregateModification(TestConstants.listOfPublicKeys[1], 2),
                    new AggregateModification(TestConstants.listOfPublicKeys[2], 2),
                };

            var response = account.AggregateMultisigModification(modList).Result;

            Assert.AreEqual(response.code, 1);
        }
        */

        [TestMethod]
        public void CanProvisionNameSpace()
        {
            Connection con = new Connection();
            PrivateKey key = new PrivateKey(TestConstants.privKey);

            VerifiableAccount account = new AccountFactory(con).FromPrivateKey(key);
            EncodedAddress recipient = new EncodedAddress(TestConstants.address);

            var response = account.ProvisionNamespace("newnamespace").Result;

            Assert.AreEqual(response.code, 1);
        }

        [TestMethod]
        public void CanTransferImportance()
        {
            Connection con = new Connection();
            PrivateKey key = new PrivateKey(TestConstants.privKey);

            VerifiableAccount account = new AccountFactory(con).FromPrivateKey(key);
            EncodedAddress recipient = new EncodedAddress(TestConstants.address);

            var response = account.ImportanceTransfer(TestConstants.listOfPublicKeys[0], true).Result;

            Assert.AreEqual(response.code, 1);
        }
        /*
        [TestMethod]
        public void CanCancleImportanceTransfer()
        {
            Connection con = new Connection();
            PrivateKey key = new PrivateKey(TestConstants.privKey);

            VerifiableAccount account = new AccountFactory(con).FromPrivateKey(key);
            EncodedAddress recipient = new EncodedAddress(TestConstants.address);

            var response = account.ImportanceTransfer(TestConstants.listOfPublicKeys[0], false).Result;

            Assert.AreEqual(response.code, 1);
        }
        */
    }
}
