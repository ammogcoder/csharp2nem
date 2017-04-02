// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class ImportanceTransferData
    {
        /*
         * Importance transfer data
         * 
         * @MultisigAccount { PublicKey } The multisig account that should transfer its importance
         *                                Note: use this when initiating from a cosignatory. 
         *                                When left as null, the transaction is initiated for 
         *                                the signing account
         * 
         * @DelegatedAccount { PublicKey } The account to transfer importance to
         * @Activate { bool } Set true to activate an importance transfer, false to revoke an importance transfer
         * @Deadline { int } The deadline when the transction must be accepted by.
         * 
         */
        public PublicKey MultisigAccount { get; set; }
        public PublicKey DelegatedAccount { get; set; }
        public bool Activate { get; set; }
        public int Deadline { get; set; }
    }
}