using System.Threading.Tasks;
using NemApi.Async;

// ReSharper disable once CheckNamespace

namespace NemApi
{
    public class Nis
    {
        public Nis(Connection connection)
        {
            Connection = connection;
        }

        public Nis()
        {
            Connection = new Connection();
        }

        private Connection Connection { get; }

        public async Task<TimeSync> TimeSync()
        {
            return await new AsyncConnector.GetAsync<TimeSync>(Connection).Get("/debug/time-synchronization");
        }

        public async Task<HeartBeat> HeartBeat()
        {
            return await new AsyncConnector.GetAsync<HeartBeat>(Connection).Get("/heartbeat");
        }

        public async Task<Status> Status()
        {
            return await new AsyncConnector.GetAsync<Status>(Connection).Get("/status");
        }

        public async Task<NetworkTime> NetworkTime()
        {
            return await new AsyncConnector.GetAsync<NetworkTime>(Connection).Get("/time-sync/network-time");
        }
    }
}