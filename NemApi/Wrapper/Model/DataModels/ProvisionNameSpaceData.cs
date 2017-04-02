// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
     * Provision NameSpace Data
     * 
     * @MultisigAccount { PublicKey } The multisig account the transaction should be initiate for
     * @NewPart { string } The name of the new namespace that should be created. 
     * @Parent { string }  The parent namespace under which the namespace should be created.
     *                     Note: In the absence of a parent namespace, the "NewPart" becomes
     *                           the parent namespace
     * @Deadline { int } The deadline by which the transaction must be accepted                    
     * 
     */
    public class ProvisionNameSpaceData
    {
        public PublicKey MultisigAccount { get; set; }
        public string NewPart { get; set; }
        public string Parent { get; set; }
        public int Deadline { get; set; }
    }
}