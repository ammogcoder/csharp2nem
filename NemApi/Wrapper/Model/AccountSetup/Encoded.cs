

using CSharp2nem.Utils;

namespace CSharp2nem.Model.AccountSetup
{
    /// <summary>
    /// Address class stores account addresses. Converts hyphenated address to unhyphenated address.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        /// <remarks>
        /// Converts hyphenated address to unhyphenated
        /// </remarks>
        /// <param name="address">The account address.</param>
        public Address(string address)
        {
            Encoded = StringUtils.GetResultsWithoutHyphen(address);
        }

        /// <summary>
        /// Gets the encoded address.
        /// </summary>
        /// <remarks>
        /// Use the <see cref="StringUtils"/> class to convert the address to an unhyphenated address. The <see cref="StringUtils"/> class also contains a method to convert unhyphenated addresses to hyphenated addresses.
        /// </remarks>
        /// <value>
        /// The encoded address.
        /// </value>
        public string Encoded { get; set; }
    }
}