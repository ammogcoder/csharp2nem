using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharp2nem.ResponseObjects.Debug
{
    /// <summary>
    /// An audit collection consists of two arrays, containing information about incoming requests from other nodes.
    /// </summary>
    /// <remarks>
    /// The first array contains information about outstanding (i.e. not yet processed requests) and the second array contains information about the most recent requests. The audit collection is for debug purposes.
    /// </remarks>
    public class AuditCollection
    {
        /// <summary>
        /// Outstanding connection properties.
        /// </summary>
        public class Outstanding
        {
            /// <summary>
            /// The relative URL path.
            /// </summary>
            public string path { get; set; }

            /// <summary>
            /// The number of seconds elapsed since the creation of the nemesis block.
            /// </summary>
            [JsonProperty("start-time")]
            public int startTime { get; set; }

            /// <summary>
            /// The host which initiated the request.
            /// </summary>
            public string host { get; set; }

            /// <summary>
            /// The time in seconds that has elapsed since the request was received.
            /// </summary>
            [JsonProperty("elapsed-time")]
            public int elapsedTime { get; set; }

            /// <summary>
            /// The unique id of the request.
            /// </summary>
            public int id { get; set; }
        }

        /// <summary>
        /// Most recent request properties.
        /// </summary>
        public class MostRecent
        {
            /// <summary>
            /// The relative URL path.
            /// </summary>
            public string path { get; set; }

            /// <summary>
            /// The number of seconds elapsed since the creation of the nemesis block.
            /// </summary>
            [JsonProperty("start-time")]
            public int startTime { get; set; }

            /// <summary>
            /// The host which initiated the request.
            /// </summary>
            public string host { get; set; }

            /// <summary>
            /// The time in seconds that has elapsed since the request was received.
            /// </summary>
            [JsonProperty("elapsed-time")]
            public int elapsedTime { get; set; }

            /// <summary>
            /// The unique id of the request.
            /// </summary>
            public int id { get; set; }
        }

        /// <summary>
        /// The connection request arrays for outstanding and most recent requests.
        /// </summary>
        public class Connections
        {
            /// <summary>
            /// List of outstanding requests. See <see cref="Outstanding"/>
            /// </summary>
            public List<Outstanding> Outstanding { get; set; }

            /// <summary>
            /// List of most recent requests. See <see cref="MostRecent"/>
            /// </summary>
            [JsonProperty("most-recent")]
            public List<MostRecent> MostRecent { get; set; }
        }


    }
}
        
