using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class MosaicsOwned
    {
        public class MosaicId
        {
            public string NamespaceId { get; set; }
            public string Name { get; set; }
        }

        public class Mosaic
        {
            public MosaicId MosaicId { get; set; }
            public long Quantity { get; set; }
        }

        public class RootObject
        {
            public List<Mosaic> Data { get; set; }
        }
    }
}