using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharp2nem.ResponseObjects.Debug
{
    /// <summary>
    /// NIS uses timers to schedule periodic tasks. Those tasks are monitored and their result is memorized. The NemAsyncTimeVisitor structure holds the information.
    /// </summary>
    public class DebugTimers
    {
        /// <summary>
        /// Contains a number of properties for timer information
        /// </summary>
        public class Data
        {
            /// <summary>
            /// The number of milliseconds since the last execution of the timer.
            /// </summary>
            [JsonProperty("last-delay-time")]
            public int lastDelayTime { get; set; }

            /// <summary>
            /// The number of times the task was executed.
            /// </summary>
            public int executions { get; set; }
            /// <summary>
            /// The number times the task failed.
            /// </summary>
            public int failures { get; set; }

            /// <summary>
            /// The number times the task was successful.
            /// </summary>
            public int successes { get; set; }

            /// <summary>
            /// The time at which the task started last time.
            /// </summary>
            [JsonProperty("last-operation-start-time")]
            public int lastOperationStartTime { get; set; }

            /// <summary>
            /// True if the task is executing, false otherwise.
            /// </summary>
            [JsonProperty("is-executing")]
            public int isExecuting { get; set; }

            /// <summary>
            /// The name of the task.
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// The number of seconds the task needed on average.
            /// </summary>
            [JsonProperty("average-operation-time")]
            public int averageOperationTime { get; set; }

            /// <summary>
            /// The number of seconds the task needed the last time.
            /// </summary>
            [JsonProperty("last-operation-time")]
            public int lastOperationTime { get; set; }
        }

        /// <summary>
        /// Contains timer information
        /// </summary>
        public class Timers
        {
            /// <summary>
            /// List of timer information
            /// </summary>
            public List<Data> data { get; set; }
        }
    }
}
