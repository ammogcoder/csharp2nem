using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class NetworkTime
    {
        public long SendTimeStamp { get; set; }
        public long ReceiveTimeStamp { get; set; }
    }

    public class HeartBeat
    {
        public int Code { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
    }

    public class Status
    {
        public int Code { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
    }

    public class Datum
    {
        public string DateTime { get; set; }
        public int CurrentTimeOffset { get; set; }
        public int Change { get; set; }
    }

    public class TimeSync
    {
        public List<Datum> Data { get; set; }
    }
}