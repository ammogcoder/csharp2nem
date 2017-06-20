using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chaos.NaCl;
using CSharp2nem.internals;
using Newtonsoft.Json;

namespace CSharp2nem.Async
{
    /*
     * 
     * Performs HTTP GET & POST requests
     * 
     */
    internal class AsyncConnector
    {

        internal class PostAsync
        {
            /*
             * Create instance of PostAsync
             * 
             * @Connection The Connection to use
             */
            internal PostAsync(Connection connection)
            {
                if (null == connection)
                    throw new ArgumentNullException(nameof(connection));

                Connection = connection;
            }

            // The transaction byte array and corrosponding signature object
            internal ByteArrayWtihSignature Rpa { get; set; }

            private Connection Connection { get; }

            /*
             * Posts data to the specified Connection
             * 
             * @TPostType The Type of data to be sent
             * 
             * @path The API path to send the data to
             * @value The data to be sent
             */
            internal async Task Post<TPostType>(string path, TPostType value)
            {
                
                await Connection.Client.PostAsync(
                    Connection.GetUri(path).Uri,
                    new StringContent(JsonConvert.SerializeObject(value),
                        Encoding.UTF8,
                        "application/json"));

                // Included per request
                //Connection.Client.UploadStringAsync(Connection.GetUri(path).Uri, JsonConvert.SerializeObject(value));
            }

            /*
             * Post data to the specified Connection
             * 
             * @path The API path to post the data
             * 
             * Note: no data is actually posted. This is in support of the nis unlocked info API
             */
            internal async Task<UnlockedInfo> Post(string path)
            {
                var a = new StringContent(string.Empty);

                try
                {
                    var b = await Connection.Client.PostAsync(
                        Connection.GetUri(path).Uri, a);

                    return JsonConvert.DeserializeObject<UnlockedInfo>(await b.Content.ReadAsStringAsync());
                }
                catch (WebException e)
                {
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw new Exception(e.Message);
                    Connection.SetNewHost();
                    return await Post(path);
                }
                //Connection.Client.UploadStringAsync(Connection.GetUri(path).Uri, JsonConvert.SerializeObject(value));
            }

            /*
             * Send the transaction data and signature
             */
            internal async Task<NemAnnounceResponse.Response> Send()
            {
                try
                {
                   
                    var response = await Connection.Client.PostAsync(
                        Connection.GetUri("/transaction/announce").Uri,
                        new StringContent(JsonConvert.SerializeObject(Rpa),
                            Encoding.UTF8,
                            "application/json"));
                    
                    return JsonConvert.DeserializeObject<NemAnnounceResponse.Response>(
                            await response.Content.ReadAsStringAsync());
 
                }
                catch (WebException e)
                {
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw new Exception(e.Message);
                    Connection.SetNewHost();
                    return await Send();
                }
            }
        }

        /*
         * Gets data via a GET request
         * 
         * @TReturnType The type of object to be returned
         */
        internal class GetAsync<TReturnType>
        {
            /*
             * GetAsync constructor
             * 
             * @Connection The Connection to use
             */
            internal GetAsync(Connection connection)
            {
                Connection = connection;
            }

            private Connection Connection { get; }

            /*
             * Sends a get request to the specified connection
             * 
             * @TReturnType The type of object to be returned
             * 
             * @path The API path to use
             * @query The query to use for the request
             */
            internal async Task<TReturnType> Get(string path, string query = "")
            {
                try
                {                   
                    var response = await Connection.Client.GetAsync(Connection.GetUri(path, query).Uri);

                    return JsonConvert.DeserializeObject<TReturnType>(await response.Content.ReadAsStringAsync());
                }
                catch (WebException e)
                {
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw new Exception(e.Message);
                    Connection.SetNewHost();
                    return await Get(path, query);
                }
            }

            /*
             * Get is actually a post request, but returns an object
             * but seemed to make more sense to go under "GetAsync" due to its usage.
             * 
             * @TReturnType The type of data to be returned
             * @TPostType The type of data to be sent
             * 
             * @path the API path to send the data
             * @value the data to be sent
             */
            internal async Task<TReturnType> Get<TPostType>(string path, TPostType value)
            {
                try
                {
                    var response = await Connection.Client.PostAsync(
                        Connection.GetUri(path).Uri,
                        new StringContent(
                            JsonConvert.SerializeObject(value), 
                            Encoding.UTF8, 
                            "application/json"));
                    
                    return JsonConvert.DeserializeObject<TReturnType>(await response.Content.ReadAsStringAsync());
                }
                catch (WebException e)
                {
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw new Exception(e.Message);
                    Connection.SetNewHost();
                    return await Get(path, value);
                }
            }
        }
    }
}