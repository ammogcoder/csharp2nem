using System.Security;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class PrivateKey
    {
        /*
         * Creates a PrivateKey
         * 
         * @key { SecureString } The private key to create the object with
         * 
         * Removes any instance of "00" at the start of 66 char negative keys.
         * Removal does not affect the resulting public key or address for
         * pre-existing accounts
         */
        public PrivateKey(SecureString key)
        {
            Raw = key.ConvertToUnsecureString().Length == 66
                ? key.ConvertToUnsecureString().Substring(2, key.Length - 2).ToSecureString()
                : key;
        }

        /*
         * Creates a PrivateKey
         * 
         * @key { string } The private key to create the object with
         *                                                
         */
        public PrivateKey(string key)
        {
            Raw = key.Length == 66
                ? key.Substring(2, key.Length - 2).ToSecureString()
                : key.ToSecureString();
        }

        public SecureString Raw { get; private set; }
    }
}