using System;
using CSharp2nem.Model.AccountSetup;

namespace CSharp2nem.Model.MultiSig
{
    /*
    * 
    */

    /// <summary>
    /// Creates an aggregate modification instance to be used in an aggregate multisig modification transaction.
    /// </summary>
    public class AggregateModification
    {
        /// <summary>
        /// Constructs an instance of <see cref="AggregateModification"/> based on a set of parameters.
        /// </summary>
        /// <param name="publicKey">The public key of the co-signatory that should be added as a co-signatory or removed.</param>
        /// <param name="modType">The modification type to be applied to the given public key.</param>
        /// <exception cref="ArgumentNullException">Public key cannot be null.</exception>
        /// <exception cref="ArgumentException">Invalid modification type. Must be type 1 or 2.</exception>
        /// <remarks>
        /// A modification of type 1 indicates that the given account should be added as a co-signatory. A modification of type 2 indicates that the given account should be removed as a co-signatory. 
        /// </remarks>
        public AggregateModification(PublicKey publicKey, int modType)
        {
            if (publicKey == null)
                throw new ArgumentNullException(nameof(publicKey));
            if (modType != 1 && modType != 2)
                throw new ArgumentException("Modification type invalid");

            ModificationType = modType;
            PublicKey = publicKey;
        }

        internal int ModificationType { get; set; }
        internal PublicKey PublicKey { get; set; }
    }
}