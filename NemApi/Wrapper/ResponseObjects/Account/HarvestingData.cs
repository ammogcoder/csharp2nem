using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class HarvestingData
    {

        public class Datum
        {
            public int timeStamp { get; set; }
            public long difficulty { get; set; }
            public long totalFee { get; set; }
            public int id { get; set; }
            public long height { get; set; }
        }

        public class ListData
        {
            public List<Datum> data { get; set; }
        }
    }
}