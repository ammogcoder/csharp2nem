using System.Security;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class PrivateKey
    {
        public PrivateKey(SecureString key)
        {
            Raw = key.ConvertToUnsecureString().Length == 66
                ? key.ConvertToUnsecureString().Substring(2, key.Length - 2).ToSecureString()
                : key;
        }

        public PrivateKey(string key)
        {
            Raw = key.Length == 66
                ? key.Substring(2, key.Length - 2).ToSecureString()
                : key.ToSecureString();
        }

        public SecureString Raw { get; private set; }
    }
}