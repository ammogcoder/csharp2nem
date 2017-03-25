// ReSharper disable once CheckNamespace

namespace NemApi
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