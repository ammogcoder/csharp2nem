// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class HistoricData
    {
        public int Height { get; set; }
        public string Address { get; set; }
        public long Balance { get; set; }
        public long VestedBalance { get; set; }
        public long UnvestedBalance { get; set; }
        public double Importance { get; set; }
        public double PageRank { get; set; }
    }
}