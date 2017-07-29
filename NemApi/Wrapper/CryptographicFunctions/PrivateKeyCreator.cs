using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.RequestClients;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace CSharp2nem.CryptographicFunctions
{
    public static class PrivateKeyCreator
    {
        public static PrivateKey Create()
        {
            var r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            var keys = r.GenerateKeyPair();      
            return new PrivateKeyAccountClientFactory().FromNewDataPrivateKey(keys.Private.ToString()).PrivateKey;
        }
    }
}
