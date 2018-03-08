using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Account
{
    /// <summary>
    /// Constaints a number of properties that denote harvesting information about an account
    /// </summary>
    public class HarvestingData
    {

        /// <summary>
        /// Harvested block information
        /// </summary>
        public class Datum
        {
            /// <summary>
            /// The timestamp for the block
            /// </summary>
            public int timeStamp { get; set; }
            /// <summary>
            /// The block difficulty
            /// </summary>
            public long difficulty { get; set; }
            /// <summary>
            /// The total fee harvested for this block
            /// </summary>
            public long totalFee { get; set; }
            /// <summary>
            /// The block id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// The block height
            /// </summary>
            public long height { get; set; }
        }

        public class ListData
        {
            public List<Datum> data { get; set; }
        }
    }
}