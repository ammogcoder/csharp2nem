

namespace CSharp2nem.ResponseObjects
{
    /*
    * Stores stores the transaction byte array and signature data to be prepared amd sent to nis.
    */

    public class ByteArrayWtihSignature
    {
        public string data { get; set; }
        public string signature { get; set; }
    }
}