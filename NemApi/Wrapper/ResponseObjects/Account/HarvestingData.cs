using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class HarvestingData
    {
        public class BlockHash
        {
            public string Data { get; set; }
        }

        public class Datum
        {
            public int TimeStamp { get; set; }
            public BlockHash BlockHash { get; set; }
            public int TotalFee { get; set; }
            public int Height { get; set; }
        }

        public class ListData
        {
            public List<Datum> Data { get; set; }
        }
    }
}