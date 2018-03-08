using CSharp2nem.Model.AccountSetup;
using CSharp2nem.RequestClients;

namespace CSharp2nem.Model.DataModels
{
    /// <summary>
    /// The data used to initiate a provision namespace transaction. See: <see cref="PrivateKeyAccountClient.BeginProvisionNamespaceAsync"/>
    /// </summary>
    public class ProvisionNameSpaceData
    {
        /// <summary>
        /// The multisig account that should create and own the namespace.
        /// </summary>
        /// <remarks>
        /// A co-signatory of the given multisig account must be the initiator of the transaction when this property is initialised. When not initialised, the namespace is acquired by the signing account.
        /// </remarks>
        public PublicKey MultisigAccount { get; set; }

        /// <summary>
        /// The name of the new root or sub namespace that should be created.
        /// </summary>
        /// /// <remarks>
        /// Mosaic name valid characters are: [a, b, c, ..., z, A, B, C, ..., Z, 0, 1, 2, ..., 9, _ , -]. A namepsace cannot start with a number or special character. ie. '1alice' is not valid. 'ali1ce' is valid. A root namespace can be 16 characters long. When the Parent property is not initialised, the newPart creates a new root namespace. When the Parent is initialised, the newPart will be a new sub namespace under the parent namespace.
        /// </remarks>
        public string NewPart { get; set; }

        /// <summary>
        /// The name of the root namespace that a sub namespace should be created under.
        /// </summary>
        /// /// <remarks>
        /// Mosaic name valid characters are: [a, b, c, ..., z, A, B, C, ..., Z, 0, 1, 2, ..., 9, _ , -]. A namepsace cannot start with a number or special character. ie. '1alice' is not valid. 'ali1ce' is valid. A sub namespace can be 64 characters long. When the Parent is initialised, the newPart will be a new sub namespace under the parent namespace.
        /// </remarks>
        public string Parent { get; set; }

        /// <summary>
        /// The deadline by which the transaction must be accepted befre it is rejected by the network.
        /// </summary>
        public int Deadline { get; set; }
    }
}