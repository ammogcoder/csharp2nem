// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class MultisigSignatureTransactionData
    {
        public string TransactionHash { get; set; }
        public int Deadline { get; set; }
        public Address MultisigAddress { get; set; }
    }
}