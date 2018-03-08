

namespace CSharp2nem.ResponseObjects.Account
{
    /// <summary>
    /// Contains a number of properties for an account at a certain point in history.
    /// </summary>
    public class HistoricData
    {
        /// <summary>
        /// The block height for which the data is valid.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// The address of the account
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The balance of the account
        /// </summary>
        public long Balance { get; set; }
        /// <summary>
        /// The vested balance of the account
        /// </summary>
        public long VestedBalance { get; set; }
        /// <summary>
        /// The unvested balance of the account
        /// </summary>
        public long UnvestedBalance { get; set; }
        /// <summary>
        /// The importance of the account 
        /// </summary>
        public double Importance { get; set; }
        /// <summary>
        /// The rank part of the importance
        /// </summary>
        public double PageRank { get; set; }
    }
}