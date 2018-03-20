using System;
using CSharp2nem.Connectivity;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects.Debug;
using CSharp2nem.ResponseObjects.Nis;

namespace CSharp2nem.RequestClients
{
    /// <summary>
    /// Contains methods to get debug information from nis
    /// </summary>
    public class DebugClient
    {
        /// <summary>
        /// Gets or sets the connection for this instance
        /// </summary>
        public Connection Connection { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="DebugClient"/> class using a given connection.
        /// </summary>
        /// <param name="connection">The <see cref="Connection"/> to use for the client</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DebugClient(Connection connection)
        {
            if(connection == null) throw new ArgumentNullException(nameof(connection));

            Connection = connection;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="DebugClient"/> class using a default main net connection.
        /// </summary>
        public DebugClient()
        {
            Connection = new Connection();

        }

        #region Getting Debug Time Synchronisation

        /// <summary>
        /// Gets the time syncronisation debug infortmation
        /// </summary>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetChainScoreAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetTimeSyncAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("4 " + obj.Content.Data[0].DateTime);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("4: error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("4: error: " + ex.Message); }                       
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetTimeSync(Action<ResponseAccessor<TimeSync>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/debug/time-synchronization");    
        }

        /// <summary>
        /// Gets the time syncronisation debug infortmation
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetChainScoreAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetTimeSyncAsync();
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("4: error: " + ex.Message); }                       
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetTimeSync()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/debug/time-synchronization");
        }

        /// <summary>
        /// Ends the asynchronous request for debug time synchronisation information.
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from performing the <see cref="BeginGetTimeSyncAsync"/> method.</param>
        /// <returns>The <see cref="TimeSync"/> debug information </returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetTimeSyncAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetTimeSyncAsync();
        /// 
        ///             TimeSync data = client.EndGetTimeSync(responseResult);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("4: error: " + ex.Message); }                       
        ///     }
        /// }
        /// </code>
        /// </example>
        public TimeSync EndGetTimeSync(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<TimeSync>(result);
        }

        #endregion

        #region Getting Outgoing Connections

        /// <summary>
        /// Gets debug information about outgoing connections.
        /// </summary>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetOutgoingConnectionsAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetOutgoingConnectionsAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("host: " + obj.Content.MostRecent[0].host);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }         
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetOutgoingConnections(Action<ResponseAccessor<AuditCollection.Connections>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/debug/connections/outgoing");
        }

        /// <summary>
        /// Gets debug information about outgoing connections.
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetOutgoingConnectionsAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetOutgoingConnectionsAsync();
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }         
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetOutgoingConnections()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/debug/connections/outgoing");
        }


        /// <summary>
        /// Ends the asynchronous request to get outgoing connection information.
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from intiating the <see cref="BeginGetOutgoingConnectionsAsync"/> method.</param>
        /// <returns>The outgoing <see cref="AuditCollection.Connections"/> for the node.</returns>
        /// /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>EndGetOutgoingConnections</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetOutgoingConnectionsAsync();
        /// 
        ///             DebugConnections.Connections connections = client.EndGetOutgoingConnections
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }         
        ///     }
        /// }
        /// </code>
        /// </example>
        public AuditCollection.Connections EndGetOutgoingConnections(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<AuditCollection.Connections>(result);
        }

        #endregion

        #region Getting Incomming Connections

        /// <summary>
        /// Gets debug information about incoming connections.
        /// </summary>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetIncomingConnectionsAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetIncomingConnectionsAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("host: " + obj.Content.MostRecent[0].host);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }         
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetIncomingConnections(Action<ResponseAccessor<AuditCollection.Connections>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/debug/connections/incoming");
        }

        /// <summary>
        /// Gets debug information about incoming connections.
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetIncomingConnectionsAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetIncomingConnectionsAsync();
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }         
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetIncomingConnections()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/debug/connections/incoming");
        }

        /// <summary>
        /// Ends the asynchronous request to get incoming connection information.
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from intiating the <see cref="BeginGetIncomingConnectionsAsync"/> method.</param>
        /// <returns>The outgoing <see cref="AuditCollection.Connections"/> for the node.</returns>
        /// /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>EndGetIncomingConnections</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetIncomingConnectionsAsync();
        /// 
        ///             DebugConnections.Connections connections = client.EndGetIncomingConnections
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }         
        ///     }
        /// }
        /// </code>
        /// </example>
        public AuditCollection.Connections EndGetIncomingConnection(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<AuditCollection.Connections>(result);
        }

        #endregion

        #region Getting Connection Timers

        /// <summary>
        /// Gets connection timer information about the currently connected node
        /// </summary>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetConnectionTimersAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetConnectionTimersAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("average time: " + obj.Content.data[0].averageOperationTime);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetConnectionTimersAsync(Action<ResponseAccessor<DebugTimers.Timers>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/debug/timers");  
        }

        /// <summary>
        /// Gets connection timer information about the currently connected node
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetConnectionTimersAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetConnectionTimersAsync();
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetConnectionTimersAsync()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/debug/timers");
        }

        /// <summary>
        /// Ends the asynchronous request to get connection timers
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned for initiating the <see cref="BeginGetConnectionTimersAsync"/> method.</param>
        /// <returns>The <see cref="DebugTimers.Timers"/> for the current connection.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetConnectionTimersAsync</cref>
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
        ///         DebugClient client = new DebugClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetConnectionTimersAsync();
        /// 
        ///             DebugTimers.Timers timers = client.EndGetConnectionTimers(responseResult);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }
        /// }
        /// </code>
        /// </example>
        public DebugTimers.Timers EndGetConnectionTimers(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<DebugTimers.Timers>(result);
        }

        #endregion
    }
}
