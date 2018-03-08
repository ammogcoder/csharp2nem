using System;
using System.Security;
using System.Text;
using Chaos.NaCl;
using CSharp2nem.Connectivity;
using CSharp2nem.RequestClients;
using CSharp2nem.Utils;
using Org.BouncyCastle.Crypto.Digests;

namespace CSharp2nem.Model.AccountSetup
{

    /// <summary>
    /// The <see cref="PrivateKeyAccountClientFactory"/> class contains a number of methods for creating <see cref="PrivateKeyAccountClient"/> instances. 
    /// </summary>
    public class PrivateKeyAccountClientFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateKeyAccountClientFactory"/> class.
        /// </summary>
        /// <param name="connection">The connection to use when creating private key account client.</param>
        /// <exception cref="ArgumentNullException">the connection connot be null.</exception>
        /// <example> 
        /// This sample shows how to create an instance of the <see cref="PrivateKeyAccountClientFactory"/> class.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {
        ///         Connection connection = new Connection();
        /// 
        ///         AccountFactory accountFactory = new AccountFactory(connection);               
        ///     }   
        /// }
        /// </code>
        /// </example>
        public PrivateKeyAccountClientFactory(Connection connection)
        {
            if (null == connection)
                throw new ArgumentNullException(nameof(connection));

            Connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateKeyAccountClientFactory"/> class.
        /// </summary>
        /// <remarks>
        /// Sets a default main net connection. The default main net connection defaults to a pool of trusted developer nodes but does not select the fastest node by default.
        /// </remarks>
        /// <example> 
        /// This sample shows how to create an instance of the <see cref="PrivateKeyAccountClientFactory"/> class.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {
        ///         AccountFactory accountFactory = new AccountFactory();
        ///     }
        /// }
        /// </code>
        /// </example>
        public PrivateKeyAccountClientFactory()
        {
            Connection = new Connection();      
        }

        private Connection Connection { get; }

        /// <summary>
        /// Produces a <see cref="PrivateKeyAccountClient"/> from provided data.
        /// </summary>
        /// <remarks>
        /// Takes a string of any description, hashes it with Sha3 and converts the produced bytes to a 64 char hex string. 
        /// </remarks>
        /// <param name="data">The data to convert to a private key.</param>
        /// <returns><see cref="PrivateKeyAccountClient"/> that can be used to initiate transactions.</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="FromNewDataPrivateKey"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {
        ///         AccountFactory accountFactory = new AccountFactory();
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromNewDataPrivateKey("anyText");
        ///     }
        /// }
        /// </code>
        /// </example>
        public PrivateKeyAccountClient FromNewDataPrivateKey(string data)
        {
            var digestSha3 = new KeccakDigest(256);
            var dataBytes = Encoding.Default.GetBytes(data);
            var pkBytes = new byte[32];

            digestSha3.BlockUpdate(dataBytes, 0, 32);
            digestSha3.DoFinal(pkBytes, 0);
            var sk = StringUtils.ToSecureString(CryptoBytes.ToHexStringLower(pkBytes));
            

            return FromPrivateKey(new PrivateKey(sk));
        }

        /// <summary>
        /// Produces a <see cref="PrivateKeyAccountClient"/> from a given <see cref="SecureString"/> of any size.
        /// </summary>
        /// <remarks>
        /// Takes a SecureString, hashes with Sha3 and converts the produced bytes to a 64 char hex string. The <see cref="StringUtils"/> class contains methods to convert to or from <see cref="SecureString"/>.
        /// </remarks>
        /// <param name="data">The data to convert to a private key.</param>
        /// <returns>A <see cref="PrivateKeyAccountClient"/> that can be used to initiate transactions.</returns>
        public PrivateKeyAccountClient FromNewDataPrivateKey(SecureString data)
        {
            var digestSha3 = new KeccakDigest(256);
            var dataBytes = Encoding.Default.GetBytes(StringUtils.ConvertToUnsecureString(data));
            var pkBytes = new byte[32];

            digestSha3.BlockUpdate(dataBytes, 0, 32);
            digestSha3.DoFinal(pkBytes, 0);
            var sk = StringUtils.ToSecureString(CryptoBytes.ToHexStringLower(pkBytes));
            
            return FromPrivateKey(new PrivateKey(sk));
        }

        /// <summary>
        /// Produces a <see cref="PrivateKeyAccountClient"/> from a given private key string.
        /// </summary>
        /// <remarks>
        /// The private key string must be 64 or 66 character hexidecimal only.
        /// </remarks>
        /// <param name="key">The private key string to use.</param>
        /// <returns>A <see cref="PrivateKeyAccountClient"/> that can be used to initiate transactions</returns>
        /// <exception cref="ArgumentException">invalid private key. Exception bounds: Must be hexidecimal string. Must be 64 or 66 chars long.</exception>
        /// <example> 
        /// This sample shows how to use the <see cref="FromPrivateKey"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {
        ///         AccountFactory accountFactory = new AccountFactory();
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///     }
        /// }
        /// </code>
        /// </example>
        public PrivateKeyAccountClient FromPrivateKey(string key)
        {
            if (!StringUtils.OnlyHexInString(key) || key.Length != 64 && key.Length != 66)
                throw new ArgumentException("invalid private key");

            return new PrivateKeyAccountClient(Connection, new PrivateKey(key));
        }

        /// <summary>
        /// Produces a <see cref="PrivateKeyAccountClient"/> from a given PrivateKey instance.
        /// </summary>
        /// <param name="key">The PrivateKey instance to use.</param>
        /// <returns>A <see cref="PrivateKeyAccountClient"/> that can be used to initiate transactions</returns>
        /// <exception cref="ArgumentException">invalid private key. Exception bounds: Must be hexidecimal string. Must be 64 or 66 chars long.</exception>
        public PrivateKeyAccountClient FromPrivateKey(PrivateKey key)
        {
            if (!StringUtils.OnlyHexInString(key.Raw) || key.Raw.Length != 64 && key.Raw.Length != 66)
                throw new ArgumentException("invalid private key");

            return new PrivateKeyAccountClient(Connection, key);
        }

        /// <summary>
        /// Produces a <see cref="PrivateKeyAccountClient"/> from a given private key <see cref="SecureString"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="StringUtils"/> class contains methods to convert to or from <see cref="SecureString"/>
        /// </remarks>
        /// <param name="key">The SecureString encoded private key to use.</param>
        /// <returns>Private key account client that can be used to initiate transactions</returns>
        /// <exception cref="ArgumentException">invalid private key. Exception bounds: Must be hexidecimal string. Must be 64 or 66 chars long.</exception>
        public PrivateKeyAccountClient FromPrivateKey(SecureString key)
        {
            if (!StringUtils.OnlyHexInString(StringUtils.ConvertToUnsecureString(key)) || StringUtils.ConvertToUnsecureString(key).Length != 64 && StringUtils.ConvertToUnsecureString(key).Length != 66)
                throw new ArgumentException("invalid private key");

            return new PrivateKeyAccountClient(Connection, new PrivateKey(key));
        }
    }
}