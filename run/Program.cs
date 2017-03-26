using System;
using NemApi;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.NaCl;

namespace run
{
    internal class Program
    {
        #region class properties
        private static readonly Connection Con = new Connection();

        private static VerifiableAccount Cosig1 { get; }

        private static VerifiableAccount Cosig2 { get; }

        private static VerifiableAccount Cosig3 { get; }

        private static UnverifiableAccount Unverifiable { get; }
        private static UnverifiableAccount UnverifiableFromAddress { get; }

        private static UnverifiableAccount DelegatedAccount { get; }

        private static UnverifiableAccount Multisig { get; }

        #endregion

        static Program()
        {
            Con.SetTestNet();

            Cosig1 = new AccountFactory(Con).FromPrivateKey(
                "fcdadb68356c6227a0942b377209401574ece844e8e579edbfe36a5193cf8cb5");

            Cosig2 = new AccountFactory(Con).FromPrivateKey(
                "afaa0261af8972b05394a442238281b1231c1a8d569293ced9f3df2564f383d1");

            Cosig3 = new AccountFactory(Con).FromPrivateKey(
                "09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0");

            DelegatedAccount = new AccountFactory(Con).FromPublicKey(
                "c10e970b5a9c50ddcb9847ec6d3a8972eec31f0509a6c6c5c331bb133d5f745b");

            Multisig = new AccountFactory(Con).FromPrivateKey(
                "12f431833272b16b43828317a9af631dc2d12b88974c4522e7b71250db0ec0c0");

            Unverifiable = new AccountFactory(Con).FromPublicKey(
                "53e140b5947f104cabc2d6fe8baedbc30ef9a0609c717d9613de593ec2a266d3");

            UnverifiableFromAddress = new AccountFactory(Con).FromEncodedAddress(
                "TALIC37D2B7KRFHGXRJAQO67YWOUWWA36OU46HSG");
        }

        private static void Main()
        {
            TransferTrans();
            //NameSpaceRental();
            //MosaicDefinition();
            MultisigSignatureTransaction();
        }

        private static void TransferTrans()
        {
            
            var mosaicList = new List<Mosaic>
           {
               new Mosaic("kod_multisig", "non_business_without_levy", 300000000),
               new Mosaic("kod_multisig", "non_business_without_levy_second", 300000000)
           };
          
            var transferData = new TransferTransactionData
            {
                Amount = 4000000,
                Message = "f",
                Encrypted = true, // encoding still broken, encrypts wrapper to wrapper successfully but not compatible with ncc
                Recipient = UnverifiableFromAddress,
                ListOfMosaics = mosaicList,
                MultisigAccount = Multisig.PublicKey
            };

            var transferResponse = Cosig1.SendTransactionAsync(transferData).Result;

            Console.WriteLine(transferResponse.Message);
            Console.WriteLine(transferResponse.Code);
            Console.ReadLine();
        }

        private static void NameSpaceRental()
        {
            var provData = new ProvisionNameSpaceData
            {
                NewPart = "kod_multisig",
                //Parent = "kod_multisig1"
                MultisigAccount = Multisig.PublicKey
            };
            
            var provisionResponse = Cosig1.ProvisionNamespaceAsync(provData).Result;

            Console.WriteLine(provisionResponse.Message);
            Console.ReadKey();
        }

        private static void MosaicDefinition()
        {
            var lev = new MosaicLevy(Cosig1.Address, new Mosaic("kod_multisig", "non_business_levy", 100000000), 1);
            
            var mosaicDefCreate = new MosaicCreationData
            {
                Divisibility = 3,
                SupplyMutable = true,
                InitialSupply = 9000000,
                NameSpaceId = "kod_multisig",
                Description = "testing",
                Transferable = true,
                MosaicName = "non_business_without_levy_second",
                //MosaicLevy = lev,
                MultisigAccount = Multisig.PublicKey
            };
            
            var definitionResponse = Cosig1.CreateMosaicAsync(mosaicDefCreate).Result;

            Console.WriteLine(definitionResponse.Message);
            Console.ReadLine();
        }

        private static void ImportanceTransfer()
        {
            var impData = new ImportanceTransferData
            {
                Activate = false,
                DelegatedAccount = DelegatedAccount.PublicKey,
                MultisigAccount = Multisig.PublicKey            
            };
            
            var importanceResponse = Cosig1.ImportanceTransferAsync(impData).Result;

            Console.WriteLine(importanceResponse.Message);
            Console.ReadLine();
        }

        private static void MultisigSignatureTransaction()
        {
            Console.WriteLine("no of unconfirmed transactions:");
            Console.WriteLine(Multisig.GetUnconfirmedTransactionsAsync().Result.data.Count);
            Console.WriteLine();
            var multiSigSignature = new MultisigSignatureTransactionData
            {
                MultisigAddress = Multisig.Address,
                TransactionHash = Multisig.GetUnconfirmedTransactionsAsync().Result.data[0].meta.data
            };
            
            var signatureResponse = Cosig2.SignMultisigTransaction(multiSigSignature).Result;

            Console.WriteLine(signatureResponse.Message);
            Console.ReadLine();
        }

        private static void AggregateMultisigModification()
        {
            var modList = new List<AggregateModification>
            {
                new AggregateModification(Cosig3.PublicKey, 2)
            };
            
            var modData = new AggregateModificationData
            {
                Modifications = modList,
                MultisigAccount = Multisig.PublicKey,
                RelativeChange = -1,    
            };
            
            var aggModResponse = Cosig1.AggregateMultisigModificationAsync(modData).Result;

            Console.WriteLine(aggModResponse.Message);
            Console.ReadLine();
        }

        private static void MosaicSupplyChange()
        {
            var changeData = new MosaicSupplyChangeData
            {
                NameSpaceId = "",
                MosaicName = "",
                SupplyChangeType = 0,
                Delta = 0,
                MultisigAccount = Multisig.PublicKey
            };
            
            var supplyChangeResponse = Cosig1.MosaicSupplychangeAsync(changeData).Result;

            Console.WriteLine(supplyChangeResponse.Message);
            Console.ReadLine();
        }

        private static void OtherDataRequests()
        {
            var newKeyPair = Unverifiable.GetGenerateNewAccountAsync().Result; // needs to be taken out of UnverifiableAccount class
            
            var result01 =  Unverifiable.GetAccountInfoAsync().Result;
            var result02 =  Unverifiable.GetAccountStatusAsync().Result;
            
            // account transactions
            var result03 = Unverifiable.GetAllTransactionsAsync().Result;
            var result04 = Unverifiable.GetIncomingTransactionsAsync().Result;
            var result05 = Unverifiable.GetOutgoingTransactionsAsync().Result;
            var result06 = Unverifiable.GetUnconfirmedTransactionsAsync().Result;
            
            // other
            var result07 = Unverifiable.GetDelegatedAccountRootAsync().Result;
            var result08 = Unverifiable.GetHarvestingInfoAsync().Result;
            var result09 = Unverifiable.GetImportancesAsync().Result;
            
            // account namespace and mosaic requests
            var result10 = Unverifiable.GetMosaicsAsync().Result;
            var result11 = Unverifiable.GetMosaicsByNameSpaceAsync("<namespace>", "<Optional ID>").Result;
            var result12 = Unverifiable.GetMosaicsOwnedAsync().Result;
            var result13 = Unverifiable.GetNamespacesAsync().Result;
            
            // node to which request sent must have historic data retrieval enabled.
            var result14 = Unverifiable.HistoricData(Cosig1.Address.Encoded, 0, 100, 2).Result;
        }

        private static string ReverseStringToBytes(string a)
        {
            var twos = new string[a.Length];

            for (var x = 0; x < a.Length - 1; x += 2)
            {
                twos[x] = string.Concat("0x", a.Substring(x, 2), ", ");
            }

            return twos.Reverse().Aggregate("", (current, t) => current + t);
        }

        private static string ReverseLongToBytes(long l)
        {
            var a = l.ToString("X");

            a = a.PadLeft(16, '0');

            var twos = new string[a.Length];

            for (var x = 0; x < a.Length - 1; x += 2)
            {
                twos[x] = string.Concat("0x", a.Substring(x, 2), ", ");
            }

            return twos.Reverse().Aggregate("", (current, t) => current + t);
        }

    }
}

