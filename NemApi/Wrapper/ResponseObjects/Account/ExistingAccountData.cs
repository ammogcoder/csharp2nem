using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Account
{
    public class ExistingAccount
    {
        public class Cosignatory
        {
            public string Address { get; set; }
            public int HarvestedBlocks { get; set; }
            public long Balance { get; set; }
            public double Importance { get; set; }
            public long VestedBalance { get; set; }
            public string PublicKey { get; set; }
            public object Label { get; set; }
            public MultisigInfo MultisigInfo { get; set; }
        }

        public class MultisigInfo
        {
            public int CosignatoriesCount { get; set; }
            public int MinCosignatories { get; set; }
        }

        public class MultisigInfo2
        {
            public int CosignatoriesCount { get; set; }
            public int MinCosignatories { get; set; }
        }

        public class CosignatoryOf
        {
            public string Address { get; set; }
            public int HarvestedBlocks { get; set; }
            public long Balance { get; set; }
            public double Importance { get; set; }
            public long VestedBalance { get; set; }
            public string PublicKey { get; set; }
            public object Label { get; set; }
            public MultisigInfo MultisigInfo { get; set; }
        }

        public class Account
        {
            public string Address { get; set; }
            public int HarvestedBlocks { get; set; }
            public long Balance { get; set; }
            public double Importance { get; set; }
            public long VestedBalance { get; set; }

            public string PublicKey { get; set; }
            public object Label { get; set; }
            public MultisigInfo2 MultisigInfo2 { get; set; }
        }

        public class Meta
        {
            public List<Cosignatory> Cosignatories { get; set; }
            public List<CosignatoryOf> CosignatoryOf { get; set; }
            public string Status { get; set; }
            public string RemoteStatus { get; set; }
        }

        public class Data
        {
            public Meta Meta { get; set; }
            public Account Account { get; set; }
        }
    }
}