using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharp2nem;

namespace Tests.Account
{
    [TestClass]
    public class AccountTest
    {

       // [TestMethod]
       // public void CanCreateVerifiableAccountFromData()
       // {
       //     var con = new Connection();
       //     con.SetTestNet();
       //
       //     var key = new PrivateKey(TestConstants.PrivKey);
       //
       //     var account = new AccountFactory().FromsNewDataPrivateKey("dsfsdgdfgdgdfgdfgdfg");
       //     Assert.AreEqual(TestConstants.Address, account.Address.Encoded);
       //     Assert.AreEqual(TestConstants.PubKey, account.PublicKey.Raw);
       //     Assert.AreEqual(TestConstants.PrivKey, account.PrivateKey.Raw.ConvertToUnsecureString());
       // }

        [TestMethod]
        public void CanCreateVerifiableAccount()
        {
            var con = new Connection();
            con.SetTestNet();

            var key = new PrivateKey(TestConstants.PrivKey);

            var account = new VerifiableAccount(con, key);
            Assert.AreEqual(TestConstants.Address, account.Address.Encoded);
            Assert.AreEqual(TestConstants.PubKey, account.PublicKey.Raw);
            Assert.AreEqual(TestConstants.PrivKey, account.PrivateKey.Raw.ConvertToUnsecureString());
        }

        [TestMethod]
        public void CanCreateUnverifiableAccountFromPublicKey()
        {
            var con = new Connection();
            con.SetTestNet();

            var key = new PublicKey(TestConstants.PubKey);

            var account = new UnverifiableAccount(con, key);

            Assert.AreEqual(TestConstants.Address, account.Address.Encoded);
            Assert.AreEqual(TestConstants.PubKey, account.PublicKey.Raw);
        }

        [TestMethod]
        public void CanCreateUnverifiableAccountFromPublicKeyWithAccountFactory()
        {
            var con = new Connection();
            con.SetTestNet();

            var key = new PublicKey(TestConstants.PubKey);

            var account = new AccountFactory(con).FromPublicKey(key);
            var account2 = new AccountFactory(con).FromPublicKey(TestConstants.PubKey);

            Assert.AreEqual(TestConstants.Address, account.Address.Encoded);
            Assert.AreEqual(TestConstants.PubKey, account.PublicKey.Raw);

            Assert.AreEqual(TestConstants.Address, account2.Address.Encoded);
            Assert.AreEqual(TestConstants.PubKey, account2.PublicKey.Raw);
        }

        [TestMethod]
        public void CanCreateVerifiableAccountFromPrivateKeyWithAccountFactory()
        {
            var con = new Connection();
            con.SetTestNet();

            var key = new PrivateKey(TestConstants.PrivKey);

            var account = new AccountFactory(con).FromPrivateKey(key);

            Assert.AreEqual(TestConstants.Address, account.Address.Encoded);
            Assert.AreEqual(TestConstants.PubKey, account.PublicKey.Raw);
            Assert.AreEqual(TestConstants.PrivKey, account.PrivateKey.Raw.ConvertToUnsecureString());

            var account2 = new AccountFactory(con).FromPrivateKey(TestConstants.PrivKey);

            Assert.AreEqual(TestConstants.Address, account2.Address.Encoded);
            Assert.AreEqual(TestConstants.PubKey, account2.PublicKey.Raw);
            Assert.AreEqual(TestConstants.PrivKey, account2.PrivateKey.Raw.ConvertToUnsecureString());
        }
    }
}
