using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class AccountForwarded
    {
        public class Account
        {
            public string Address { get; set; }
            public long Balance { get; set; }
            public long VestedBalance { get; set; }
            public double Importance { get; set; }
            public string PublicKey { get; set; }
            public object Label { get; set; }
            public int HarvestedBlocks { get; set; }
        }

        public class Meta
        {
            public List<object> CosignatoryOf { get; set; }
            public List<object> Cosignatories { get; set; }
            public string Status { get; set; }
            public string RemoteStatus { get; set; }
        }

        public class Data
        {
            public Account Account { get; set; }
            public Meta Meta { get; set; }
        }
    }
}