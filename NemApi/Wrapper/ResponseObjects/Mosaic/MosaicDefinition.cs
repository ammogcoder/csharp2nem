using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class MosaicDefinitions
    {
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

        public class Mosaic
        {
            public string Creator { get; set; }
            public Id Id { get; set; }
            public string Description { get; set; }
            public List<Property> Properties { get; set; }
        }

        public class List
        {
            public List<Mosaic> Data { get; set; }
        }
    }
}