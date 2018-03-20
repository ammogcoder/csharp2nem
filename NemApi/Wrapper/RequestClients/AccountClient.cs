using System;
using CSharp2nem.Connectivity;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects.Account;
using CSharp2nem.Utils;

namespace CSharp2nem.RequestClients
{
    /// <summary>
    /// A request client used to query account related information.
    /// </summary>
    public class AccountClient
    {
        /// <summary>
        /// The connection to use for this client.
        /// </summary>
        public Connection Connection { get; set; }


        /// <summary>
        /// Constructs an <see cref="AccountClient"/> with using given <see cref="Connection"/>.
        /// </summary>
        /// <param name="connection">The connection instance to use for the client</param>
        public AccountClient(Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountClient"/> class.
        /// </summary>
        /// <remarks>
        /// Uses a defualt main net connection.
        /// </remarks>
        public AccountClient()
        {
            Connection = new Connection();
        }     

        #region Getting Account Info

        /// <summary>
        /// Get account information for an account.
        /// </summary>
        /// <param name="callback">The action to perform upon completion of the request.</param>
        /// <param name="address">The account address to get information about.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom IAsyncResult from perfomring the request.</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="BeginGetAccountInfoFromAddress"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         AccountClient client = new AccountClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetAccountInfoFromAddress(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        /// 
        ///                     Console.WriteLine("6 callback: " + returnedObject.Content.Account.Address);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("6 calback: error: " + e.Message); }
        /// 
        ///             }, "TCG5JI-RN4V24-NH63AY-54ACGP-UOEO7K-KYHKNW-H32N");
        ///         }
        ///         catch (Exception ex) 
        ///         { 
        ///              Console.WriteLine("6 callback: error: " + ex.Message); 
        ///         }                                                                  
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountInfoFromAddress(Action<ResponseAccessor<ExistingAccount.Data>> callback, string address)
        {
            const string path = "/account/get";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get account information for an account.
        /// </summary>
        /// <param name="callback">The action to perform upon completion of the request.</param>
        /// <param name="publicKey">The account public key to get information about</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom IAsyncResult from perfomring the request.</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="BeginGetAccountInfoFromPublicKey"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         AccountClient client = new AccountClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetAccountInfoFromPublicKey(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        /// 
        ///                     Console.WriteLine("6 callback: " + returnedObject.Content.Account.Address);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("6 calback: error: " + e.Message); }
        /// 
        ///             }, "09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         }
        ///         catch (Exception ex) 
        ///         { 
        ///              Console.WriteLine("6 callback: error: " + ex.Message); 
        ///         }                                                                  
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountInfoFromPublicKey(Action<ResponseAccessor<ExistingAccount.Data>> callback, string publicKey)
        {
           
            if (!StringUtils.OnlyHexInString(publicKey) || publicKey.Length != 64)
                throw new ArgumentException("invalid public key");

            const string path = "/account/get/from-public-key";

            var query = string.Concat("publicKey=", publicKey);

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get account information from a given address
        /// </summary>
        /// <param name="address">The account address for which information should be retrieved</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="BeginGetAccountInfoFromAddress"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetAccountInfoFromAddress("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex) 
        ///         { 
        ///              Console.WriteLine("6 callback: error: " + ex.Message);                              
        ///        }      
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountInfoFromAddress(string address)
        {
            const string path = "/account/get";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Get account information from a given public key
        /// </summary>
        /// <param name="publicKey">The account public key for which information should be retrieved</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="BeginGetAccountInfoFromPublicKey"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetAccountInfoFromPublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         }
        ///         catch (Exception ex) 
        ///         { 
        ///              Console.WriteLine("6 callback: error: " + ex.Message);                              
        ///        }      
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountInfoFromPublicKey(string publicKey)
        {
            if (!StringUtils.OnlyHexInString(publicKey) || publicKey.Length != 64)
                throw new ArgumentException("invalid public key");

            const string path = "/account/get/from-public-key";

            var query = string.Concat("publicKey=", publicKey);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Ends an asynchronous request to get account information from a given public key
        /// </summary>
        /// <remarks>
        /// This method is used to get the result from all of the BeginGetAccountInfoFromX methods except the overload that allows for a callback.
        /// </remarks>
        /// <param name="result">The <see cref="ManualAsyncResult"/> result from any of the GetAccountFrom methods</param>
        /// <returns>A <see cref="ExistingAccount.Data "/> returned from the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="EndGetAccountInfo"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetAccountInfoFromPublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///        
        ///             ExistingAccount.Data data = client.EndGetAccountInfo(result);
        ///         }
        ///         catch (Exception ex) 
        ///         { 
        ///              Console.WriteLine("6 callback: error: " + ex.Message);                              
        ///        }      
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ExistingAccount.Data EndGetAccountInfo(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<ExistingAccount.Data>(result);
        }

        #endregion

        #region Getting Account Root Info

        /// <summary>
        /// Get the root account information from a given delegated account address.
        /// </summary>
        /// /// <param name="callback">The action to perform upon completion of the request.</param>
        /// <param name="address">The delegated account address for which root information should be retrieved</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetAccountRootFromAddress</cref>
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
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///             client.BeginGetAccountRootFromAddress(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        /// 
        ///                     Console.WriteLine("callback: " + returnedObject.Content.Account.Address);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("callback: error: " + e.Message); }
        /// 
        ///             }, "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountRootFromAddress(Action<ResponseAccessor<AccountForwarded.Data>> callback, string address)
        {
            const string path = "/account/get/forwarded";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get the root account information from a given delegated account public key.
        /// </summary>
        /// /// <param name="callback">The action to perform upon completion of the request.</param>
        /// <param name="publicKey">The delegated account public key for which root information should be retrieved</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetAccountRootFromPublicKey</cref>
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
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///             client.BeginGetAccountRootFromPublicKey(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        /// 
        ///                     Console.WriteLine("callback: " + returnedObject.Content.Account.Address);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("callback: error: " + e.Message); }
        /// 
        ///             }, "09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountRootFromPublicKey(Action<ResponseAccessor<AccountForwarded.Data>> callback, string publicKey)
        {
            if (!StringUtils.OnlyHexInString(publicKey))
                throw new Exception("invalid public key");

            var path = "/account/get/forwarded/from-public-key";

            var query = string.Concat("publicKey=", publicKey);

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get the root account information from a given delegated account address.
        /// </summary>
        /// <param name="address">The delegated account address for which root information should be retrieved</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetAccountRootFromAddress</cref>
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
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///            ManualAsyncResult requestResponse = client.BeginGetAccountRootFromAddress("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountRootFromAddress(string address)
        {
            const string path = "/account/get/forwarded";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Get the root account information from a given delegated account public key.
        /// </summary>
        /// /// <param name="callback">The action to perform upon completion of the request.</param>
        /// <param name="publicKey">The delegated account public key for which root information should be retrieved</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetAccountRootFromPublicKey</cref>
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
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///            ManualAsyncResult requestResponse = client.BeginGetAccountRootFromPublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountRootFromPublicKey(string publicKey)
        {
            if (!StringUtils.OnlyHexInString(publicKey))
                throw new Exception("invalid public key");

            const string path = "/account/get/forwarded/from-public-key";

            var query = string.Concat("publicKey=", publicKey);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Ends an asynchronous request to get the account root information from a given public key or address
        /// </summary>
        /// <remarks>
        /// This method is used to get the result from all of the BeginGetAccountRootFromX methods except the overload that allows for a callback.
        /// </remarks>
        /// <param name="result">The <see cref="ManualAsyncResult"/> result from any of the BeginGetAccountRootFromX methods</param>
        /// <returns>A <see cref="AccountForwarded.Data"/> returned from the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="EndGetAccountRoot"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///          try
        ///          {
        ///             ManualAsyncResult requestResponse = client.BeginGetAccountRootFromPublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///          
        ///             AccountForwarded.Data accountData = client.EndGetAccountRoot(requestResponse);
        /// 
        ///           }
        ///          catch (Exception ex) { Console.WriteLine("error: " + ex.Message); } 
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public AccountForwarded.Data EndGetAccountRoot(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<AccountForwarded.Data>(result);
        }

        #endregion

        #region Getting Account Status

        /// <summary>
        /// Get the status of a given account.
        /// </summary>
        /// <param name="callback">The action to perform upon completion of the request.</param>
        /// <param name="address">The account address for which status information should be retrieved</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetAccountStatus</cref>
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
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///             client.BeginGetAccountStatus(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        /// 
        ///                     else Console.WriteLine("3 callback: " + returnedObject.Content.status);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        /// 
        ///             }, "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountStatus(Action<ResponseAccessor<AccountStatus>> callback, string address)
        {
            const string path = "/account/status";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get the status of a given account.
        /// </summary>
        /// <param name="address">The account address for which status information should be retrieved</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetAccountStatus</cref>
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
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///         try
        ///         {
        ///            ManualAsyncResult requestResponse = client.BeginGetAccountStatus("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetAccountStatus(string address)
        {
            const string path = "/account/status";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Ends an asynchronous request to get the status of a given account.
        /// </summary> 
        /// <param name="result">The <see cref="ManualAsyncResult"/> result from the BeginGetAccountStatus method.</param>
        /// <returns>A <see cref="AccountStatus"/> returned from the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="EndGetAccountStatus"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///          try
        ///          {
        ///             ManualAsyncResult requestResponse = client.BeginGetAccountRootFromPublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///          
        ///             AccountForwarded.Data accountData = client.EndGetAccountRoot(requestResponse);
        /// 
        ///           }
        ///          catch (Exception ex) { Console.WriteLine("error: " + ex.Message); } 
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public AccountStatus EndGetAccountStatus(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<AccountStatus>(result);
        }

        #endregion

        #region Getting Account Harvesting Info

        ///  <summary>
        ///  Get the harvesting information for an account.
        ///  </summary>
        ///  <param name="callback">The action to perform upon completion of the request.</param>
        ///  <param name="address">The account address for which status information should be retrieved</param>
        /// <param name="id">The id of the block up to which harvested blocks are returned.</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        ///  <example> 
        ///  This sample shows how to use the <see>
        ///          <cref>BeginGetHarvestingInfo</cref>
        ///      </see>
        ///      method.
        ///  <code>
        ///  class TestClass 
        ///  {
        ///      static void Main() 
        ///      {         
        ///          Connection connection = new Connection();
        ///          
        ///          connection.SetTestnet();
        ///  
        ///          AccountClient client = new AccountClient(connection);
        ///                   
        ///          try
        ///          {
        ///              client.BeginGetHarvestingInfo(returnedObject => 
        ///              {
        ///                  try
        ///                  {
        ///                      if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///  
        ///                      else Console.WriteLine(returnedObject.Content.data[0].timeStamp);
        ///                  }
        ///                  catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        /// 
        ///              }, "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");             
        ///          }
        ///          catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///      }                                                       
        ///  }
        ///  </code>
        ///  </example>
        public ManualAsyncResult BeginGetHarvestingInfo(Action<ResponseAccessor<HarvestingData.ListData>> callback, string address, string id = null)
        {

            const string path = "/account/harvests";

            var query = id == null ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address)) : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&id=", id);

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        ///  <summary>
        ///  Starts an asynchronous request to get the harvesting information for an account.
        ///  </summary>
        ///  <param name="address">The account address for which status information should be retrieved</param>
        /// <param name="hash">The hash of the block up to which harvested blocks are returned.</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        ///  <example> 
        ///  This sample shows how to use the <see>
        ///          <cref>BeginGetHarvestingInfo</cref>
        ///      </see>
        ///      method.
        ///  <code>
        ///  class TestClass 
        ///  {
        ///      static void Main() 
        ///      {         
        ///          Connection connection = new Connection();
        ///          
        ///          connection.SetTestnet();
        ///  
        ///          AccountClient client = new AccountClient(connection);
        ///                   
        ///          try
        ///          {
        /// 
        ///              manualAsyncResult requestResult = client.BeginGetHarvestingInfo("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT", "09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");  
        ///             
        ///          }
        ///          catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///      }                                                       
        ///  }
        ///  </code>
        ///  </example>
        public ManualAsyncResult BeginGetHarvestingInfo(string address, string hash = null)
        {
            if (null == hash)
            {
                var block = new BlockClient(Connection);

                block.BeginGetLastBlock(blockResult => {
                    hash = blockResult.Content.PrevBlockHash.Data;
                }).AsyncWaitHandle.WaitOne(500);
            }

            if (!StringUtils.OnlyHexInString(hash))
                throw new ArgumentException("invalid hash");

            const string path = "/account/harvests";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&hash=", hash);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Ends an asynchronous request to get the account root information from a given public key or address
        /// </summary>
        /// <remarks>
        /// This method is used to get the result from the BeginGetHarvestingInfo method.
        /// </remarks>
        /// <param name="result">The <see cref="ManualAsyncResult"/> result from the BeginGetHarvestingInfo method</param>
        /// <returns>A <see cref="HarvestingData.ListData"/> returned from the reqeust</returns>
        /// <example> 
        /// This sample shows how to use the <see cref="EndGetHarvestingInfo"/> method.
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main() 
        ///     {         
        ///         Connection connection = new Connection();
        ///         
        ///         connection.SetTestnet();
        /// 
        ///         AccountClient client = new AccountClient(connection);
        ///                  
        ///          try
        ///          {
        ///             ManualAsyncResult requestResponse = client.BeginGetHarvestingInfo("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///          
        ///             AccountForwarded.Data accountData = client.EndGetHarvestingInfo(requestResponse);
        /// 
        ///           }
        ///          catch (Exception ex) { Console.WriteLine("error: " + ex.Message); } 
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public HarvestingData.ListData EndGetHarvestingInfo(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<HarvestingData.ListData>(result);
        }

        #endregion

        #region Getting Importances


        ///  <summary>
        ///  Get the importances information.
        ///  </summary>
        ///  <param name="callback">The action to perform upon completion of the request.</param>
        /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        ///  <example> 
        ///  This sample shows how to use the <see>
        ///          <cref>BeginGetImportances</cref>
        ///      </see>
        ///      method.
        ///  <code>
        ///  class TestClass 
        ///  {
        ///      static void Main() 
        ///      {         
        ///          Connection connection = new Connection();
        ///          
        ///          connection.SetTestnet();
        ///  
        ///          AccountClient client = new AccountClient(connection);
        ///                   
        ///          try
        ///          {
        ///              client.BeginGetImportances(returnedObject => 
        ///              {
        ///                  try
        ///                  {
        ///                      if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///  
        ///                      else Console.WriteLine("callback: " + returnedObject.Content.Data[0].Address);
        ///                  }
        ///                  catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        /// 
        ///              });             
        ///          }
        ///          catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///      }                                                       
        ///  }
        ///  </code>
        ///  </example>
        public ManualAsyncResult BeginGetImportances(Action<ResponseAccessor<Importances.ListImportances>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/account/importances");
        }

        ///  <summary>
        ///  Starts an asynchronous request to get the importances information.
        ///  </summary>
         /// <returns>A <see cref="ManualAsyncResult"/> for the reqeust</returns>
        ///  <example> 
        ///  This sample shows how to use the <see>
        ///          <cref>BeginGetImportances</cref>
        ///      </see>
        ///      method.
        ///  <code>
        ///  class TestClass 
        ///  {
        ///      static void Main() 
        ///      {         
        ///          Connection connection = new Connection();
        ///          
        ///          connection.SetTestnet();
        ///  
        ///          AccountClient client = new AccountClient(connection);
        ///                   
        ///          try
        ///          {
        ///              ManualAsyncResult requestResponse = client.BeginGetImportances();  
        ///                 
        ///          }
        ///          catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///      }                                                       
        ///  }
        ///  </code>
        ///  </example>
        public ManualAsyncResult BeginGetImportances()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/account/importances");           
        }

        ///  <summary>
        ///  Ends an asynchronous request to get the importances information.
        ///  </summary>
        /// <returns>An <see cref=" Importances.ListImportances"/> object</returns>
        ///  <example> 
        ///  This sample shows how to use the <see>
        ///          <cref>EndGetImportances</cref>
        ///      </see>
        ///      method.
        ///  <code>
        ///  class TestClass 
        ///  {
        ///      static void Main() 
        ///      {         
        ///          Connection connection = new Connection();
        ///          
        ///          connection.SetTestnet();
        ///  
        ///          AccountClient client = new AccountClient(connection);
        ///                   
        ///          try
        ///          {
        ///              ManualAsyncResult requestResponse = client.BeginGetImportances();  
        ///                 
        ///              Importances.ListImportances data = client.EndGetImportances(requestResponse);
        ///             
        ///          }
        ///          catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }
        ///      }                                                       
        ///  }
        ///  </code>
        ///  </example>
        public Importances.ListImportances EndGetImportances(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<Importances.ListImportances>(result);
        }

        #endregion

        #region Getting Account Historical Data

        /// <summary>
        /// Get historical data for an account
        /// </summary>
        /// <remarks>
        /// The configuration for NIS offers the possibility for a node to expose additional features that other nodes don't want to offer. One of those features is the supply of historical account data like balance and importance information. To turn on this feature for your NIS, you need to add HISTORICAL_ACCOUNT_DATA to the list of optional features in the file config.properties.
        /// </remarks> todo: get nodes.
        /// <param name="callback"></param>
        /// <param name="address">The account for which historical data should be returned</param>
        /// <param name="start">The block at which to start returning data</param>
        /// <param name="end"> The end block for which to return data</param>
        /// <param name="increment">The increment of blocks at which data should be returned</param>
        /// <returns>A<see cref="ManualAsyncResult"/> for the reqeust</returns>
        ///  <example> 
        ///  This sample shows how to use the <see>
        ///          <cref>BeginGetHistoricData</cref>
        ///      </see>
        ///      method.
        ///  <code>
        ///  class TestClass 
        ///  {
        ///      static void Main() 
        ///      {         
        ///          Connection connection = new Connection();
        ///          
        ///          connection.SetTestnet();
        ///  
        ///          AccountClient client = new AccountClient(connection);
        ///                
        ///           try
        ///          {
        ///              client.Connection.SetHost("34.211.28.210"); // node that accepts historical requests at time of writing
        /// 
        ///              client.BeginGetHistoricData(returnedObject =>
        ///              {
        ///                  try
        ///                  {
        ///                      if (returnedObject.Ex != null) throw returnedObject.Ex;
        /// 
        ///                      Console.WriteLine("callback: "+returnedObject.Content.Height);
        ///                  }
        ///                  catch (Exception e) { Console.WriteLine("callback: error: " + e.Message); }
        /// 
        ///              }, "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT", 100, 200, 1);
        ///          }
        ///          catch (Exception ex) { Console.WriteLine(" error: " + ex.Message); }                          
        ///      }                                                       
        ///  }
        ///  </code>
        ///  </example>
        public ManualAsyncResult BeginGetHistoricalData(Action<ResponseAccessor<HistoricData>> callback, string address, long start, long end, int increment)
        {
            const string path = "/account/historical/get";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&startHeight=", start, "&endHeight=", end, "&increment=", increment);

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get historical data for an account
        /// </summary>
        /// <remarks>
        /// This request requires the use of a node that allows for historical requests. Support for this request can be added to a node via the config file. Please refer to the nis config file for details. Alternatively there are public nodes that support this request.
        /// </remarks> todo: get nodes.
        /// <param name="address">The account for which historical data should be returned</param>
        /// <param name="start">The block at which to start returning data</param>
        /// <param name="end"> The end block for which to return data</param>
        /// <param name="increment">The increment of blocks at which data should be returned</param>
        /// <returns>A<see cref="ManualAsyncResult"/> for the reqeust</returns>
        ///  <example> 
        ///  This sample shows how to use the <see>
        ///          <cref>BeginGetHistoricData</cref>
        ///      </see>
        ///      method.
        ///  <code>
        ///  class TestClass 
        ///  {
        ///      static void Main() 
        ///      {         
        ///          Connection connection = new Connection();
        ///          
        ///          connection.SetTestnet();
        ///  
        ///          AccountClient client = new AccountClient(connection);
        ///                
        ///           try
        ///          {
        ///              client.Connection.SetHost("34.211.28.210"); // node that accepts historical requests at time of writing
        /// 
        ///              ManualAsyncResult requestResponse = client.BeginGetHistoricData("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT", 100, 200, 1);
        ///         
        ///          }
        ///          catch (Exception ex) { Console.WriteLine(" error: " + ex.Message); }                          
        ///      }                                                       
        ///  }
        ///  </code>
        ///  </example>
        public ManualAsyncResult BeginGetHistoricalData(string address, long start, long end, int increment)
        {
            const string path = "/account/historical/get";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&startHeight=", start, "&endHeight=", end, "&increment=", increment);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        ///  <summary>
        ///  Ends an asynchronous request to get historical information about an account.
        ///  </summary>
        /// <returns>An <see cref=" HistoricData"/> object</returns>
        ///  <example> 
        ///  This sample shows how to use the <see>
        ///          <cref>BeginGetHistoricData</cref>
        ///      </see>
        ///      method.
        ///  <code>
        ///  class TestClass 
        ///  {
        ///      static void Main() 
        ///      {         
        ///          Connection connection = new Connection();
        ///          
        ///          connection.SetTestnet();
        ///  
        ///          AccountClient client = new AccountClient(connection);
        ///                
        ///           try
        ///          {
        ///              client.Connection.SetHost("34.211.28.210"); // node that accepts historical requests at time of writing
        /// 
        ///              ManualAsyncResult requestResponse = client.BeginGetHistoricData("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT", 100, 200, 1);
        ///         
        ///              HistoricalData data = client.EndGetHistoricalData(requestResponse);
        ///          }
        ///          catch (Exception ex) { Console.WriteLine(" error: " + ex.Message); }                          
        ///      }                                                       
        ///  }
        ///  </code>
        ///  </example>
        public HistoricData EndGetHistoricalData(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<HistoricData>(result);
        }

        #endregion
    }
}
