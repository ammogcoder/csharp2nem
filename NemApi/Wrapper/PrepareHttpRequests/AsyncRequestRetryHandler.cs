using System;

namespace CSharp2nem.PrepareHttpRequests
{
    internal class AsyncRequestRetryHandler
    {
        private HttpAsyncConnector HttpAsyncConnector { get; }       

        internal Action<ResponseAccessor<string>> Callback { get; set; }

        private int RequestRetries { get; set; }

        private ManualAsyncResult Result { get; }

        internal AsyncRequestRetryHandler(HttpAsyncConnector httpConnector, ManualAsyncResult result)
        {
            RequestRetries = 0;

            HttpAsyncConnector = httpConnector;

            Result = result;
        }
        
        internal void SetCallback(Action<ResponseAccessor<string>> callback)
        {
            Callback = callback;
        }
        internal void RetryAsyncRequest()
        {
            try
            {
                
                if (RequestRetries < 5)
                {
                    HttpAsyncConnector.Con.SetNewHost();

                    RequestRetries++;
      
                    if (Result.HttpWebRequest.Method == "GET")
                        HttpAsyncConnector.RequestGetAsync(Callback, Result);

                    if (Result.HttpWebRequest.Method == "Post")
                        HttpAsyncConnector.RequestPostAsync(Callback, Result);
                }
                else
                {
                    Callback(new ResponseAccessor<string>
                    {
                        Ex = Result.Error
                    });

                    Result.m_waitHandle.Set();
                }
            }
            catch (Exception ex)
            {
                Callback(new ResponseAccessor<string>
                {
                    Ex = Result.Error
                });

                Result.m_waitHandle.Set();
            }           
        }
    }
}
