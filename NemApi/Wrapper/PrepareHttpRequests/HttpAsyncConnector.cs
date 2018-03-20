using CSharp2nem.Connectivity;
using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CSharp2nem.PrepareHttpRequests
{
    internal class HttpAsyncConnector
    {

        internal HttpAsyncConnector(Connection con)
        {
            Con = con;
        }
        internal Connection Con { get; set; }

        private ManualAsyncResult SetUpHttpWebRequest(ManualAsyncResult result, string requestType, int timeout)
        {
            var http = (HttpWebRequest)WebRequest.Create(Con.GetUri(result.Path, result.Query).Uri);
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = requestType;
            http.ReadWriteTimeout = timeout;
            http.Timeout = timeout;
           
            result.HttpWebRequest = http;
       
            return result;
        }

        internal ManualAsyncResult RequestGetAsync(Action<ResponseAccessor<string>> callback, ManualAsyncResult result)
        {                  
            try
            {          
                result.HttpWebRequest.BeginGetResponse(result.WrapHandler(ar =>
                {                   
                    try
                    {         
                        var responseStream = result.HttpWebRequest.EndGetResponse(ar);
                        
                        using (var response = responseStream.GetResponseStream())
                        {
                            ReadStream(callback, result, response, 0);
                        }                    
                    }
                    catch (Exception ex)
                    {
                        result.Error = ex;

                        if (Con.AutoHost)
                            result.AsyncRequestRetryHandler.RetryAsyncRequest(); 
                    }
                }), null);
            }
            catch (Exception ex)
            {
                result.Error = ex;

                if (Con.AutoHost)
                    result.AsyncRequestRetryHandler.RetryAsyncRequest();   
            }
            return result;
        }

        internal ManualAsyncResult PrepareGetRequestAsync<TReturnType>(Action<ResponseAccessor<TReturnType>> callback, string path, string query = null)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = path,
                Query = query,
            };

            asyncResult = SetUpHttpWebRequest(asyncResult, "GET", 5000);

            asyncResult.AsyncRequestRetryHandler = new AsyncRequestRetryHandler(this, asyncResult);

            try
            {
                asyncResult.AsyncRequestRetryHandler.SetCallback(body =>
                {
                    if (body.Ex != null)
                    {
                        callback(new ResponseAccessor<TReturnType>
                        {
                            Ex = body.Ex
                        });
                    }
                    else
                    {
                        callback(new ResponseAccessor<TReturnType>
                        {
                            Content = JsonConvert.DeserializeObject<TReturnType>(body.Content),
                        });
                    }
                });

                asyncResult = RequestGetAsync(asyncResult.AsyncRequestRetryHandler.Callback, asyncResult);
            }
            catch (Exception ex)
            {
                callback(new ResponseAccessor<TReturnType>
                {
                    Ex = ex
                });
            }

            return asyncResult;
        }

        internal ManualAsyncResult RequestPostAsync(Action<ResponseAccessor<string>> callback, ManualAsyncResult result)
        {      
            try
            {
               
                result.HttpWebRequest.BeginGetRequestStream(result.WrapHandler(ar =>
                {
                   
                    try
                    {
                       
                        var stream = result.HttpWebRequest.EndGetRequestStream(ar);
                   
                        stream.Write(result.Bytes, 0, result.Bytes.Length);
                       
                        result.HttpWebRequest.BeginGetResponse(result.WrapHandler(ar2 =>
                        {
                          
                            try
                            {
                                var responseStream = result.HttpWebRequest.EndGetResponse(ar2);
                               
                                using (var response = responseStream.GetResponseStream())
                                {
                                    ReadStream(callback, result, response, 0);
                                }
                            }
                            catch (Exception ex)
                            {
                               
                                callback(new ResponseAccessor<string>()
                                {
                                    Ex = ex
                                });
                            }
                                                                                               
                        }), null);       
                    }
                    catch (Exception ex)
                    {
                        result.Error = ex;
                    
                        if (Con.AutoHost)
                            result.AsyncRequestRetryHandler.RetryAsyncRequest();
                    }
                }), null);
            }
            catch (Exception ex)
            {
                result.Error = ex;
                
                if (Con.AutoHost)
                    result.AsyncRequestRetryHandler.RetryAsyncRequest();
            }

            return result;
        }
        internal ManualAsyncResult PreparePostRequestAsync<TReturnType>(Action<ResponseAccessor<TReturnType>> callback, ManualAsyncResult asyncResult)
        {
            asyncResult = SetUpHttpWebRequest(asyncResult, "Post", 5000);

            asyncResult.AsyncRequestRetryHandler = new AsyncRequestRetryHandler(this, asyncResult);

            try
            {
           
                asyncResult.AsyncRequestRetryHandler.SetCallback(body =>
                {

                    if (body.Ex != null)
                    {                
                        callback(new ResponseAccessor<TReturnType>
                        {
                            Ex = body.Ex
                        });
                    }
                    else callback(new ResponseAccessor<TReturnType>
                    {
                        Content = JsonConvert.DeserializeObject<TReturnType>(body.Content),
                    });
                });

                RequestPostAsync(asyncResult.AsyncRequestRetryHandler.Callback, asyncResult);
            }
            catch (Exception e)
            {               
                callback(new ResponseAccessor<TReturnType>
                {
                    Ex = e
                });
            }

            return asyncResult;
        }


        private void ReadStream(Action<ResponseAccessor<string>> callback, ManualAsyncResult result, Stream stream, int len)
        {
            var done = false;

            var buffer = new byte[20000];
              
            do
            {             
                try
                {
                    var innerResult = new ManualAsyncResult();
                    
                    stream.BeginRead(buffer, len, 20000, result.WrapHandler(innerResult.WrapHandler(ar =>
                    {
                        try
                        {                                               
                            var readLen = stream.EndRead(ar);

                            if (readLen == 0)
                            {
                                stream.Close();
                               
                                done = true;
                                
                                callback(new ResponseAccessor<string>
                                {
                                    Content = Encoding.UTF8.GetString(buffer, 0, len)
                                });                     
                            }    
                                                 
                            var temp = buffer;

                            Array.Resize(ref temp, buffer.Length + readLen);

                            buffer = temp;

                            len += readLen;                                                     
                        }
                        catch (Exception ex)
                        {
                            done = true;

                            stream?.Close();

                            callback(new ResponseAccessor<string>
                            {
                                
                                Ex = ex
                            });                          
                        }
                    })), null);

                    innerResult.AsyncWaitHandle.WaitOne();
                }
                catch (Exception ex)
                {
                    callback(new ResponseAccessor<string>
                    {
                        Ex = ex
                    });
                }         
            } while (!done);
        }
    }
}
