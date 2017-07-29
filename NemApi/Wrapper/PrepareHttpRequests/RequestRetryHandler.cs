using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Text;
using CSharp2nem.Connectivity;
using Newtonsoft.Json;

namespace CSharp2nem.PrepareHttpRequests
{
    internal class RequestRetryHandler
    {
        
        HttpConnector HttpConnector { get; }
        
        private int RequestRetries { get; set; }

        private ManualAsyncResult Result { get; set; }

        internal RequestRetryHandler(HttpConnector httpConnector, ManualAsyncResult result)
        {         
            RequestRetries = 0;

            HttpConnector = httpConnector;

            Result = result;
        }

        internal ManualAsyncResult RetryRequest()
        {
            try
            {
                HttpConnector.Con.SetNewHost();

                if(Result.HttpWebRequest.Method == "GET")
                    Result = HttpConnector.RequestGet(Result);

                else if (Result.HttpWebRequest.Method == "Post")
                    Result = HttpConnector.RequestPost(Result);

                Result.TimeOutWait();

                if (Result.Error == null)
                {
                    return Result;
                }

                throw Result.Error;
            }
            catch (Exception)
            {
                RequestRetries++;

                if (RequestRetries < 5) return RetryRequest();
                
                throw new Exception("Too Many Request Failures");
            }
        }
    }
}
