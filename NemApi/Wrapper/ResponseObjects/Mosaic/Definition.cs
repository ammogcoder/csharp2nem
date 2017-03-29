using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class Definition
    {
        public class Meta
        {
            public int Id { get; set; }
        }

        public class Id
        {
            public string NamespaceId { get; set; }
            public string Name { get; set; }
        }

        public class Property
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class Levy
        {
            public int Type { get; set; }
            public string Recipient { get; set; }
            public Id MosaicId { get; set; }
            public int Fee { get; set; }
        }

        public class Mosaic
        {
            public string Creator { get; set; }
            public string Description { get; set; }
            public Id Id { get; set; }
            public List<Property> Properties { get; set; }
            public Levy Levy { get; set; }
        }

        public class Datum
        {
            public Meta Meta { get; set; }
            public Mosaic Mosaic { get; set; }
        }

        public class List
        {
            public List<Datum> Data { get; set; }
        }
    }
}