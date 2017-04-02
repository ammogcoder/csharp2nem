using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * The data required to modify a multisignature account
     * Must be initiated by a cosignatory.
     * 
     * @Modifications { AggregateModification } The modifications to make to the account 
     *      {
     *          ModificationType 1 for add, 2 for remove
     *          PublicKey The account to remove or add as cosignatory
     *      }
     * @MultisigAccount { PublicKey } The multisig account to be modified
     * @RelativeChange { int } The change to the number of minimum cosignatories
     *                         ie. If a multisig account is 2 of 3, a relative change 
     *                         of -1 (without a corrosponding Aggregate modification)
     *                         attached, this will result in a 1 of 3 multisig account. 
     *                         If a cosignatory is then added so that there are 4 signatory
     *                         accounts, to make the account 3 of 4, a relative change of 2 
     *                         should be included, otherwise the multisig account will remain
     *                         as 1 of 4. An addition of a cosignatory account does not increase
     *                         the minimum cosignatories required for a transaction.
     * 
     * @Deadline { int } The deadline when the transaction must be accepted before it is cancled
     * 
     */
    public class AggregateModificationData
    {
        public List<AggregateModification> Modifications { get; set; }
        public PublicKey MultisigAccount { get; set; }
        public int RelativeChange { get; set; }
        public int Deadline { get; set; }
    }
}