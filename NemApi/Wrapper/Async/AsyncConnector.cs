using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

            // The transaction byte array and corrosponding signature
            internal ByteArrayWtihSignature Rpa { get; set; }

            // The Connection to use
            private Connection Connection { get; }

            /*
             * Posts data to the specified Connection
             * 
             * @TPostType The Type of data to be sent
             * 
             * @path The API path to send the data to
             * @value The data to be sent
             */
            internal async void Post<TPostType>(string path, TPostType value)
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

                // the content to be sent
                var a = new StringContent(string.Empty);

                try
                {
                    // send data to specified connection and wait for the response
                    var b = await Connection.Client.PostAsync(
                        // get the fully qualified path
                        Connection.GetUri(path).Uri, a);

                    // convert json response to c# object and return
                    return JsonConvert.DeserializeObject<UnlockedInfo>(await b.Content.ReadAsStringAsync());
                }
                catch (Exception e)
                {
                
                    // check if a new host should be found upon request failure
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw new WebException();

                    // set a new host
                    Connection.SetNewHost();

                    // resend the request
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
                    // send the transaction data and signature to the specified connection and wait for the response.
                    var response = await Connection.Client.PostAsync(
                        // get the fully qualified path
                        Connection.GetUri("/transaction/announce").Uri,
                        // create the content from the serialized transacion object
                        new StringContent(JsonConvert.SerializeObject(Rpa),
                            Encoding.UTF8,
                            "application/json"));
                    
                    // return the deserialized response
                    return JsonConvert.DeserializeObject<NemAnnounceResponse.Response>(
                            await response.Content.ReadAsStringAsync());

                    #region .NET 3.0 compatible

                   //var http = (HttpWebRequest)WebRequest.Create(Connection.GetUri("/transaction/announce").Uri);
                   //http.Accept = "application/json";
                   //http.ContentType = "application/json";
                   //http.Method = "POST";
                   //
                   //var parsedContent = JsonConvert.SerializeObject(Rpa);
                   //var encoding = new ASCIIEncoding();
                   //var bytes = encoding.GetBytes(parsedContent);
                   //var newStream = http.GetRequestStream();
                   //newStream.Write(bytes, 0, bytes.Length);
                   //newStream.Close();
                   //
                   //
                   //var task = await Task.Factory.FromAsync(
                   //        http.BeginGetResponse,
                   //        asyncResult => http.EndGetResponse(asyncResult),
                   //        null);
                   //
                   //
                   //return JsonConvert.DeserializeObject<NemAnnounceResponse.Response>(new StreamReader(task.GetResponseStream()).ReadToEnd());

                    #endregion
                }
                catch (WebException)
                {
                    // check if a new host should be found upon request failure
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw new WebException();

                    // set new host
                    Connection.SetNewHost();

                    // resend the transaction
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

                    #region .NET 3.0 compatible

                    //var http = WebRequest.Create(Connection.GetUri(path, query).Uri);
                    //
                    //var task = await Task.Factory.FromAsync(
                    //    http.BeginGetResponse,
                    //    asyncResult => http.EndGetResponse(asyncResult),
                    //    null);           
                    //
                    //return JsonConvert.DeserializeObject<TReturnType>(new StreamReader(task.GetResponseStream()).ReadToEnd().Trim());

                    #endregion
                }
                catch (WebException)
                {
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw;
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

                    #region .NET 3.0 compatible

                    //var http = (HttpWebRequest)WebRequest.Create(Connection.GetUri(path).Uri);
                    //http.Accept = "application/json";
                    //http.ContentType = "application/json";
                    //http.Method = "POST";
                    //
                    //var parsedContent = JsonConvert.SerializeObject(value);
                    //var encoding = new ASCIIEncoding();
                    //var bytes = encoding.GetBytes(parsedContent);
                    //
                    //var newStream = http.GetRequestStream();
                    //newStream.Write(bytes, 0, bytes.Length);
                    //newStream.Close();
                    //
                    //var task = await Task.Factory.FromAsync(
                    //    http.BeginGetResponse,
                    //    asyncResult => http.EndGetResponse(asyncResult),
                    //    null);
                    //
                    //return JsonConvert.DeserializeObject<TReturnType>(new StreamReader(task.GetResponseStream()).ReadToEnd());

                    #endregion
                }
                catch (WebException)
                {
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw;
                    Connection.SetNewHost();
                    return await Get(path, value);
                }
            }
        }
    }
}