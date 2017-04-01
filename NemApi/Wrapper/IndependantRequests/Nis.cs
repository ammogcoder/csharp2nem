using System.Threading.Tasks;
using CSharp2nem.Async;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    public class NisClient
    {
        /*
         * Constructs the NisClient
         * 
         * @connnection { Connection } The connection to use
         * 
         */
        public NisClient(Connection connection)
        {
            Connection = connection;
        }

        /*
        * Constructs the NisClient
        * 
        * Uses default connection
        * 
        */
        public NisClient()
        {
            Connection = new Connection();
        }

        private Connection Connection { get; }

        
        /*
         * Get the heart beat
         * 
         * http://bob.nem.ninja/docs/#heart-beat-request
         * 
         * Determines if nis is up and responsive
         * 
         * Return: HeartBeat
         */
        public async Task<HeartBeat> HeartBeat()
        {
            return await new AsyncConnector.GetAsync<HeartBeat>(Connection).Get("/heartbeat");
        }

        /*
         * Gets the status of nis
         * 
         * http://bob.nem.ninja/docs/#status-request
         * 
         * Returns: The status of nis
         */
        public async Task<Status> Status()
        {
            return await new AsyncConnector.GetAsync<Status>(Connection).Get("/status");
        }
        
        /*
         * [DEBUG] Gets times sychronization
         * 
         * http://bob.nem.ninja/docs/#monitoring-the-network-time
         * 
         * Return: { TimeSync } The time syncronization
         */
        public async Task<TimeSync> TimeSync()
        {
            return await new AsyncConnector.GetAsync<TimeSync>(Connection).Get("/debug/time-synchronization");
        }
    }
}