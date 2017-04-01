// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Address
     * 
     * An address with out hypens
     */
    public class Address
    {
        /*
         * Constucts an Address object
         * 
         * @key { string } The address in string format, with or without hyphens
         * 
         */
        public Address(string key)
        {
            Encoded = key.GetResultsWithoutHyphen();
        }

        public string Encoded { get; private set; }
    }
}