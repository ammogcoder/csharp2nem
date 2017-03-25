using System;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    /*
    * Creates an aggregate modification
    */

    public class AggregateModification
    {
        /*
        * Constructs a modification
        *
        * @Param: PublicKey, The public key of the signatory to modify
        * @Param: ModType, The type of modification (1 = add, 2 = remove)
        */

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