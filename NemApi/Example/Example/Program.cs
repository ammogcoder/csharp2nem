using System;
using CSharp2nem;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Chaos.NaCl;

namespace run
{
    internal class Program
    {
        #region class properties
        private static readonly Connection Con = new Connection();

        private static VerifiableAccount NewAccount { get; }

        private static VerifiableAccount NewAccount2 { get; }

        private static VerifiableAccount Cosig1 { get; }

        private static VerifiableAccount Cosig2 { get; }

        private static VerifiableAccount Cosig3 { get; }

        private static UnverifiableAccount Unverifiable { get; }

        private static UnverifiableAccount DelegatedAccount { get; }

        private static UnverifiableAccount Multisig { get; }

        private static UnverifiableAccount UnverifiableEncoded { get; }

        #endregion

        static Program()
        {
            Con.SetTestNet();

            NewAccount = new AccountFactory(Con).FromNewPrivateKey();

            NewAccount2 = new AccountFactory().FromsNewDataPrivateKey("<any data, its hashed to produce a string the correct length>");

            Cosig1 = new AccountFactory(Con).FromPrivateKey(
                "<private key here>");

            Cosig2 = new AccountFactory(Con).FromPrivateKey(
                "<private key here>");

            Cosig3 = new AccountFactory(Con).FromPrivateKey(
                "<private key here>");

            DelegatedAccount = new AccountFactory(Con).FromPublicKey(
                "<public key here>");

            Unverifiable = new AccountFactory(Con).FromPublicKey(
                "<public key here>");

            Multisig = new AccountFactory(Con).FromEncodedAddress(
               "<Address here>");

            UnverifiableEncoded = new AccountFactory(Con).FromEncodedAddress(
                "<encoded address here>");


        }

        private static void Main()
        {


        }

        private static async void TransferTrans()
        {
            // Quantity of mosaic as smallest divisible part
            // ie. mosaic with 100,000 supply with divisibility of 3 means 1,000,000 total supply. 
            // eg. 1000 quantity => 1 whole mosaic
            var mosaicList = new List<Mosaic>
           {
               new Mosaic("kod_multisig", "non_business_without_levy", 100000),
               new Mosaic("kod_multisig", "non_business_without_levy_second", 100000)
           };

            /*
             * Amount: required >> amounts in micro xem, 1000000 MicroXEM => 1 XEM 
             * Recipient: required
             * Message: optional
             * Encrypted: optional >> encoding is broken, encrypts wrapper to wrapper successfully but not compatible with ncc/nano
             * ListOfMosaics: optional
             * MultisigAccount: optional >> if present, the initiating account must be cosigner on MultisigAccount
             * Deadline: optional >> calculated as seconds from timestamp.
             */
            var transferData = new TransferTransactionData
            {
                Amount = 1000000,
                Message = "testing transactions",
                Encrypted = false,
                Recipient = Unverifiable,
                ListOfMosaics = mosaicList,
                MultisigAccount = Multisig.PublicKey,
                Deadline = 10000
            };

            var transferResponse = await Cosig2.SendTransactionAsync(transferData);


            Console.WriteLine(transferResponse.Message);
            Console.ReadKey();
        }

        private static async void NameSpaceRental()
        {

            /*
             * NewPart: requireed
             * Parent: optional >> if parent is null, NewPart is registered as root
             * MultisigAccount: optional
             */
            var provData = new ProvisionNameSpaceData
            {
                NewPart = "NewPart",
                Parent = "Parent",
                MultisigAccount = Multisig.PublicKey
            };

            var provisionResponse = await Cosig1.ProvisionNamespaceAsync(provData);

            Console.WriteLine(provisionResponse.Message);
            Console.ReadKey();
        }

        private static async void MosaicDefinition()
        {
            /*
             * Mosaic Quantity here means the quantity of this mosaic that must be paid as a levy fee
             * for the mosaic that is to be defined.
             */
            var mosaic = new Mosaic("Namespace", "Mosaic", 100000000);

            /*
             * Fee beneificiary is the account that the fee should be paid to upon transacting
             * the mosaic to be defined.
             * Fee Type can be 1 for absolute, or 2 for percentile
             */
            var lev = new MosaicLevy(Cosig1.Address, mosaic, 1);

            /*
             * 
             * NamespaceId: required >> the namespace underwhich the mosaic should be defined
             * MosaicName: required
             * InitialSupply: required
             * Description: optional >> default empty string
             * Divisibility: optional >> default 0
             * SupplyMutable: optional >> default false
             * Transferable: optional >> default true
             * MosaicLevy: optional
             * MultisigAccount: optional
             */
            var mosaicDefCreate = new MosaicCreationData
            {
                NameSpaceId = "Namespace",
                MosaicName = "mosaic",
                InitialSupply = 9000000,
                Description = "testing",
                Divisibility = 3,
                SupplyMutable = true,
                Transferable = true,
                MosaicLevy = lev,
                MultisigAccount = Multisig.PublicKey
            };

            var definitionResponse = await Cosig1.CreateMosaicAsync(mosaicDefCreate);

            Console.WriteLine(definitionResponse.Message);
            Console.ReadLine();
        }

        private static async void ImportanceTransfer()
        {
            /*
             * Activate: required >> true to activate, falst to deactivate
             * Delegated account: required >> the account on which to activate or deactivate an importance transfer
             * MultisigAccount: optional
             */
            var impData = new ImportanceTransferData
            {
                Activate = false,
                DelegatedAccount = DelegatedAccount.PublicKey,
                MultisigAccount = Multisig.PublicKey
            };

            var importanceResponse = await Cosig1.ImportanceTransferAsync(impData);

            Console.WriteLine(importanceResponse.Message);
            Console.ReadLine();
        }

        private static async void MultisigSignatureTransaction()
        {

            /*
             * Get transactions that require signing
             */
            var trans = Multisig.GetUnconfirmedTransactionsAsync().Result;

            /*
             * MultisigAddress: required >> Multisig address which has transactions that requires signing
             * TransactionHash: hash of the transaction to be signed
             */
            var multiSigSignature = new MultisigSignatureTransactionData
            {
                MultisigAddress = Multisig.Address,
                TransactionHash = trans.data[0].meta.data
            };

            var signatureResponse = await Cosig2.SignMultisigTransaction(multiSigSignature);

            Console.WriteLine(signatureResponse.Message);

        }

        private static async void AggregateMultisigModification()
        {
            /*
             * modList 
             * List of modifications to be made to the multisig address
             * PublicKey: required >> public key of the account to be added or removed
             * ModType: require >> 1 for add, 2 for remove
             */
            var modList = new List<AggregateModification>
            {
                new AggregateModification(Cosig3.PublicKey, 2)
            };

            /*
             * MultisigAccount: required
             * Modifications: optional
             * RelativeChange: optional, default remains m of n >> if you wish to change from 3-of-3 to 2-of-3, relative change is -1
             */
            var modData = new AggregateModificationData
            {
                Modifications = modList,
                MultisigAccount = Multisig.PublicKey,
                RelativeChange = -1,
            };

            var aggModResponse = await Cosig1.AggregateMultisigModificationAsync(modData);

            Console.WriteLine(aggModResponse.Message);
            Console.ReadLine();
        }

        private static async void MosaicSupplyChange()
        {
            /*
             * NameSpaceId: required
             * MosaicName: required
             * SupplyChangeType: required >> the supplyType 1 means the supply is increased (a supply type of 2 means a supply decrease)
             * Delta: required >> the amount by which the supply should change
             * MultisigAccount: optional
             */
            var changeData = new MosaicSupplyChangeData
            {
                NameSpaceId = "Namespace",
                MosaicName = "Mosaic",
                SupplyChangeType = 0,
                Delta = 0,
                MultisigAccount = Multisig.PublicKey
            };

            var supplyChangeResponse = await Cosig1.MosaicSupplychangeAsync(changeData);

            Console.WriteLine(supplyChangeResponse.Message);
            Console.ReadLine();
        }

        private static async void OtherDataRequests()
        {
            /*
             * These requests should be self explanitory.
             * please refer to http://bob.nem.ninja/docs/ for further info
             */
          

            var result01 = await Unverifiable.GetAccountInfoAsync();
            var result02 = await Unverifiable.GetAccountStatusAsync();

            // account transactions
            var result03 = await Unverifiable.GetAllTransactionsAsync();
            var result04 = await Unverifiable.GetIncomingTransactionsAsync();
            var result05 = await Unverifiable.GetOutgoingTransactionsAsync();
            var result06 = await Unverifiable.GetUnconfirmedTransactionsAsync();

            // other
            var result07 = await Unverifiable.GetDelegatedAccountRootAsync();
            var result08 = await Unverifiable.GetHarvestingInfoAsync();
            var result09 = await Unverifiable.GetImportancesAsync();

            // account namespace and mosaic requests
            var result10 = await Unverifiable.GetMosaicsAsync();
            var result11 = await Unverifiable.GetMosaicsByNameSpaceAsync("namespace", "mosaic", 25);
            var result12 = await Unverifiable.GetMosaicsOwnedAsync();
            var result13 = await Unverifiable.GetNamespacesAsync();

            // node to which request sent must have historic data retrieval enabled.
            var result14 = await Unverifiable.HistoricData(Cosig1.Address.Encoded, 0, 100, 2);
        }

        private static async  Task BlockNisNodeRequests()
        {
            var blockClient = new BlockClient();

            var result = await blockClient.Last();

            var nisClient = new NisClient();

            var result2 = await nisClient.Status();

            var nodeClient = new NodeClient();

            var result3 = await nodeClient.Info();
        }
    }
}

