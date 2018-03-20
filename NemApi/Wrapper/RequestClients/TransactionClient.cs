using System;
using CSharp2nem.Connectivity;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects.Transaction;
using CSharp2nem.Utils;

namespace CSharp2nem.RequestClients
{
    /// <summary>
    /// Contains a number of methods to retrieve account transaction information.
    /// </summary>
    public class TransactionDataClient
    {
        /// <summary>
        /// Gets or sets the current connection for this instance.
        /// </summary>
        public Connection Connection { get; set; }

        /// <summary>
        /// Constructs an instance of the TransactionClient using a given connection.
        /// </summary>
        /// <param name="connection">The <see cref="Connectivity.Connection"/> to use for this client.</param>
        /// <exception cref="ArgumentNullException">Connection cannot be null.</exception>
        public TransactionDataClient(Connection connection)
        {
            if(connection == null) throw new ArgumentNullException(nameof(connection));

            Connection = connection;
        }

        /// <summary>
        /// Constructs an instance of the TransactionClient class using a default main net connection.
        /// </summary>
        public TransactionDataClient()
        {
            Connection = new Connection();
        }

#region Getting All Transactions

        /// <summary>
        /// Get all transactions for an account.
        /// </summary>
        /// <param name="callback">The action to be perfomed upon completion of the asynchronous request.</param>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetAllTransactionsAsync</cref>
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
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             client.BeginGetAllTransactionsAsync(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///         
        ///                     Console.WriteLine("4: " + returnedObject.Content?.data.Count);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("4: error: " + e.Message); }
        ///         
        ///             }, "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAllTransactions(Action<ResponseAccessor<Transactions.All>> callback, string address, string hash = null, int id = 0)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/transfers/all";

            var query = id == 0 && hash == null
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address))
                : id == 0 && hash != null
                    ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash)
                    : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash, "&id=", id);
           
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get all transactions for an account.
        /// </summary>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetAllTransactionsAsync</cref>
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
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetAllTransactionsAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAllTransactions(string address, string hash = null, int id = 0)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/transfers/all";

            var query = id == 0 && hash == null
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address))
                : id == 0 && hash != null
                    ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash)
                    : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash, "&id=", id);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }


        #endregion

#region Getting Incoming Transactions

        /// <summary>
        /// Get incoming transactions for an account.
        /// </summary>
        /// <param name="callback">The action to be perfomed upon completion of the asynchronous request.</param>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetIncomingTransactionsAsync</cref>
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
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             client.BeginGetIncomingTransactionsAsync(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///         
        ///                     Console.WriteLine("4: " + returnedObject.Content?.data.Count);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("4: error: " + e.Message); }
        ///         
        ///             }, "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetIncomingTransactions(Action<ResponseAccessor<Transactions.All>> callback, string address, string hash = null, int id = 0)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/transfers/incoming";

            var query = id == 0 && hash == null
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address))
                : id == 0 && hash != null
                    ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash)
                    : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash, "&id=", id);

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get incoming transactions for an account.
        /// </summary>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetIncomingTransactionsAsync</cref>
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
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetIncomingTransactionsAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetIncomingTransactions(string address, string hash = null, int id = 0)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/transfers/incoming";

            var query = id == 0 && hash == null
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address))
                : id == 0 && hash != null
                    ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash)
                    : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash, "&id=", id);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        #endregion

#region Getting Outgoing Transactions

        /// <summary>
        /// Get outgoing transactions for an account.
        /// </summary>
        /// <param name="callback">The action to be perfomed upon completion of the asynchronous request.</param>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetOutgoingTransactionsAsync</cref>
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
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             client.GetOutgoingTransactionsAsync(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///         
        ///                     Console.WriteLine("4: " + returnedObject.Content?.data.Count);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("4: error: " + e.Message); }
        ///         
        ///             }, "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetOutgoingTransactions(Action<ResponseAccessor<Transactions.All>> callback, string address, string hash = null, int id = 0)
        {
            if(address == null) throw  new ArgumentException("address cannot be null");

            const string path = "/account/transfers/outgoing";

            var query = id == 0 && hash == null
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address))
                : id == 0 && hash != null
                    ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash)
                        : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash, "&id=", id);


            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get outgoing transactions for an account.
        /// </summary>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetOutgoinggTransactionsAsync</cref>
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
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetOutgoinggTransactionsAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult GetOutgoingTransactions(string address, string hash = null, int id = 0)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/transfers/outgoing";

            var query = id == 0 && hash == null
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address))
                : id == 0 && hash != null
                    ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash)
                    : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash, "&id=", id);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }
        #endregion

#region Getting Unconfirmed Transactions

        /// <summary>
        /// Get unconfirmed transactions for an account.
        /// </summary>
        /// <param name="callback">The action to be perfomed upon completion of the asynchronous request.</param>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetUnconfirmedTransactionsAsync</cref>
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
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             client.GetUnconfirmedTransactionsAsync(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///         
        ///                     Console.WriteLine("4: " + returnedObject.Content?.data.Count);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("4: error: " + e.Message); }
        ///         
        ///             }, "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetUnconfirmedTransactions(Action<ResponseAccessor<Transactions.All>> callback,
            string address)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/unconfirmedTransactions";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get unconfirmed transactions for an account.
        /// </summary>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>GetUnconfirmedTransactionsAsync</cref>
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
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.GetUnconfirmedTransactionsAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetUnconfirmedTransactions(string address, string hash = null, int id = 0)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/unconfirmedTransactions";

            var query = id == 0 && hash == null
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address))
                : id == 0 && hash != null
                    ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash)
                    : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash, "&id=", id);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }
        #endregion

        /// <summary>
        /// Ends the asynchronous request to get transactions
        /// </summary>
        /// <remarks>
        /// Ends all transaction requests in the transaction client that do not have callbacks.
        /// </remarks>
        /// <param name="result">The <see cref="ManualAsyncResult"/> for the request to be ended.</param>
        /// <returns>The <see cref="Transactions.All"/> instance containing the requested transactions</returns>
        /// <example> 
        /// This sample shows how to use the BeginGetXTransaction methods.
        /// 
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         TransactionClient client = new TransactionClient(connection);
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.GetUnconfirmedTransactionsAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///             
        ///             Transactions.All transactions = client.EndGetTransactions(result);
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public Transactions.All EndGetTransactions(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<Transactions.All>(result);
        }
    }
}
