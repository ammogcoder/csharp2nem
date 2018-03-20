using System;
using System.Text;
using CSharp2nem.Connectivity;
using CSharp2nem.PrepareHttpRequests;
using CSharp2nem.ResponseObjects.Block;
using Newtonsoft.Json;

namespace CSharp2nem.RequestClients
{
    /// <summary>
    /// Contains methods to retrieve block related data.
    /// </summary>
    public class BlockClient
    {
        /// <summary>
        /// Constructs an instance of a blockclient.
        /// </summary>
        /// <param name="connection">The connection to use for the connection</param>
        /// <exception cref="ArgumentNullException">Connection cannot be null</exception>
        public BlockClient(Connection connection)
        {
            if(connection == null) throw new ArgumentNullException(nameof(connection));

            Connection = connection;
        }

        /// <summary>
        /// Constructs an instance of a blockclient.
        /// </summary>
        /// <remarks>
        /// Automatically sets a defeault <see cref="Connectivity.Connection"/>
        /// </remarks>
        public BlockClient()
        {
            Connection = new Connection();
        }
       
        /// <summary>
        /// Gets or sets the current connection for this client.
        /// </summary>
        public Connection Connection { get; set; }

        internal ASCIIEncoding encoding = new ASCIIEncoding();
        #region Getting Chain Score

        /// <summary>
        /// Gets the current chain score for the currently connected node.
        /// </summary>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetChainScoreAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("score: " + obj.Content.Score);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetChainScore(Action<ResponseAccessor<BlockData.BlockScore>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/chain/score");
        }

        /// <summary>
        /// Gets the current chain score for the current connected node.
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetChainScoreAsync();
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetChainScore()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/chain/score");
        }

        /// <summary>
        /// Ends the asynchronous request for the chain score
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from performing the <see cref="BeginGetChainScoreAsync"/> operation</param>
        /// <returns>The <see cref="BlockData.BlockScore"/> for the node currently connected to.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>EndGetChainScore</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetChainScoreAsync();
        /// 
        ///             BlockData.BlockScore score = client.EndGetChainScore(responseResult);
        /// 
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public BlockData.BlockScore EndGetChainScore(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<BlockData.BlockScore>(result);
        }

        #endregion

        #region Getting Block Height

        /// <summary>
        /// Gets the current chain height for the currently connected node.
        /// </summary>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetChainHeightAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetChainHeightAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("height " + obj.Content.height);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetChainHeight(Action<ResponseAccessor<BlockData.BlockHeight>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/chain/height");
        }

        /// <summary>
        /// Gets the current chain height for the currently connected node.
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetChainHeightAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetChainHeightAsync();             
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetChainHeight()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/chain/height");
        }

        /// <summary>
        /// Ends the asynchronous call to get the current chain height.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>the <see cref="BlockData.BlockHeight"/> for the current currently connected node.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>EndGetChainHeight</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetChainHeightAsync();          
        /// 
        ///             BlockData.BlockHeight data = client.EndGetChainHeight(responseResult);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public BlockData.BlockHeight EndGetChainHeight(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<BlockData.BlockHeight>(result);
        }

        #endregion

        #region Getting THe Last Block

        /// <summary>
        /// Gets the last confirmed block in the chain for the currently connected node.
        /// </summary>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetLastBlockAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetLastBlockAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("height: " + obj.Content.height);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             });
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetLastBlock(Action<ResponseAccessor<BlockData.Block>> callback)
        {
            return new HttpAsyncConnector(Connection).PrepareGetRequestAsync(callback, "/chain/last-block");
        }

        /// <summary>
        /// Gets the last confirmed block in the chain for the currently connected node.
        /// </summary>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetLastBlockAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetLastBlockAsync();
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetLastBlock()
        {
            return new HttpConnector(Connection).PrepareGetRequest("/chain/last-block");
        }

        /// <summary>
        /// Ends the asynchronous operation to get the last block in the chain of the currently connected node.
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from the <see cref="BeginGetLastBlockAsync"/> method.</param>
        /// <returns>The <see cref="BlockData.Block"/> of the most recent block in the chain of the currently connected node.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetLastBlockAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetLastBlockAsync();
        /// 
        ///             BlockData.Block data = client.EndGetLastBlock(responseResult);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public BlockData.Block EndGetLastBlock(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompleteGetRequest<BlockData.Block>(result);
        }

        #endregion

        #region Getting The Block At A current Height

        /// <summary>
        /// Gets the  block at the given height in the chain.
        /// </summary>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <param name="height">The height of the block to be retrieved.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetByHeightAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetByHeightAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("height: " + obj.Content.height);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             }, 10000);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetByHeight(Action<ResponseAccessor<BlockData.Block>> callback, int height)
        {
         
            var asyncResult = new ManualAsyncResult
            {
                Path = "/block/at/public/",               
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(new BlockData.BlockHeight { height = height }))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        /// <summary>
        /// Gets the  block at the given height in the chain.
        /// </summary>
        /// <param name="height">The height of the block to be retrieved.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetByHeightAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetByHeightAsync(10000);          
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetByHeight(int height)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/block/at/public/",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(new BlockData.BlockHeight { height = height }))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }

        /// <summary>
        /// Ends the asynchronous operation to retrieve a block at the given height.
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> returned from performing the <see cref="BeginGetByHeightAsync"/> method.</param>
        /// <returns>The <see cref="BlockData.Block"/> information at the given height in the chain.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetByHeightAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult responseResult = client.BeginGetByHeightAsync(10000);  
        /// 
        ///             BlockData.Block block = client.EndGetByHeight(responseResult);        
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public BlockData.Block EndGetByHeight(ManualAsyncResult result)
        {

            return new HttpConnector(Connection).CompletePostRequest<BlockData.Block>(result);
        }

        #endregion

        #region Getting Part Of THe Chain

        /// <summary>
        /// Gets a sequence of blocks in chronological order from a local node.
        /// </summary>
        /// <remarks>
        /// The request will be refused if attempted with a remote node. if there are less than 10 blocks up to the height given, the number of blocks available will be returned.
        /// </remarks>
        /// <param name="callback">The action to perfom upon completion of the asynchronous operation.</param>
        /// <param name="height">The height upto which the sequence of blocks to be retrieved.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetLocalChainPartAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             client.BeginGetLocalChainPartAsync(obj =>
        ///             {
        ///                 try
        ///                 {
        ///                     if (obj.Ex != null) throw obj.Ex;
        /// 
        ///                     Console.WriteLine("height: " + obj.Content.Data[0].Hash);
        ///                 }
        ///                 catch (Exception e) { Console.WriteLine("error: " + e.Message); }
        ///             }, 10000);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetLocalChainPart(Action<ResponseAccessor<BlockData.BlockList>> callback, int height)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/local/chain/blocks-after",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(new BlockData.BlockHeight { height = height }))
            };

            return new HttpAsyncConnector(Connection).PreparePostRequestAsync(callback, asyncResult);
        }

        /// <summary>
        /// Gets a sequence of blocks in chronological order from a local node.
        /// </summary>
        /// <remarks>
        /// The request will be refused if attempted with a remote node. if there are less than 10 blocks up to the height given, the number of blocks available will be returned.
        /// </remarks>
        /// <param name="height">The height upto which the sequence of blocks to be retrieved.</param>
        /// <returns>The <see cref="ManualAsyncResult"/> for the asynchronous operation.</returns>
        /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetLocalChainPartAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult requestResponse = client.BeginGetLocalChainPartAsync(10000);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public ManualAsyncResult BeginGetLocalChainPart(int height)
        {
            var asyncResult = new ManualAsyncResult
            {
                Path = "/local/chain/blocks-after",
                Bytes = encoding.GetBytes(JsonConvert.SerializeObject(new BlockData.BlockHeight { height = height }))
            };

            return new HttpConnector(Connection).PreparePostRequest(asyncResult);
        }

        /// <summary>
        /// Ends the asynchronous request to get a part of the local chain
        /// </summary>
        /// <param name="result">The <see cref="ManualAsyncResult"/> response from performing the <see cref="BeginGetLocalChainPartAsync"/> method.</param>
        /// <returns>The <see cref="BlockData.BlockList"/> up to the given height from the local chain.</returns>
        ///         /// <example> 
        /// This sample shows how to use the <see>
        ///         <cref>BeginGetLocalChainPartAsync</cref>
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
        ///         BlockClient client = new BlockClient(connection);
        ///         
        ///         try
        ///         {
        ///             ManualAsyncResult requestResponse = client.BeginGetLocalChainPartAsync(10000);
        /// 
        ///             BlockData.BlockList data = client.EndGetLocalChainPart(requestResponse);
        ///         }
        ///         catch (Exception ex) { Console.WriteLine("error: " + ex.Message); }                                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public BlockData.BlockList EndGetLocalChainPart(ManualAsyncResult result)
        {
            return new HttpConnector(Connection).CompletePostRequest<BlockData.BlockList>(result);
        }

        #endregion
    }
}