using System;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
    * Creates an aggregate modification
    */

    public class AggregateModification
    {
        /*
        * Constructs a modification
        *
        * @publickKey { PublicKey } The public key of the signatory to modify
        * @modeType { int } The type of modification (1 = add, 2 = remove)
        * 
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