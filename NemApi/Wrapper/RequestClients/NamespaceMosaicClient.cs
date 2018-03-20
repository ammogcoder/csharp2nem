using System;
using CSharp2nem.Connectivity;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects.Mosaic;
using CSharp2nem.ResponseObjects.NameSpaces;
using CSharp2nem.Utils;

namespace CSharp2nem.RequestClients
{
    /// <summary>
    /// Contains a number of methods to get information about namespaces and mosaics.
    /// </summary>
    public class NamespaceMosaicClient
    {
        /// <summary>
        /// Gets or sets the connection to be used by this client.
        /// </summary>
        public Connection Connection { get; set; }


        /// <summary>
        /// Constructs an instance of the NamespaceMosaicClient using a given connection.
        /// </summary>
        /// <param name="connection">The connection to use for the client.</param>
        /// <exception cref="ArgumentNullException">Connection cannot be null</exception>
        public NamespaceMosaicClient(Connection connection)
        {
            if(connection == null) throw new ArgumentNullException(nameof(connection));

            Connection = connection;
        }

        /// <summary>
        /// Constructs an instance of the NamespaceMosaicClient using a default connection.
        /// </summary>
        public NamespaceMosaicClient()
        {
            Connection = new Connection();
        }

        #region Getting Mosaics By Namespace

        /// <summary>
        /// Get upto 100 mosaics created under a given namespace.
        /// </summary>
        /// <param name="callback">The action to be performed upon completion of the asynchronous operation</param>
        /// <param name="namespaceId">The namespace id.</param>
        /// <param name="id">The topmost mosaic definition database id up to which root mosaic definitions are returned. The parameter is optional. If not supplied the most recent mosaic definitiona are returned.</param>
        /// <param name="pageSize">The number of mosaic definition objects to be returned for each request. The parameter is optional. The default value is 25, the minimum value is 5 and hte maximum value is 100.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for this operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsByNameSpaceAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             client.BeginGetMosaicsByNameSpaceAsync(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///        
        ///                     Console.WriteLine("desc: " + returnedObject.Content.Data[0].Mosaic.Description);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        /// 
        ///             }, "namespaceID", "optionalID");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetMosaicsByNameSpace(Action<ResponseAccessor<MosaicDefinition.List>> callback, string namespaceId, string id = null, int pageSize = 0)
        {
            if (namespaceId == null) throw new ArgumentException("namespaceId cannot be null");

            const string path = "/namespace/mosaic/definition/page";

            var query = id == null && pageSize == 0
                ? string.Concat("namespace=", namespaceId) : id != null && pageSize == 0
                ? string.Concat("namespace=", namespaceId, "&name=", id)
                : string.Concat("namespace=", namespaceId, "&name=", id, "&pageSize=", pageSize);

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get upto 100 mosaics created under a given namespace.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="namespaceId">The namespace id.</param>
        /// <param name="id">The topmost mosaic definition database id up to which root mosaic definitions are returned. The parameter is optional. If not supplied the most recent mosaic definitiona are returned.</param>
        /// <param name="pageSize">The number of mosaic definition objects to be returned for each request. The parameter is optional. The default value is 25, the minimum value is 5 and hte maximum value is 100.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for this operation.</returns>
        /// <exception cref="ArgumentException">Namespace ID cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsByNameSpaceAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetMosaicsByNameSpaceAsync("namespaceID", "optionalID");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetMosaicsByNameSpace(string namespaceId, string id = null, int pageSize = 0)
        {
            if (namespaceId == null) throw new ArgumentException("namespaceId cannot be null");

            const string path = "/namespace/mosaic/definition/page";

            var query = id == null && pageSize == 0
                ? string.Concat("namespace=", namespaceId) : id != null && pageSize == 0
                ? string.Concat("namespace=", namespaceId, "&id=", id)
                : string.Concat("namespace=", namespaceId, "&id=", id, "&pageSize=", pageSize);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Ends the asynchronous request to get mosaics under a given namespace
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from the <see cref="BeginGetMosaicsByNameSpaceAsync"/> method.</param>
        /// <returns>The <see cref="MosaicDefinition.List"/> of mosaics created under the namespace.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsByNameSpaceAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetMosaicsByNameSpaceAsync("namespaceID", "optionalID");
        /// 
        ///             Definition.List mosaics = EndGetMosaicsByNamespace(responseResult);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }      
        ///     }
        /// }
        /// </code>
        /// </example>
        public MosaicDefinition.List EndGetMosaicsByNameSpace(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<MosaicDefinition.List>(result);
        }

        #endregion

        #region Getting Mosaics Created By An Account

        /// <summary>
        /// Get all mosaics created by an account.
        /// </summary>
        /// <param name="callback">The action to be perfomed upon completion of the asynchronous request.</param>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsCreatedAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             client.BeginGetMosaicsCreatedAsync(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///        
        ///                     Console.WriteLine("name: " + returnedObject.Content.Data[0].MosaicId.Name);
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
        public ManualAsyncResult BeginGetMosaicsCreated(Action<ResponseAccessor<MosaicDefinition.DefinitionList>> callback, string address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            const string path = "/account/mosaic/definition/page";

            var query =  string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get all mosaics created by an account.
        /// </summary>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsCreatedAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             ManualAsyncResult requestResult = client.BeginGetMosaicsCreatedAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                              
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetMosaicsCreated(string address)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/mosaic/definition/page";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Ends the asynchronous request to get mosaics created by an account.
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from performing the <see cref="BeginGetMosaicsCreatedAsync"/> method.</param>
        /// <returns>The <see cref="MosaicDefinition.DefinitionList"/> of mosaics an account has created.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsByNameSpaceAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             ManualAsyncResult requestResult = client.BeginGetMosaicsCreatedAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        /// 
        ///              MosaicDefinitions.List mosaics = client.EndGetMosaicsCreated(requestResponse);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                              
        ///     }
        /// }
        /// </code>
        /// </example>
        public MosaicDefinition.DefinitionList EndGetMosaicsCreated(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<MosaicDefinition.DefinitionList>(result);
        }

        #endregion

        #region Getting Mosaics Owned By An Account

        /// <summary>
        /// Get all mosaics owned by an account.
        /// </summary>
        /// <param name="callback">The action to be perfomed upon completion of the asynchronous request.</param>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsOwnedAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             client.BeginGetMosaicsOwnedAsync(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///        
        ///                     Console.WriteLine("name: " + returnedObject.Content.Data[0].MosaicId.Name);
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
        public ManualAsyncResult BeginGetMosaicsOwned(Action<ResponseAccessor<MosaicDefinition.OwnedList>> callback, string address)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/mosaic/owned";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get all mosaics owned by an account.
        /// </summary>
        /// <param name="callback">The action to be perfomed upon completion of the asynchronous request.</param>
        /// <param name="address">The address of the account</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsOwnedAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetMosaicsOwnedAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                              
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetMosaicsOwned(string address)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/mosaic/owned";

            var query = string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address));

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Ends the asynchronous request to get all mosaics that an account owns
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from performing the <see cref="BeginGetMosaicsOwnedAsync"/> method.</param>
        /// <returns>The <see cref="MosaicDefinition.OwnedList"/> for a given account.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetMosaicsOwnedAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetMosaicsOwnedAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        /// 
        ///             MosaicDefinition.MosaicOwned mosaics = client.EndGetMosaicsOwned(result);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                              
        ///     }
        /// }
        /// </code>
        /// </example>
        public MosaicDefinition.OwnedList EndGetMosaicsOwned(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<MosaicDefinition.OwnedList>(result);
        }

        #endregion

        #region Getting Namepsaces Owned By An Account
        
        /// <summary>
        /// Get all mosaics owned by an account.
        /// </summary>
        /// <param name="callback">The action to be perfomed upon completion of the asynchronous request.</param>
        /// <param name="address">The address of the account</param>
        /// <param name="parent">The optional parent namespace id.</param>
        /// <param name="id">The optional namespace database id up to which namespaces are returned.</param>
        /// <param name="pageSize">The optional number of namespaces to be returned.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetNamespacesAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             client.BeginGetNamespacesAsync(returnedObject =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (returnedObject.Ex != null) throw returnedObject.Ex;
        ///        
        ///                     Console.WriteLine("fqn: " + returnedObject.Content.Data[0].Fqn);
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
        public ManualAsyncResult BeginGetNamespaces(Action<ResponseAccessor<NameSpaceList>> callback, string address, string parent = null, string id = null, int pageSize = 0)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/namespace/page";

            var query = parent == null && id == null && pageSize == 0
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address)) : parent != null && id == null && pageSize == 0
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&parent=", parent) : parent != null && id != null && pageSize == 0
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&parent=", parent, "&id=", id)
                : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&parent=", parent, "&id=", id, "&pageSize=", pageSize);

            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, path, query);
        }

        /// <summary>
        /// Get all mosaics owned by an account.
        /// </summary>
        /// <param name="address">The address of the account</param>
        /// <param name="parent">The optional parent namespace id.</param>
        /// <param name="id">The optional namespace database id up to which namespaces are returned.</param>
        /// <param name="pageSize">The optional number of namespaces to be returned.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the operation.</returns>
        /// <exception cref="ArgumentNullException">Address cannot be null</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetNamespacesAsync</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetNamespacesAsync( "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                              
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetNamespaces(string address, string parent = null, string id = null, int pageSize = 0)
        {
            if (address == null) throw new ArgumentException("address cannot be null");

            const string path = "/account/namespace/page";
            
            var query = parent == null && id == null && pageSize == 0
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address)) : parent != null && id == null && pageSize == 0
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&parent=", parent) : parent != null && id != null && pageSize == 0
                ? string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&parent=", parent, "&id=", id)
                : string.Concat("address=", StringUtils.GetResultsWithoutHyphen(address), "&parent=", parent, "&id=", id, "&pageSize=", pageSize);

            return new HttpConnector(Connection).PrepareGetRequest(path, query);
        }

        /// <summary>
        /// Ends the asynchronous request to get all namespaces that an account created
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from performing the <see cref="BeginGetNamespacesAsync"/> method.</param>
        /// <returns>The <see cref="NameSpaceList"/> for a given account.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>EndGetNamespaces</cref>
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
        ///         NamespaceMosaicClient client = new NamespaceMosaicClient(connection);  
        ///        
        ///         try
        ///         {
        ///             ManualAsyncResult result = client.BeginGetNamespacesAsync("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT");
        /// 
        ///             MosaicsOwned.Mosaics mosaics = client.EndGetNamespaces(result);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                              
        ///     }
        /// }
        /// </code>
        /// </example>
        public NameSpaceList EndGetNamespaces(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<NameSpaceList>(result);
        }

        #endregion
    }
}
