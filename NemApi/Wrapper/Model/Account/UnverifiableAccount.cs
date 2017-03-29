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
    */

    public class UnverifiableAccount
    {
        public UnverifiableAccount(Connection connection, PublicKey publicKey)
        {
            if (!publicKey.Raw.OnlyHexInString() || publicKey.Raw.Length != 64)
                throw new ArgumentException("invalid public key");

            Connection = connection;
            PublicKey = publicKey;

            Address = new Address(AddressEncoding.ToEncoded(Connection.GetNetworkVersion(), PublicKey));
        }

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

        public async Task<Transactions.All> GetUnconfirmedTransactionsAsync()
        {
            const string path = "/account/unconfirmedTransactions";

            var query = string.Concat("address=", Address.Encoded);

            var transactions = await new AsyncConnector.GetAsync<Transactions.All>(Connection).Get(path, query);

            DecodeTransactions(transactions);

            return transactions;
        }

        public async Task<HarvestingData.ListData> GetHarvestingInfoAsync(string hash = null)
        {
            if (null == hash)
            {
                var block = new Block(Connection);

                var lastBlock = block.Last().Result;

                hash = lastBlock.PrevBlockHash.Data;
            }

            const string path = "/account/harvests";

            var query = string.Concat("address=", Address.Encoded, "&hash=", hash);

            var data = await new AsyncConnector.GetAsync<HarvestingData.ListData>(Connection).Get(path, query);

            return data;
        }

        public async Task<Importances.ListImportances> GetImportancesAsync()
        {
            var data = await new AsyncConnector.GetAsync<Importances.ListImportances>(Connection).Get("/account/importances");

            return data;
        }

        public async Task<GeneratedKeyPair> GetGenerateNewAccountAsync()
        {
            var data = await new AsyncConnector.GetAsync<GeneratedKeyPair>(Connection).Get("/account/generate");

            return data;
        }

        public async Task<HistoricData> HistoricData(string address, long start, long end, int increment)
        {
            const string path = "/account/historical/get";

            var query = string.Concat("address=", address, "&startHeight=", start, "&endHeight=", end, "&increment=", increment);

            var data = await new AsyncConnector.GetAsync<HistoricData>(Connection).Get(path, query);

            return data;
        }

        internal async Task<ExistingAccount.Data> AccountInfoFromAddress(string address)
        {
            const string path = "/account/get";

            var query = string.Concat("address=", address);

            var data = await new AsyncConnector.GetAsync<ExistingAccount.Data>(Connection).Get(path, query);

            return data;
        }

        public async Task<ExistingAccount.Data> GetAccountInfoAsync()
        {
            var path = PublicKey == null ? "/account/get" : "/account/get/from-public-key";

            var query = PublicKey == null
                ? string.Concat("address=", Address.Encoded)
                : string.Concat("publicKey=", PublicKey.Raw);

            var data = await new AsyncConnector.GetAsync<ExistingAccount.Data>(Connection).Get(path, query);

            return data;
        }


        public async Task<AccountForwarded.Data> GetDelegatedAccountRootAsync()
        {
            var path = PublicKey.Raw == null ? "/account/get/forwarded" : "/account/get/forwarded/from-public-key";

            var query = PublicKey.Raw == null
                ? string.Concat("address =", Address.Encoded)
                : string.Concat("publicKey=", PublicKey.Raw);

            var data = await new AsyncConnector.GetAsync<AccountForwarded.Data>(Connection).Get(path, query);

            return data;
        }

        public async Task<Account.Status> GetAccountStatusAsync()
        {
            const string path = "/account/status";

            var query = string.Concat("address=", Address.Encoded);

            var data = await new AsyncConnector.GetAsync<Account.Status>(Connection).Get(path, query);

            return data;
        }

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

        public async Task<MosaicsOwned.RootObject> GetMosaicsOwnedAsync()
        {
            const string path = "/account/mosaic/owned";

            var query = string.Concat("address=", Address.Encoded);

            var data = await new AsyncConnector.GetAsync<MosaicsOwned.RootObject>(Connection).Get(path, query);

            return data;
        }

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