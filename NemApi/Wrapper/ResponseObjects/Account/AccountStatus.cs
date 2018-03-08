using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Account
{

    /// <summary>
    /// The status for the account.
    /// </summary>
    public class AccountStatus
    {
        /// <summary>
        /// JSON array of AccountInfo structures. The account is cosignatory for each of the accounts in the array.
        /// </summary>
        public List<ExistingAccount.CosignatoryOf> CosignatoryOf { get; set; }

        /// <summary>
        /// JSON array of AccountInfo structures. The array holds all accounts that are a cosignatory for this account.
        /// </summary>
        public List<ExistingAccount.Cosignatory> Cosignatories { get; set; }

        /// <summary>
        /// The harvesting status of a queried account.
        /// </summary>
        /// <remarks>
        /// The harvesting status can be one of the following values:
        /// 
        ///"UNKNOWN": The harvesting status of the account is not known.
        ///"LOCKED": The account is not harvesting.
        ///"UNLOCKED": The account is harvesting.
        /// </remarks>
        public string Status { get; set; }

        /// <summary>
        /// The status of remote harvesting of a queried account.
        /// </summary>
        /// <remarks>
        /// The remote harvesting status can be one of the following values:
        /// 
        ///"REMOTE": The account is a remote account and therefore remoteStatus is not applicable for it.
        ///"ACTIVATING": The account has activated remote harvesting but it is not yet active.
        ///"ACTIVE": The account has activated remote harvesting and remote harvesting is active.
        ///"DEACTIVATING": The account has deactivated remote harvesting but remote harvesting is still active.
        ///"INACTIVE": The account has inactive remote harvesting, or it has deactivated remote harvesting and deactivation is operational.
        /// </remarks>
        public string RemoteStatus { get; set; }
    }
}
