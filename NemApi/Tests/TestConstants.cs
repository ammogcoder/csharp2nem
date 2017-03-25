using System.Collections.Generic;
using NemApi;
namespace Tests
{
    public class TestConstants
    {
        public const string Address = "TCEIV52VWTGUTMYOXYPVTGMGBMQC77EH4MBJRSNT";
        public const string PubKey = "09ac855e55fad630bdfbd52e08c54e520524e6f9bbd14844a2b0ecca66cae6a0";
        public const string PrivKey = "fcdadb68356c6227a0942b377209401574ece844e8e579edbfe36a5193cf8cb5";

        public static List<PublicKey> ListOfPublicKeys = new List<PublicKey>
        {
            new PublicKey("c559463bf86320eeac6c846a124cde5e6f457108c1d852e54ea55611d1e545bb"),
            new PublicKey("72d0e65f1ede79c4af0ba7ec14204e10f0f7ea09f2bc43259cd60ea8c3a087e2"),
            new PublicKey("3ec8923f9ea5ea14f8aaa7e7c2784653ed8c7de44e352ef9fc1dee81fc3fa1a3")
        };
    }
}
