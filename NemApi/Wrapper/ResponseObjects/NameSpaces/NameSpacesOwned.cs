using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.NameSpaces
{
    /// <summary>
    /// Contains a number of namespace related properties
    /// </summary>
    public class NameSpace
    {
        /// <summary>
        /// The fully qualified name of the namespace, also named namespace id.
        /// </summary>
        public string Fqn { get; set; }

        /// <summary>
        /// The name space owner public key 
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The height at which the ownership begins
        /// </summary>
        public long Height { get; set; }
    }

    /// <summary>
    /// A list of namespace instances
    /// </summary>
    public class NameSpaceList
    {
        /// <summary>
        /// The list of namespaces. See <see cref="NameSpace"/>
        /// </summary>
        public List<NameSpace> Data { get; set; }
    }
}