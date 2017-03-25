using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class AggregateModificationData
    {
        public List<AggregateModification> Modifications { get; set; }
        public PublicKey MultisigAccount { get; set; }
        public int RelativeChange { get; set; }
        public int Deadline { get; set; }
    }
}