using System;
using System.Text;
using CSharp2nem.Connectivity;
using CSharp2nem.CryptographicFunctions;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.DataModels;
using CSharp2nem.Model.Mosaics;
using CSharp2nem.Model.MultiSig;
using CSharp2nem.Model.ProvisionNamespace;
using CSharp2nem.Model.Transfer;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects;
using CSharp2nem.Utils;
using Newtonsoft.Json;

namespace CSharp2nem.RequestClients
{

    /// <summary>
    /// Used to initiate transactions for a given account
    /// </summary>
    public class PrivateKeyAccountClient
    {
        /// <summary>
        /// The <see cref="Connectivity.Connection"/> to use for the client
        /// </summary>
        public Connection Connection { get; set; }

        /// <summary>
        /// The <see cref="Model.AccountSetup.PrivateKey"/> used to sign transactions
        /// </summary>
        public PrivateKey PrivateKey { get; internal set; }

        /// <summary>
        /// The <see cref="Model.AccountSetup.PublicKey"/> of the signing account
        /// </summary>
        public PublicKey PublicKey { get; set; }

        /// <summary>
        /// The <see cref="Model.AccountSetup.Address"/> of the signing account
        /// </summary>
        public Address Address { get; set; }

        internal ASCIIEncoding encoding = new ASCIIEncoding();

        internal PrivateKeyAccountClient(Connection connection, PrivateKey privateKey)
           
        {
            if (!StringUtils.OnlyHexInString(privateKey.Raw) || privateKey.Raw.Length != 64 && privateKey.Raw.Length != 66)
                    throw new ArgumentException("invalid private key");

          Connection = connection ?? throw new ArgumentNullException("Connection");

            PrivateKey = privateKey;
            PublicKey = new PublicKey(PublicKeyConversion.ToPublicKey(privateKey));
            Address = new Address(AddressEncoding.ToEncoded(Connection.GetNetworkVersion(), PublicKey));
;        }

        /// <summary>
        /// Ends the send transaction asynchronous operation.
        /// </summary>
        /// <remarks>
        /// Processes the response from performing the <see cref="BeginSendTransactionAsync"/> operation overload that does not include a callback as a parameter.
        /// </remarks>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from performing a BeginSendTransactionAsync wihtout a callback argument.</param>
        /// <returns>A <see cref="NemAnnounceResponse.Response"/> object that contains information about whether the transaction was successful.</returns>
        /// <exception cref="Exception">Any exception that might have happened internally will be raised in this function. This may include a faiure to communicate with a node.</exception>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginSendTransactionAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///       
        ///         var mosaicsList = new List&lt;Mosaic&gt;
        ///         {
        ///             new Mosaic("kod", "testing", 100000)
        ///         };
        ///         
        ///         var transData = new TransferTransactionData()
        ///         {
        ///             Amount = 50000000,
        ///             Message = "pacnemTime!!",
        ///             RecipientAddress = "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT", 
        ///             ListOfMosaics = mosaicsList,
        ///             MultisigAccountKey = new PublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0")
        ///         };
        ///         
        ///         try
        ///         {
        ///             var asyncResult = accClient.BeginSendTransactionAsync(transData);
        /// 
        ///             var response = accClient.EndSendTransaction(asyncResult);
        /// 
        ///         }
        ///         catch (Exception ex)
        ///         {        
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }     
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public NemAnnounceResponse.Response EndTransaction(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompletePostRequest<NemAnnounceResponse.Response>(result);
        }

        /// <summary>
        /// Begins the send transaction asynchronous operation.
        /// </summary>
        /// <remarks>
        /// The minimum arguments required to send a transaction include the recipient address and amount, however amount can be initialised as 0 if a mosaic or message should be sent without xem. All other arguments are optional.
        /// </remarks>
        /// <param name="callback">The callback action to be performed upon/post completion of the async operation.</param>
        /// <param name="transactionData">The transaction data used to define the transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginSendTransactionAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///       
        ///         var mosaicsList = new List&lt;Mosaic&gt;
        ///         {
        ///             new Mosaic("kod", "testing", 100000)
        ///         };
        ///         
        ///         var transData = new TransferTransactionData()
        ///         {
        ///             Amount = 50000000,
        ///             Message = "pacnemTime!!",
        ///             RecipientAddress = "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT", 
        ///             ListOfMosaics = mosaicsList,
        ///             MultisigAccountKey = new PublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0")
        ///         };
        ///         
        ///         try
        ///         {
        ///             var asyncResult = accClient.BeginSendTransactionAsync(body =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (body.Ex != null) throw body.Ex;
        /// 
        ///                     Console.WriteLine("message: " + body.Content.Message);                                                                                                                                                  
        ///                 }
        ///                 catch (Exception e)
        ///                 {
        ///                     Console.WriteLine("error: " + e.Message);
        ///                 }
        ///             }, transData);
        ///         }
        ///         catch (Exception ex)
        ///         {
        ///         
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }     
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginSendTransaction(Action<ResponseAccessor<NemAnnounceResponse.Response>> callback, TransferTransactionData transactionData)
        {

            
            var transfer = new TransferTransaction(Connection, PublicKey, PrivateKey, transactionData);
            
            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(transfer.GetTransferBytes());
          
            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };
          
            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);       
        }

        /// <summary>
        /// Begins the send transaction asynchronous operation.
        /// </summary>
        /// <remarks>
        /// The minimum arguments required to send a transaction include the recipient address and amount, however amount can be initialised as 0 if a mosaic or message should be sent without xem. All other arguments are optional.
        /// </remarks>
        /// <param name="transactionData">The <see cref="TransferTransactionData"/> is used to define the transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginSendTransactionAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///       
        ///         var mosaicsList = new List&lt;Mosaic&gt;
        ///         {
        ///             new Mosaic("kod", "testing", 100000)
        ///         };
        ///         
        ///         var transData = new TransferTransactionData()
        ///         {
        ///             Amount = 50000000,
        ///             Message = "pacnemTime!!",
        ///             RecipientAddress = "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT", 
        ///             ListOfMosaics = mosaicsList,
        ///             MultisigAccountKey = new PublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0")
        ///         };
        ///         
        ///         try
        ///         {
        ///             var asyncResult = accClient.BeginSendTransactionAsync(transData);
        ///         }
        ///         catch (Exception ex)
        ///         {        
        ///             Console.WriteLine("error: " + ex.Message);
        ///         }     
        ///     }                                                       
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginSendTransaction(TransferTransactionData transactionData)
        {
            var transfer = new TransferTransaction(Connection, PublicKey, PrivateKey, transactionData);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(transfer.GetTransferBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }     

        /// <summary>
        /// Begins the importance transfer transcation.
        /// </summary>
        /// <remarks>
        /// This transaction allows for transferring importance to another account, or also know as the delegated account. When this transaction is performed, your importance and therefore harvesting power is "lent" to the delegate account until you revoke it allowing you to harvest safely and securely. The importance transfer can be cancled at any stage after the transfer has been successfully complete, which takes 12 hours from the time that this transaction is initiated.
        /// </remarks>
        /// <param name="callback">The callback action to be performed upon completion of the async operation.</param>
        /// <param name="data">The <see cref="ImportanceTransferData"/> is used to define the importance transfer transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the importance transfer transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginSendTransactionAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         
        ///         var impTransferData = new ImportanceTransferData()
        ///         {
        ///             DelegatedAccount = new PublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0"),
        ///             Activate = true
        ///         };
        ///         try
        ///         {
        ///             accClient.BeginImportanceTransferAsync(body =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (body.Ex != null) throw body.Ex;
        /// 
        ///                     Console.WriteLine("message: " + body.Content.Message);
        ///                 }
        ///                 catch (Exception e)
        ///                 {
        ///                     Console.WriteLine("error: " + e.Message);
        ///                 }
        /// 
        ///             }, impTransferData);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }   
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginImportanceTransfer(Action<ResponseAccessor<NemAnnounceResponse.Response>> callback, ImportanceTransferData data)
        {
            var transfer = new ImportanceTransfer(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(transfer.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        /// <summary>
        /// Begins the importance transfer transcation.
        /// </summary>
        /// <remarks>
        /// This transaction allows for transferring importance to another account, or also know as the delegated account. When this transaction is performed, your importance and therefore harvesting power is "lent" to the delegate account until you revoke it allowing you to harvest safely and securely. The importance transfer can be cancled at any stage after the transfer has been successfully complete, which takes 12 hours from the time that this transaction is initiated.
        /// </remarks>
        /// <param name="data">The <see cref="ImportanceTransferData"/> is used to define the importance transfer transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the importance transfer transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginSendTransactionAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         
        ///         var impTransferData = new ImportanceTransferData()
        ///         {
        ///             DelegatedAccount = new PublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0"),
        ///             Activate = true
        ///         };
        /// 
        ///         try
        ///         {
        ///             ManualAsyncResult response = accClient.BeginImportanceTransferAsync(impTransferData);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }   
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginImportanceTransfer(ImportanceTransferData data)
        {
            var transfer = new ImportanceTransfer(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(transfer.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }

      
        /// <summary>
        /// Begins an aggregate multisig modification transcation.
        /// </summary>
        /// <remarks>
        /// This transaction allows for modification of the cosignatories and minimum number of signatory signatures required to send a transaction out of a multisignature account. When this transaction is performed, you may add or remove any number of cosignatories. For example, in a 5 of 5 multisignature account, 5 signatures are required to send a transaction. If you wish to remove 1 signatory in an aggregate modification transaction, you can specify which to remove and upon recieving 4 signatures, the cosignatory will be removed. An M-of-M multisig account is the only case where the number of signatures required to remove a signatory is different to the number required to send an outgoing transaction. This is for safety reasons so that an account will not be lost if one cosignatory loses their key. 
        /// 
        /// You may also add a cosignatory in the same transaction as when you remove one, in effect, replacing one cosignatory with another. The same is true for any number of cosignatories as long as the transaction recieves the required minimum signatures.
        /// </remarks>
        /// <param name="callback">The callback action to be performed upon completion of the async operation.</param>
        /// <param name="data">The <see cref="AggregateModificationData"/> is used to define the aggregate modification transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the aggregate modification transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginAggregateMultisigModificationAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         
        ///         var aggMod = new AggregateModificationData()
        ///         {
        ///             Modifications = new List&lt;AggregateModification&gt;()
        ///             {                                        
        ///                 new AggregateModification(new PublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0"), 2)
        ///             }
        ///         };
        ///         try
        ///         {
        ///             acc.BeginAggregateMultisigModificationAsync(body =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (body.Ex != null) throw body.Ex;
        /// 
        ///                     Console.WriteLine("message: " + body.Content.Message);
        ///                 }
        ///                 catch (Exception e)
        ///                 {
        ///                     Console.WriteLine("error: " + e.Message);
        ///                 }
        /// 
        ///             }, aggMod);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginAggregateMultisigModification(Action<ResponseAccessor<NemAnnounceResponse.Response>> callback, AggregateModificationData data)
        {
            var aggregateModification = new AggregateModificatioList(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(aggregateModification.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        /// <summary>
        /// Begins an aggregate multisig modification transcation.
        /// </summary>
        /// <remarks>
        /// This transaction allows for modification of the cosignatories and minimum number of signatory signatures required to send a transaction out of a multisignature account. When this transaction is performed, you may add or remove any number of cosignatories. For example, in a 5 of 5 multisignature account, 5 signatures are required to send a transaction. If you wish to remove 1 signatory in an aggregate modification transaction, you can specify which to remove and upon recieving 4 signatures, the cosignatory will be removed. An M-of-M multisig account is the only case where the number of signatures required to remove a signatory is different to the number required to send an outgoing transaction. This is for safety reasons so that an account will not be lost if one cosignatory loses their key. 
        /// 
        /// You may also add a cosignatory in the same transaction as when you remove one, in effect, replacing one cosignatory with another. The same is true for any number of cosignatories as long as the transaction recieves the required minimum signatures.
        /// 
        /// </remarks>
        /// <param name="data">The <see cref="AggregateModificationData"/> is used to define the aggregate modification transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the aggregate modification transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginAggregateMultisigModificationAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         
        ///         var aggMod = new AggregateModificationData()
        ///         {
        ///             Modifications = new List&lt;AggregateModification&gt;()
        ///             {                                        
        ///                 new AggregateModification(new PublicKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0"), 2)
        ///             }
        ///         };
        ///         try
        ///         {
        ///             acc.BeginAggregateMultisigModificationAsync(aggMod);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginAggregateMultisigModification(AggregateModificationData data)
        {
            var aggregateModification = new AggregateModificatioList(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(aggregateModification.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }

      
        /// <summary>
        /// Begins a provision name space transcation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="callback">The callback action to be performed upon completion of the async operation.</param>
        /// <param name="data">The <see cref="ProvisionNameSpaceData"/> is used to define the provision namespace transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the provision namespace transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginProvisionNamespaceAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         
        ///         var provNameSpace = new ProvisionNameSpaceData()
        ///         {
        ///             NewPart = "testaab",
        ///             Parent = "kod"
        ///         };
        /// 
        ///         try
        ///         {
        ///             accClient.BeginProvisionNamespaceAsync(body =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (body.Ex != null) throw body.Ex;
        /// 
        ///                     Console.WriteLine("message: " + body.Content.Message);
        ///                 }
        ///                 catch (Exception e)
        ///                 {
        ///                     Console.WriteLine("error: " + e.Message);
        ///                 }
        ///         
        ///             }, provNameSpace);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }                                                                                     
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginProvisionNamespaceAsync(Action<ResponseAccessor<NemAnnounceResponse.Response>> callback, ProvisionNameSpaceData data)
        {
            var nameSpace = new ProvisionNamespace(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(nameSpace.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        /// <summary>
        /// Begins a provision name space transcation.
        /// </summary>
        /// <param name="data">The <see cref="ProvisionNameSpaceData"/> is used to define the provision namespace transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the provision namespace transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginProvisionNamespaceAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");
        ///         
        ///         var provNameSpace = new ProvisionNameSpaceData()
        ///         {
        ///             NewPart = "testaab",
        ///             Parent = "kod"
        ///         };
        /// 
        ///         try
        ///         {
        ///             accClient.BeginProvisionNamespaceAsync(provNameSpace);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }                                                                                     
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginProvisionNamespaceAsync(ProvisionNameSpaceData data)
        {
            var transfer = new ProvisionNamespace(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(transfer.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }

       

        /// <summary>
        /// Begins a mosaic creation transcation.
        /// </summary>
        /// <remarks>
        /// A mosaic must be issued under a root or sub namespace. Use the <see cref="BeginProvisionNamespaceAsync"/> method to acquire a namespace prior to issuing a mosaic. Mosaics can also have a number of settings. Please refer to <see cref="ProvisionNameSpaceData"/> for further details on mosaic customisation. A number of the arguments given in the below example are optional. Please refer to <see cref="MosaicCreationData"/> class for further details.
        /// </remarks>
        /// <param name="callback">The callback action to be performed upon completion of the async operation.</param>
        /// <param name="data">The <see cref="MosaicCreationData"/> is used to define the mosaic creation transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the mosaic creation transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginCreateMosaicAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");       
        ///         
        ///         Mosaic mosaic = new Mosaic("namepsace", "mosaicName", 10000); 
        /// 
        ///         MosaicLevy levy = new MosaicLevy(new Address("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT"), mosaic, 1);
        /// 
        ///         var mosaicCreationData = new MosaicCreationData
        ///         {
        ///             Description = "test",
        ///             InitialSupply = 1000000,
        ///             Divisibility = 4,
        ///             NameSpaceId = "kod",
        ///             Transferable = true,
        ///             MosaicName = "test",
        ///             SupplyMutable = true,
        ///             MosaicLevy = levy      
        ///         };
        ///         
        ///         try
        ///         {
        ///             accClient.BeginCreateMosaicAsync(body =>
        ///             {
        ///                 try
        ///                 {
        ///                     if(body.Ex != null)
        ///                         Console.WriteLine(body.Ex);
        ///         
        ///                     else
        ///                         Console.WriteLine("message: " + body.Content.Message);
        ///                 }
        ///                 catch (Exception e)
        ///                 {
        ///                     Console.WriteLine("error: " + e.Message);
        ///                 }
        ///         
        ///             }, mosaicCreationData);      
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }        
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginCreateMosaicAsync(Action<ResponseAccessor<NemAnnounceResponse.Response>> callback, MosaicCreationData data)
        {
            var model = new MosaicDefinition(data, data.MultisigAccount ?? PublicKey);

            var mosaic = new CreateMosaic(Connection, PublicKey, model);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(mosaic.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        /// <summary>
        /// Begins a mosaic creation transcation.
        /// </summary>
        /// <remarks>
        /// A mosaic must be issued under a root or sub namespace. Use the <see cref="BeginProvisionNamespaceAsync"/> method to acquire a namespace prior to issuing a mosaic. Mosaics can also have a number of settings. Please refer to <see cref="ProvisionNameSpaceData"/> for further details on mosaic customisation. A number of the arguments given in the below example are optional. Please refer to <see cref="MosaicCreationData"/> class for further details.
        /// </remarks>
        /// <param name="data">The <see cref="MosaicCreationData"/> is used to define the mosaic creation transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the mosaic creation transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginCreateMosaicAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");       
        ///         
        ///         Mosaic mosaic = new Mosaic("namepsace", "mosaicName", 10000);
        /// 
        ///         MosaicLevy levy = new MosaicLevy(new Address("TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT"), mosaic, 1);
        /// 
        ///         var mosaicCreationData = new MosaicCreationData
        ///         {
        ///             Description = "test",
        ///             InitialSupply = 1000000,
        ///             Divisibility = 4,
        ///             NameSpaceId = "kod",
        ///             Transferable = true,
        ///             MosaicName = "test",
        ///             SupplyMutable = true,
        ///             MosaicLevy = levy        
        ///         };
        ///         
        ///         try
        ///         {
        ///             accClient.BeginCreateMosaicAsync(mosaicCreationData);      
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }        
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginCreateMosaicAsync(MosaicCreationData data)
        {
            var model = new MosaicDefinition(data, data.MultisigAccount ?? PublicKey);

            var mosaic = new CreateMosaic(Connection, PublicKey, model);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(mosaic.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }

       

        /// <summary>
        /// Begins a mosaic supply change transcation.
        /// </summary>
        /// <param name="callback">The callback action to be performed upon completion of the async operation.</param>
        /// <param name="data">The <see cref="MosaicSupplyChangeData"/> is used to define the mosaic supply change transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the mosaic creation transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginMosaicSupplyChangeAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");       
        ///         
        ///         var supChange = new MosaicSupplyChangeData()
        ///         {
        ///             Delta = 1000,
        ///             MosaicName = "test",
        ///             SupplyChangeType = 1,
        ///             NameSpaceId = "kod"
        ///         };
        ///         
        ///         try
        ///         {
        ///             accClient.BeginMosaicSupplyChangeAsync(body =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (body.Ex != null) throw body.Ex;
        ///         
        ///                     Console.WriteLine("message: " + body.Content.Message);
        ///                 }
        ///                 catch (Exception e)
        ///                 {
        ///                     Console.WriteLine("error: " + e.Message);
        ///                 }         
        ///             }, supChange);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }                                                                                                                                  
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginMosaicSupplyChangeAsync(Action<ResponseAccessor<NemAnnounceResponse.Response>> callback, MosaicSupplyChangeData data)
        {
            var change = new SupplyChange(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(change.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        /// <summary>
        /// Begins a mosaic supply change transcation.
        /// </summary>
        /// <param name="data">The <see cref="MosaicSupplyChangeData"/> is used to define the mosaic supply change transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the mosaic creation transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginMosaicSupplyChangeAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");       
        ///         
        ///         var supChange = new MosaicSupplyChangeData()
        ///         {
        ///             Delta = 1000,
        ///             MosaicName = "test",
        ///             SupplyChangeType = 1,
        ///             NameSpaceId = "kod"
        ///         };
        ///         
        ///         try
        ///         {
        ///             accClient.BeginMosaicSupplyChangeAsync(supChange);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("error: " + e.Message);
        ///         }                                                                                                                                  
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginMosaicSupplyChangeAsync(MosaicSupplyChangeData data)
        {
            var SupplyChange = new SupplyChange(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(SupplyChange.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }    

        /// <summary>
        /// Begins a multisignature signature transcation.
        /// </summary>
        /// <param name="callback">The callback action to be performed upon completion of the async operation.</param>
        /// <param name="data">The <see cref="MultisigSignatureTransactionData"/> is used to define the signature transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the multisig signature transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginSignatureTransactionAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");       
        ///         
        ///         try
        ///         {
        ///             acc.BeginSignatureTransactionAsync(body =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (body.Ex != null) throw body.Ex;
        /// 
        ///                     Console.WriteLine("4 " + body.Content.Message);
        ///                 }
        ///                 catch (Exception e)
        ///                 {
        ///                     Console.WriteLine("4 error: " + e.Message);
        ///                 }
        /// 
        ///             }, sigData);
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("4 error: " + e.Message);
        ///         }                                                                                                              
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginSignatureTransactionAsync(Action<ResponseAccessor<NemAnnounceResponse.Response>> callback, MultisigSignatureTransactionData data)
        {
            var signature = new MultisigSignature(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(signature.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        /// <summary>
        /// Begins a multisignature signature transcation.
        /// </summary>
        /// <param name="callback">The callback action to be performed upon completion of the async operation.</param>
        /// <param name="data">The <see cref="MultisigSignatureTransactionData"/> is used to define the signature transaction.</param>
        /// <returns><see cref="ManualAsyncResult"/>, The custom async result of initiating the multisig signature transaction.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginSignatureTransactionAsync</cref>
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
        ///         AccountFactory accountFactory = new AccountFactory(connection);
        /// 
        ///         PrivateKeyAccountClient accClient = accountFactory.FromPrivateKey("09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");       
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult result = accClient.BeginSignatureTransactionAsync(sigData);         
        ///         }
        ///         catch (Exception e)
        ///         {
        ///             Console.WriteLine("4 error: " + e.Message);
        ///         }                                                                                                              
        ///     }                                         
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginSignatureTransactionAsync(MultisigSignatureTransactionData data)
        {
            var transfer = new MultisigSignature(Connection, PublicKey, data);

            var bytesAndSig = new Prepare(Connection, PrivateKey).Transaction(transfer.GetBytes());

            var asyncResult = new ManualAsyncResult
            {
                Path = "/transaction/announce",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(bytesAndSig))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }
    }
}