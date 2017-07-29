using System.Security;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.AccountSetup
{
    /// <summary>
    /// PrivateKey class contains stores private keys. Converts string plain text keys to SecureString for storage.
    /// </summary>
    public class PrivateKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateKey"/> class.
        /// </summary>
        /// <remarks>
        /// The <see cref="StringUtils"/> class contains methods to convert to or from secure string.
        /// </remarks>
        /// <param name="key">The SecureString private key.</param>
        public PrivateKey(SecureString key)
        {
           
            Raw = StringUtils.ConvertToUnsecureString(key).Length == 66
                ? StringUtils.ToSecureString(StringUtils.ConvertToUnsecureString(key).Substring(2, key.Length - 2))
                : key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateKey"/> class.
        /// </summary>
        /// <remarks>
        /// Converts the string key to SecureString. The <see cref="StringUtils"/> class contains methods to convert to or from secure string.
        /// </remarks>
        /// <param name="key">The private key string.</param>
        public PrivateKey(string key)
        {
            
            Raw = key.Length == 66
               ? StringUtils.ToSecureString(key.Substring(2, key.Length - 2))
               : StringUtils.ToSecureString(key);
        }

        /// <summary>
        /// Gets the private key SecureString.
        /// </summary>
        /// <remarks>
        /// The <see cref="StringUtils"/> class contains methods to convert to or from secure string.
        /// </remarks>
        /// <value>
        /// The private key secure string.
        /// </value>
        public SecureString Raw { get; private set; }
    }
}