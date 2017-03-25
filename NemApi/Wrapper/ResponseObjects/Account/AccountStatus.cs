using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class Account
    {
        public class Status
        {
            public List<object> CosignatoryOf { get; set; }

            public string status { get; set; }

            public string RemoteStatus { get; set; }
        }
    }
}