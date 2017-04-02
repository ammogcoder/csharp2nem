// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Multisig Signature Transaction
     * 
     * @TransactionHash { string } The transaction hash of the transaction to sign
     * @Deadline { int } The deadline by which the transaction should be accepted
     * @MultisigAddress { Address } The multisig account that that contains the pending transaction
     * 
     */
    public class MultisigSignatureTransactionData
    {
        public string TransactionHash { get; set; }
        public int Deadline { get; set; }
        public Address MultisigAddress { get; set; }
    }
}