// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class Address
    {
        public Address(string key)
        {
            Encoded = StringUtils.GetResultsWithoutHyphen(key);
        }

        public string Encoded { get; private set; }
    }
}