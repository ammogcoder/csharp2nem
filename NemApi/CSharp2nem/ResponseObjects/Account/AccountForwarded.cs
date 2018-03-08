using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Account
{
    /// <summary>
    /// Contains a number of account forwarded properties.
    /// </summary>
    /// <remarks>
    /// If the queried account is not a delegated account, account details of the quried account will be returned. Otherwise, the root account details are returned.
    /// </remarks>
    public class AccountForwarded
    {
        /// <summary>
        /// The root account or quried account
        /// </summary>
        public class Account
        {
            /// <summary>
            /// The root account address
            /// </summary>
            /// <remarks>
            /// If the queried account is not a delegated account, account details of the quried account will be returned. Otherwise, the root account details are returned.
            /// </remarks>
            public string Address { get; set; }
            /// <summary>
            /// The account balance
            /// </summary>
            public long Balance { get; set; }
            /// <summary>
            /// The root account vested balance
            /// </summary>
            /// <remarks>
            /// Vested balance vests at a rate of 10% per day.
            /// </remarks>
            public long VestedBalance { get; set; }
            /// <summary>
            /// The account importance score.
            /// </summary>
            public double Importance { get; set; }
            /// <summary>
            /// The account public key.
            /// </summary>
            public string PublicKey { get; set; }
            /// <summary>
            /// The account label.
            /// </summary>
            public object Label { get; set; }
            /// <summary>
            /// The number of blocks harvested.
            /// </summary>
            public int HarvestedBlocks { get; set; }
        }

        /// <summary>
        /// The account meta data.
        /// </summary>
        public class Meta
        {
            /// <summary>
            /// The accounts that the account is a cosignaotry for.
            /// </summary>
            public List<ExistingAccount.CosignatoryOf> CosignatoryOf { get; set; }
            /// <summary>
            /// The accounts that are cosignatories of this account.
            /// </summary>
            public List<ExistingAccount.Cosignatory> Cosignatories { get; set; }
            /// <summary>
            /// The satus for the account.
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// The remote status of the account
            /// </summary>
            /// <remarks>
            /// Active indicates that the account has activated delegated harvesting.
            /// </remarks>
            public string RemoteStatus { get; set; }
        }

        /// <summary>
        /// The account data.
        /// </summary>
        public class Data
        {
            /// <summary>
            /// The account specific data.
            /// </summary>
            public Account Account { get; set; }
            /// <summary>
            /// The account meta data.
            /// </summary>
            public Meta Meta { get; set; }
        }
    }
}