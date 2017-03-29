using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class Importances
    {
        public class Importance
        {
            public int IsSet { get; set; }
            public double Score { get; set; }
            public double Ev { get; set; }
            public long Height { get; set; }
        }

        public class Datum
        {
            public string Address { get; set; }
            public Importance Importance { get; set; }
        }

        public class ListImportances
        {
            public List<Datum> Data { get; set; }
        }
    }
}