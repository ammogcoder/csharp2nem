using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Nis
{
    /// <summary>
    /// Communication timestamps contain information about the network time of a remote NIS.
    /// </summary>
    /// <remarks>
    /// NEM uses a time synchronization mechanism to synchronize time across the network. Each node maintains a network time which is the time of the computer clock plus an offset which compensates for the deviation from the computer clocks of other nodes.
    /// </remarks>
    public class NetworkTime
    {
        /// <summary>
        /// The network time at the moment the reply was sent.
        /// </summary>
        public long SendTimeStamp { get; set; }

        /// <summary>
        /// The network time at the moment the request was received.
        /// </summary>
        public long ReceiveTimeStamp { get; set; }
    }

    /// <summary>
    /// Determines if NIS is up and responsive.
    /// </summary>
    public class NemRequestResult
    {
        /// <summary>
        /// The meaning of the code is dependent on the type. For type 1 (validation result) only 0 and 1 mean there was no failure. 
        /// </summary>
        /// <remarks>
        /// The following codes are the most frequent ones occurring:
        /// 
        /// 0:Neutral result. A typical example would be that a node validates an incoming transaction and realizes that it already knows about the transaction. In this case it is neither a success (meaning the node has a new transaction) nor a failure (because the transaction itself is valid).
        /// 1:Success result. A typical example would be that a node validates a new valid transaction.
        /// 2:Unknown failure. The validation failed for unknown reasons.
        /// 3:The entity that was validated has already past its deadline.
        /// 4:The entity used a deadline which lies too far in the future.
        /// 5:There was an account involved which had an insufficient balance to perform the operation.
        /// 6:The message supplied with the transaction is too large.
        /// 7:The hash of the entity which got validated is already in the database.
        /// 8:The signature of the entity could not be validated.
        /// 9:The entity used a timestamp that lies too far in the past.
        /// 10:The entity used a timestamp that lies in the future which is not acceptable.
        /// 11:The entity is unusable.
        /// 12:The score of the remote block chain is inferior (although a superior score was promised).
        /// 13:The remote block chain failed validation.
        /// 14:There was a conflicting importance transfer detected.
        /// 15:There were too many transaction in the supplied block.
        /// 16:The block contains a transaction that was signed by the harvester.
        /// 17:A previous importance transaction conflicts with a new transaction.
        /// 18:An importance transfer activation was attempted while previous one is active.
        /// 19:An importance transfer deactivation was attempted but is not active.
        /// For type 2 the following codes are supported:
        /// 
        /// 1:Successful heart beat detected.
        /// For type 3 the following codes are supported:
        /// 
        /// 0:Unknown status.
        /// 1:NIS is stopped.
        /// 2:NIS is starting.
        /// 3:NIS is running.
        /// 4:NIS is booting the local node (implies NIS is running).
        /// 5:The local node is booted (implies NIS is running).
        /// 6:The local node is synchronized (implies NIS is running and the local node is booted).
        /// 7:There is no remote node available (implies NIS is running and the local node is booted).
        /// 8:NIS is currently loading the block chain.
        /// </remarks>
        public int Code { get; set; }
        /// <summary>
        /// The type is dependent on the request which was answered.
        /// </summary>
        /// <remarks>
        /// The interpretation of the code field depends on the type. Currently the following types are supported:
        /// 
        /// 1: The result is a validation result.
        /// 2: The result is a heart beat result.
        /// 4: The result indicates a status.
        /// </remarks>
        public int Type { get; set; }
        /// <summary>
        /// The message associated with the code.
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// A time synchronization result is the outcome of the network time synchronization of a node with other remote nodes. To agree upon a common time nodes need to synchronize time every hour.
    /// </summary>
    public class TimeSynchronizationResult
    {
        /// <summary>
        /// The date and time when the synchronization was performed.
        /// </summary>
        public string DateTime { get; set; }
        /// <summary>
        /// The current offset to the local computer clock in milliseconds.
        /// </summary>
        public int CurrentTimeOffset { get; set; }
        /// <summary>
        /// The change in milliseconds compared to the last synchronization.
        /// </summary>
        public int Change { get; set; }
    }

    /// <summary>
    /// Time sync data
    /// </summary>
    public class TimeSync
    {
        /// <summary>
        /// Contains a list of <see cref="TimeSynchronizationResult"/> instances returned from nis.
        /// </summary>
        public List<TimeSynchronizationResult> Data { get; set; }
    }
}