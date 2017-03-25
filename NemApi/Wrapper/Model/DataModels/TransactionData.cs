using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class TransferTransactionData
    {
        public PublicKey MultisigAccount { get; set; }
        public UnverifiableAccount Recipient { get; set; }
        public long Amount { get; set; }
        public List<Mosaic> ListOfMosaics { get; set; }
        public string Message { get; set; }
        public bool Encrypted { get; set; }
        public int Deadline { get; set; }
    }
}