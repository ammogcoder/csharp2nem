using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using CSharp2nem.Connectivity;

namespace CSharp2nem.PrepareHttpRequests
{
    internal class HttpConnector
    {
        internal HttpConnector(Connection con)
        {
            Con = con;
        }
        internal Connection Con { get; }

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

        internal TReturnType CompleteGetRequest<TReturnType>(ManualAsyncResult result)
        {
            var response = EndGetRequest<TReturnType>(result);

            if (response.Ex != null)
                throw response.Ex;

            return response.Content;
        }
        internal TReturnType CompletePostRequest<TReturnType>(ManualAsyncResult result)
        {
            var response = EndPostRequest<TReturnType>(result);
            
            if (response.Ex != null)
                throw response.Ex;

            return response.Content;
        }
        private ResponseAccessor<TReturnType> EndPostRequest<TReturnType>(ManualAsyncResult result)
        {
            try
            {
               
                result.TimeOutWait();
               
                if (result.Error != null)
                {

                    if (Con.AutoHost) result = result.RequestRetryHandler.RetryRequest();
                }
              
                Stream responseStream = null;
                
                var temp = new ManualAsyncResult();               
               
                result.HttpWebRequest.BeginGetResponse(temp.WrapHandler(ar =>
                {                  
                    try
                    {
                        var response = result.HttpWebRequest.EndGetResponse(ar);
                        
                        responseStream = response.GetResponseStream();        
                    }
                    catch (Exception ex)
                    {
                        result.Error = ex;
                    }          
                }), null);

                temp.TimeOutWait();

                if (result.Error != null)
                {
                    return new ResponseAccessor<TReturnType>()
                    {
                        Ex = result.Error
                    };
                }

                var responseAccessor = ReadStream(result, responseStream, 0);
      
                return new ResponseAccessor<TReturnType>()
                {
                    Content = JsonConvert.DeserializeObject<TReturnType>(responseAccessor.Content),
                };
            }
            catch (Exception ex)
            {
                return new ResponseAccessor<TReturnType>()
                {
                    Ex = ex
                };
            }
        }

        private ResponseAccessor<TReturnType> EndGetRequest<TReturnType>(ManualAsyncResult result)
        {
            try
            {
                result.TimeOutWait();

                if (result.Error != null)
                {
                    if (Con.AutoHost) result = result.RequestRetryHandler.RetryRequest();
                }
               
                var responseStream = result.Response.GetResponseStream();
                
                var responseAccesor = ReadStream(result, responseStream, 0);

                return new ResponseAccessor<TReturnType>()
                {
                    Content = JsonConvert.DeserializeObject<TReturnType>(responseAccesor.Content),
                };
            }
            catch (Exception ex)
            {
                return new ResponseAccessor<TReturnType>()
                {
                    Ex = ex
                };
            }
        }

        // ----------------------- get ----------------------- //
        
        internal ManualAsyncResult RequestGet(ManualAsyncResult result)
        {           
            try
            {
                result.HttpWebRequest.BeginGetResponse(result.WrapHandler(ar =>
                {
                    result.Response = result.HttpWebRequest.EndGetResponse(ar);

                }), null);
            }
             
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }   

        internal ManualAsyncResult PrepareGetRequest(string path, string query = null)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = path,
                Query = query,
            };

            asyncResult = SetUpHttpWebRequest(asyncResult, "GET", 5000);

            asyncResult.RequestRetryHandler = new RequestRetryHandler(this, asyncResult);

            try
            {
                 asyncResult = RequestGet(asyncResult);
            }
            catch (Exception ex)
            {
                asyncResult.Error = ex;
            }

            return asyncResult;
        }

        // ----------------------- post ---------------------- //
       
        internal ManualAsyncResult RequestPost(ManualAsyncResult result)
        {                  
            try
            {               
                result.HttpWebRequest.BeginGetRequestStream(result.WrapHandler(ar =>
                {
                    try
                    {
                        var stream = result.HttpWebRequest.EndGetRequestStream(ar);

                        stream.Write(result.Bytes, 0, result.Bytes.Length);
                    }
                    catch (Exception ex)
                    {
                        result.Error = ex;
                    }

                }), null);
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }     

        internal ManualAsyncResult PreparePostRequest(ManualAsyncResult asyncResult)
        {                  
            asyncResult = SetUpHttpWebRequest(asyncResult, "Post", 5000);

            asyncResult.RequestRetryHandler = new RequestRetryHandler(this, asyncResult);

            try
            {
                asyncResult = RequestPost(asyncResult);
            }
            catch (Exception ex)
            {
                asyncResult.Error = ex;
            }


            return asyncResult;
        }

        // ------------------------- read -----------------------//
       
        private ResponseAccessor<string> ReadStream(ManualAsyncResult result, Stream stream, int len)
        {          
            var done = false;

            var buffer = new byte[20000];

            var response = new ResponseAccessor<string>();
            
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
                               
                                response = new ResponseAccessor<string>
                                {
                                    Content = Encoding.UTF8.GetString(buffer, 0, len)
                                };
                               
                                innerResult.m_waitHandle.Set();
                                return;
                            }
                  
                            var temp = buffer;

                            Array.Resize(ref temp, buffer.Length + readLen);

                            buffer = temp;

                            len += readLen;
                        }
                        catch (Exception ex)
                        {
                            done = true; 

                            response = new ResponseAccessor<string>
                            {
                                Ex = ex
                            };

                            innerResult.m_waitHandle.Set();

                            stream?.Close();
                        }
                    })), null);
                  
                    innerResult.AsyncWaitHandle.WaitOne();
                   
                }
                catch (Exception ex)
                {
                    response = new ResponseAccessor<string>
                    {
                        Ex = ex
                    };

                    done = true;
                }
            } while (!done);
           
            return response;
        }
    }
}
