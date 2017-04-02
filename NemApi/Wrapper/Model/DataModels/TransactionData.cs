using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Transfer Transaction Data
     * 
     * @MultisigAccount { PublicKey } The multisig account the transaction should be intiated for
     * @Recipient { UnverifiableAccount } The recipient address the transcation should be sent to
     * @Amount { long } The amount of XEM that should be sent
     * @ListOfMosaics { List<Mosaics> } The list of mosaics to be sent in the transaction
     * @Message { string } The message to attach to the transaction
     * @Encrypted { bool } indicates whether the message should be encrypted
     * @Deadline { int } The deadline by which the transaction should be sent
     * @FeeDeductedFromAmount { bool } Indicates whether the fee should be deducted from the amount
     *                                  ie. sending 100 xem with a fee of one results in the recipient 
     *                                  recieving 99 xem.
     */
    public class TransferTransactionData
    {
        public PublicKey MultisigAccount { get; set; }
        public UnverifiableAccount Recipient { get; set; }
        public long Amount { get; set; }
        public List<Mosaic> ListOfMosaics { get; set; }
        public string Message { get; set; }
        public bool Encrypted { get; set; }
        public int Deadline { get; set; }
        public bool FeeDeductedFromAmount { get; set; }
    }
}