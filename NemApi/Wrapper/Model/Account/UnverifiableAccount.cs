using System;
using System.Text;
using System.Threading.Tasks;
using Chaos.NaCl;
using CSharp2nem.Async;

// ReSharper disable once CheckNamespace

namespace CSharp2nem
{
    /*
    * Creates an instance of an unverfiable account
    * 
    * These requests do not require a signature.
    * 
    */
    public class UnverifiableAccount
    {
        /*
         * Create an Unverifiable account from a given connection and public key
         * 
         * @connection { Connection } The connection to use for the account
         * @publicKey { PublicKey } The public key to use to create the account
         * 
         */
        public UnverifiableAccount(Connection connection, PublicKey publicKey)
        {
            if (!publicKey.Raw.OnlyHexInString() || publicKey.Raw.Length != 64)
                throw new ArgumentException("invalid public key");

            Connection = connection;
            PublicKey = publicKey;

            Address = new Address(Connection.GetNetworkVersion().ToEncoded(PublicKey));
        }

        /*
         * Create an Unverifiable account from a given connection and public key
         * 
         * @connection { Connection } The connection to use for the account
         * @address { Address } The address to use to create the account
         * 
         * The public key is automatically retrieved if it is known to the network
         */
        public UnverifiableAccount(Connection connection, Address address)
        {
            if (address.Encoded.Length != 40)
                throw new ArgumentException("invalid Address");

            Connection = connection;
            Address = address;
            
            var a = AccountInfoFromAddress(Address.Encoded).Result;

            if (a.Account?.PublicKey != null)
            {
                PublicKey = new PublicKey(a.Account.PublicKey);

            }   
        }

        public Connection Connection { get; set; }
        public PublicKey PublicKey { get; set; }
        public Address Address { get; set; }

        /*
         * Get all transactions for a given account
         * 
         * http://bob.nem.ninja/docs/#requesting-transaction-data-for-an-account
         * 
         * @hash { string } The 256 bit sha3 hash of the transaction up to which transactions are returned
         * @id { int } The transaction id up to which transactions are returned
         * 
         * Return: All the transactions for the account
         */
        public async Task<Transactions.All> GetAllTransactionsAsync(string hash = null, int id = 0)
        {
            const string path = "/account/transfers/all";

            var query = id == 0 && hash == null
                ? string.Concat("address=", Address.Encoded)
                : id == 0 && hash != null
                    ? string.Concat("address=", Address.Encoded, "&hash=", hash)
                    : string.Concat("address=", Address.Encoded, "&hash=", hash, "&id=", id);

            var transactions = await new AsyncConnector.GetAsync<Transactions.All>(Connection).Get(path, query);

            DecodeTransactions(transactions);

            return transactions;
        }

        /*
         * Get all incoming transactions for a given account
         * 
         * http://bob.nem.ninja/docs/#requesting-transaction-data-for-an-account
         * 
         * @hash { string } The 256 bit sha3 hash of the transaction up to which transactions are returned
         * @id { int } The transaction id up to which transactions are returned
         * 
         * Return: All incoming the transactions for the account
         */
        public async Task<Transactions.All> GetIncomingTransactionsAsync(string hash = null, int id = 0)
        {
            const string path = "/account/transfers/incoming";

            var query = id == 0 && hash == null
                ? string.Concat("address=", Address.Encoded)
                : id == 0 && hash != null
                    ? string.Concat("address=", Address.Encoded, "&hash=", hash)
                    : string.Concat("address=", Address.Encoded, "&hash=", hash, "&id=", id);

            var transactions = await new AsyncConnector.GetAsync<Transactions.All>(Connection).Get(path, query);

            DecodeTransactions(transactions);

            return transactions;
        }

        /*
         * Get all outgoing transactions for a given account
         * 
         * http://bob.nem.ninja/docs/#requesting-transaction-data-for-an-account
         * 
         * @hash { string } The 256 bit sha3 hash of the transaction up to which transactions are returned
         * @id { int } The transaction id up to which transactions are returned
         * 
         * Return: All outgoing the transactions for the account
         */
        public async Task<Transactions.All> GetOutgoingTransactionsAsync(string hash = null, int id = 0)
        {
            const string path = "/account/transfers/outgoing";

            var query = id == 0 && hash == null
                ? string.Concat("address=", Address.Encoded) 
                : id == 0 && hash != null
                    ? string.Concat("address=", Address.Encoded, "&hash=", hash)
                        : string.Concat("address=", Address.Encoded, "&hash=", hash, "&id=", id);

            var transactions = await new AsyncConnector.GetAsync<Transactions.All>(Connection).Get(path, query);

            DecodeTransactions(transactions);

            return transactions;
        }

        /*
         * Get all unconfirmed transactions for a given account
         * 
         * http://bob.nem.ninja/docs/#requesting-transaction-data-for-an-account
         * 
         * @hash { string } The 256 bit sha3 hash of the transaction up to which transactions are returned
         * @id { int } The transaction id up to which transactions are returned
         * 
         * Return: All unconfirmed the transactions for the account
         */
        public async Task<Transactions.All> GetUnconfirmedTransactionsAsync()
        {
            const string path = "/account/unconfirmedTransactions";

            var query = string.Concat("address=", Address.Encoded);

            var transactions = await new AsyncConnector.GetAsync<Transactions.All>(Connection).Get(path, query);

            DecodeTransactions(transactions);

            return transactions;
        }

        /*
         * Get a list of harvesting info for an account
         * 
         * http://bob.nem.ninja/docs/#requesting-harvest-info-data-for-an-account 
         * 
         * @hash { string } The 256 bit sha3 hash of the block up to which harvested blocks are returned
         * 
         * Return: List of harvesting formation objects       
         */
        public async Task<HarvestingData.ListData> GetHarvestingInfoAsync(string hash = null)
        {
            if (null == hash)
            {
                var block = new BlockClient(Connection);

                var lastBlock = block.Last().Result;

                hash = lastBlock.PrevBlockHash.Data;
            }
            
            const string path = "/account/harvests";

            var query = string.Concat("address=", Address.Encoded, "&hash=", hash);

            var data = await new AsyncConnector.GetAsync<HarvestingData.ListData>(Connection).Get(path, query);

            return data;
        }

        /*
         * Get an array of account importance objects
         * 
         * http://bob.nem.ninja/docs/#retrieving-account-importances-for-accounts
         * 
         * Return: List of account importance view models
         * 
         */
        public async Task<Importances.ListImportances> GetImportancesAsync()
        {
            var data = await new AsyncConnector.GetAsync<Importances.ListImportances>(Connection).Get("/account/importances");

            return data;
        }

        /*
         * Get historical data for an account
         * 
         * http://bob.nem.ninja/docs/#retrieving-historical-account-data
         * 
         * @address { string } The account for which historical data should be returned
         * @start { long } The block at which to start returning data
         * @end { long } The end block for which to return data
         * @increment { int } The increment of blocks at which data should be returned
         * 
         * Return: Historical account data object
         * 
         */
        public async Task<HistoricData> HistoricData(string address, long start, long end, int increment)
        {
            const string path = "/account/historical/get";

            var query = string.Concat("address=", address, "&startHeight=", start, "&endHeight=", end, "&increment=", increment);

            var data = await new AsyncConnector.GetAsync<HistoricData>(Connection).Get(path, query);

            return data;
        }

        /*
         * Get account information for a given account
         * 
         * http://bob.nem.ninja/docs/#requesting-the-account-data
         * 
         * @address { string } The address information should be returned for
         * 
         * Return: Account data object
         * 
         * Note: used internally for convenience
         */
        internal async Task<ExistingAccount.Data> AccountInfoFromAddress(string address)
        {
            const string path = "/account/get";

            var query = string.Concat("address=", address);

            var data = await new AsyncConnector.GetAsync<ExistingAccount.Data>(Connection).Get(path, query);

            return data;
        }

        /*
         * Get account information for a given account
         * 
         * http://bob.nem.ninja/docs/#requesting-the-account-data
         * 
         * @address { string } The address information should be returned for
         * 
         * Return: Account data object
         * 
         */
        public async Task<ExistingAccount.Data> GetAccountInfoAsync()
        {
            var path = PublicKey == null ? "/account/get" : "/account/get/from-public-key";

            var query = PublicKey == null
                ? string.Concat("address=", Address.Encoded)
                : string.Concat("publicKey=", PublicKey.Raw);

            var data = await new AsyncConnector.GetAsync<ExistingAccount.Data>(Connection).Get(path, query);

            return data;
        }


        /*
         * Get the delegated account meta data pair for a given account.
         * eg. If you create an Unverifiable account, this will return 
         * information on the account that is acting as delegate for this 
         * account.
         * 
         * http://bob.nem.ninja/docs/#requesting-the-original-account-data-for-a-delegate-account
         * 
         * Return: account meta data pair
         * 
         */
        public async Task<AccountForwarded.Data> GetDelegatedAccountRootAsync()
        {
            var path = PublicKey.Raw == null ? "/account/get/forwarded" : "/account/get/forwarded/from-public-key";

            var query = PublicKey.Raw == null
                ? string.Concat("address =", Address.Encoded)
                : string.Concat("publicKey=", PublicKey.Raw);

            var data = await new AsyncConnector.GetAsync<AccountForwarded.Data>(Connection).Get(path, query);

            return data;
        }

        /*
         * Get the accont status of an account such as cosignatory status, 
         * locked for harvesting status and remote active status.
         * 
         * http://bob.nem.ninja/docs/#requesting-the-account-status
         * 
         * Return: Account status object
         * 
         */
        public async Task<Account.Status> GetAccountStatusAsync()
        {
            const string path = "/account/status";

            var query = string.Concat("address=", Address.Encoded);

            var data = await new AsyncConnector.GetAsync<Account.Status>(Connection).Get(path, query);

            return data;
        }

        /*
         * Get all mosaics under a given namespace
         * 
         * http://bob.nem.ninja/docs/#retrieving-mosaic-definitions
         * 
         * @namespaceId { string } The namespace under which mosaics should be returned
         * @id { string } The topmost mosaic definition database id up to which root mosaic definitions are returned.
         *                If none supplied, the top most mosaics are returned
         * 
         * Return: A list of mosaic definitions
         * 
         */
        public async Task<Definition.List> GetMosaicsByNameSpaceAsync(string namespaceId, string id = null,
            int pageSize = 0)
        {
            const string path = "/namespace/mosaic/definition/page";

            var query = id == null && pageSize == 0
                ? string.Concat("namespace=", namespaceId) : id != null && pageSize == 0
                ? string.Concat("namespace=", namespaceId, "&id=", id)
                : string.Concat("namespace=", namespaceId, "&id=", id, "&pageSize=", pageSize);

            var data = await new AsyncConnector.GetAsync<Definition.List>(Connection).Get(path, query);

            return data;
        }

        /*
         * Get mosaics created by an account
         * 
         * http://bob.nem.ninja/docs/#retrieving-mosaics-that-an-account-owns
         * 
         * @id { string } The database id upto which mosaics should be returned.
         *                If none supplied, the top most mosaics are returned.
         * @pageSize { int } The number of mosaics to be returned
         * 
         * Return: A list of mosaic definitions created by the given account
         * 
         */
        public async Task<MosaicDefinitions.List> GetMosaicsAsync(string id = null, int pageSize = 0)
        {
            const string path = "/account/mosaic/definition/page";

            var query = id == null && pageSize == 0
                ? string.Concat("address=", Address.Encoded) : id != null && pageSize == 0
                ? string.Concat("address=", Address.Encoded, "&id=", id)
                : string.Concat("address=", Address.Encoded, "&id=", id, "&pageSize=", pageSize);

            var data = await new AsyncConnector.GetAsync<MosaicDefinitions.List>(Connection).Get(path, query);

            return data;
        }

        /*
         * Get mosaics owned by an account
         * 
         * http://bob.nem.ninja/docs/#retrieving-mosaics-that-an-account-owns
         * 
         * @id { string } The database id upto which mosaics should be returned.
         *                If none supplied, the top most mosaics are returned.
         * @pageSize { int } The number of mosaics to be returned
         * 
         * Return: A list of mosaic definitions owned by the given account
         * 
         */
        public async Task<MosaicsOwned.RootObject> GetMosaicsOwnedAsync()
        {
            const string path = "/account/mosaic/owned";

            var query = string.Concat("address=", Address.Encoded);

            var data = await new AsyncConnector.GetAsync<MosaicsOwned.RootObject>(Connection).Get(path, query);

            return data;
        }

        /*
         * Get namespaces owned by an account
         * 
         * http://bob.nem.ninja/docs/#retrieving-namespaces-that-an-account-owns
         * 
         * @parent { string } The parent namespace under which sub namespaces 
         *                    should be returned. If null, only parent addresses
         *                    are returned
         *                    
         * @id { string } The database id upto which namespaces should be returned.
         *                If none supplied, the top most namespaces are returned.
         *                
         * @pageSize { int } The number of namespaces to be returned
         * 
         * Return: A list of namespace definitions
         * 
         */
        public async Task<NameSpaceList> GetNamespacesAsync(
            string parent = null, string id = null, int pageSize = 0)
        {
            const string path = "/account/namespace/page";

            var query = parent == null && id == null && pageSize == 0
                ? string.Concat("address=", Address.Encoded) : parent != null && id == null && pageSize == 0
                ? string.Concat("address=", Address.Encoded, "&parent=", parent) : parent != null && id != null && pageSize == 0
                ? string.Concat("address=", Address.Encoded, "&parent=", parent, "&id=", id)
                : string.Concat("address=", Address.Encoded, "&parent=", parent, "&id=", id, "&pageSize=", pageSize);

            var data = await new AsyncConnector.GetAsync<NameSpaceList>(Connection).Get(path, query);

            return data;
        }

        /*
         * Decode transaction messages returned in above API's.
         * Used internally to decode transactions
         * 
         * No documentation
         * 
         * Ret
         */
        private static void DecodeTransactions(Transactions.All transactions)
        {
            foreach (var t in transactions.data)
                if (null != t.transaction.otherTrans?.message?.payload)
                    t.transaction.otherTrans.message.payload =
                        Encoding.GetEncoding("UTF-8")
                            .GetString(CryptoBytes.FromHexString(t.transaction.otherTrans.message.payload));
        }
    }
}