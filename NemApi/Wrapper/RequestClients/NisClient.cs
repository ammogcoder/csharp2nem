using System;
using CSharp2nem.Connectivity;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects.Nis;

namespace CSharp2nem.RequestClients
{
    /// <summary>
    /// Contains a number of methods to retrieve information about a given instance of the nem infrastructure server [Nis]
    /// </summary>
    public class NisClient
    {
        /// <summary>
        /// Constructs an instance of NisClient using a given connection.
        /// </summary>
        /// <param name="connection"></param>
        public NisClient(Connection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            Connection = connection;
        }
        
        /// <summary>
        /// Constructs an instance of NisClient using a default main net connection
        /// </summary>
        public NisClient()
        {
            Connection = new Connection();
        }

        /// <summary>
        /// Gets or sets the connection for the current NisClient instance.
        /// </summary>
        public Connection Connection { get; set; }

        

        #region Getting Nis Heart Beat

        /// <summary>
        /// Determines whether the nis is active and responsive.
        /// </summary>
        /// <param name="callback">The action to be performed upon completion of the asynchronous request</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the request.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetHeartBeat</cref>
        ///     </see>
        ///     method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         NisClient client = new NisClient(connection);
        /// 
        ///         try
        ///         {
        ///             client.BeginGetHeartBeat(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        ///         
        ///                     Console.WriteLine("message: " + obj.Content.Message);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                           
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetHeartBeat(Action<ResponseAccessor<NemRequestResult>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/heartbeat");
            
        }

        /// <summary>
        /// Determines whether the nis is active and responsive.
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the request.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetHeartBeat</cref>
        ///     </see>
        ///     method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         NisClient client = new NisClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetHeartBeat();
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                           
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetHeartBeat()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/heartbeat");
        }

        /// <summary>
        /// Ends the asynchronous request to get the nis heart beat.
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from performing the <see cref="BeginGetHeartBeatAsync"/> method.</param>
        /// <returns>The nis <see cref="NemRequestResult"/></returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetHeartBeat</cref>
        ///     </see>
        ///     method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         NisClient client = new NisClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetHeartBeat();
        /// 
        ///             HeartBeat beat = client.EndGetHeartBeat(result);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                           
        ///     }
        /// }
        /// </code>
        /// </example>
        public NemRequestResult EndGetHeartBeat(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<NemRequestResult>(result);
        }

        #endregion

        #region Getting Nis Status

        /// <summary>
        /// Gets the status of the Nis.
        /// </summary>
        /// <param name="callback">The action to be performed upon completion of the asynchronous request</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the request.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetStatus</cref>
        ///     </see>
        ///     method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         NisClient client = new NisClient(connection);
        /// 
        ///         try
        ///         {
        ///             client.BeginGetStatus(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        ///         
        ///                     Console.WriteLine("message: " + obj.Content.Message);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                           
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetStatus(Action<ResponseAccessor<NemRequestResult>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/status");
        }

        /// <summary>
        /// Gets the status of the Nis.
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the request.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetStatus</cref>
        ///     </see>
        ///     method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         NisClient client = new NisClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetStatus();
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                           
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetStatus()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/status");
        }

        /// <summary>
        /// Ends the asynchronous request to get the status of the Nis.
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the request.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>EndGetStatus</cref>
        ///     </see>
        ///     method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         NisClient client = new NisClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetStatus();
        /// 
        ///             Status stat = client.EndGetStatus(result);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                           
        ///     }
        /// }
        /// </code>
        /// </example>
        public NemRequestResult EndGetStatus(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<NemRequestResult>(result);
        }

        #endregion
    }
}