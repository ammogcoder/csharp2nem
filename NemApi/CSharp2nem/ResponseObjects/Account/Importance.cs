using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Account
{
    /// <summary>
    /// The account importances view model
    /// </summary>
    public class Importances
    {
        /// <summary>
        /// Substructure that describes the importance of the account
        /// </summary>
        public class Importance
        {
            /// <summary>
            /// Indicates if the fields "score", "ev" and "height" are available.isSet can have the values 0 or 1. In case isSet is 0 the fields are not available.
            /// </summary>
            public int IsSet { get; set; }
            /// <summary>
            /// The importance of the account. The importance ranges between 0 and 1.
            /// </summary>
            public double Score { get; set; }
            /// <summary>
            /// The page rank portion of the importance. The page rank ranges between 0 and 1.
            /// </summary>
            public double Ev { get; set; }
            /// <summary>
            /// The height at which the importance calculation was calculated
            /// </summary>
            public long Height { get; set; }
        }

        /// <summary>
        /// The account data
        /// </summary>
        public class Datum
        {
            /// <summary>
            /// The address of the account
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// The importance information for the account.
            /// </summary>
            public Importance Importance { get; set; }
        }

        /// <summary>
        /// List of account importances
        /// </summary>
        public class ListImportances
        {
            /// <summary>
            /// The list of importance data.
            /// </summary>
            public List<Datum> Data { get; set; }
        }
    }
}