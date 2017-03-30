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
    internal class AsyncConnector
    {
        internal class PostAsync
        {
            internal PostAsync(Connection connection)
            {
                if (null == connection)
                    throw new ArgumentNullException(nameof(connection));

                Connection = connection;
            }

            internal ByteArrayWtihSignature Rpa { get; set; }
            private Connection Connection { get; }

            internal void Post<TPostType>(string path, TPostType value)
            {

                //await Connection.Client.PostAsync(
                //    Connection.GetUri(path).Uri,
                //    new StringContent(JsonConvert.SerializeObject(value),
                //        Encoding.UTF8,
                //        "application/json"));
                //
                 Connection.Client.UploadStringAsync(Connection.GetUri(path).Uri, JsonConvert.SerializeObject(value));
            }

            internal async Task<NemAnnounceResponse.Response> Send()
            {
                try
                {
                   // var response = await Connection.Client.PostAsync(
                   //     Connection.GetUri("/transaction/announce").Uri,
                   //     new StringContent(JsonConvert.SerializeObject(Rpa).ToString(),
                   //         Encoding.UTF8,
                   //         "application/json"));
                   // 
                   // return
                   //     JsonConvert.DeserializeObject<NemAnnounceResponse.Response>(
                   //         await response.Content.ReadAsStringAsync());

                    #region .NET 3.0 compatible

                    var http = (HttpWebRequest)WebRequest.Create(Connection.GetUri("/transaction/announce").Uri);
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";
                    
                    var parsedContent = JsonConvert.SerializeObject(Rpa);
                    var encoding = new ASCIIEncoding();
                    var bytes = encoding.GetBytes(parsedContent);
                    var newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();
                   
                    
                    var task = await Task.Factory.FromAsync(
                            http.BeginGetResponse,
                            asyncResult => http.EndGetResponse(asyncResult),
                            null);
                   

                    return JsonConvert.DeserializeObject<NemAnnounceResponse.Response>(new StreamReader(task.GetResponseStream()).ReadToEnd());

                    #endregion
                }
                catch (WebException)
                {
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw new WebException();

                    Connection.SetNewHost();
                    return await Send();
                }
            }
        }

        internal class GetAsync<TReturnType>
        {
            internal GetAsync(Connection connection)
            {
                Connection = connection;
            }

            private Connection Connection { get; }

            internal async Task<TReturnType> Get(string path, string query = "")
            {
                try
                {                   
                    //var response = await Connection.Client.GetAsync(Connection.GetUri(path, query).Uri);
                    //
                    //return JsonConvert.DeserializeObject<TReturnType>(await response.Content.ReadAsStringAsync());

                    #region .NET 3.0 compatible

                    var http = WebRequest.Create(Connection.GetUri(path, query).Uri);

                    var task = await Task.Factory.FromAsync(
                        http.BeginGetResponse,
                        asyncResult => http.EndGetResponse(asyncResult),
                        null);           
                    
                    return JsonConvert.DeserializeObject<TReturnType>(new StreamReader(task.GetResponseStream()).ReadToEnd().Trim());

                    #endregion
                }
                catch (WebException)
                {
                    if (!Connection.ShouldFindNewHostIfRequestFails) throw;
                    Connection.SetNewHost();
                    return await Get(path, query);
                }
            }

            internal async Task<TReturnType> Get<TPostType>(string path, TPostType value)
            {
                try
                {
                    //var response = await Connection.Client.PostAsync(
                    //    Connection.GetUri(path).Uri,
                    //    new StringContent(
                    //        JsonConvert.SerializeObject(value), 
                    //        Encoding.UTF8, 
                    //        "application/json"));
                    //
                    //return JsonConvert.DeserializeObject<TReturnType>(await response.Content.ReadAsStringAsync());

                    #region .NET 3.0 compatible

                    var http = (HttpWebRequest)WebRequest.Create(Connection.GetUri(path).Uri);
                    http.Accept = "application/json";
                    http.ContentType = "application/json";
                    http.Method = "POST";

                    var parsedContent = JsonConvert.SerializeObject(value);
                    var encoding = new ASCIIEncoding();
                    var bytes = encoding.GetBytes(parsedContent);

                    var newStream = http.GetRequestStream();
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();

                    var task = await Task.Factory.FromAsync(
                        http.BeginGetResponse,
                        asyncResult => http.EndGetResponse(asyncResult),
                        null);

                    return JsonConvert.DeserializeObject<TReturnType>(new StreamReader(task.GetResponseStream()).ReadToEnd());

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