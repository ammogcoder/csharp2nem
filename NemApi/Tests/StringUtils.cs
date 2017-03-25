using Microsoft.VisualStudio.TestTools.UnitTesting;
using NemApi;

namespace Tests
{
    [TestClass]
    public class StringUtilsTests
    {
        [TestMethod]
        public void CanConvertToAndFromSecureString()
        {
            var privateKey = "abf4cf55a2b3f742d7543d9cc17f50447b969e6e06f5ea9195d428ab12b7318d";

            var expected = "abf4cf55a2b3f742d7543d9cc17f50447b969e6e06f5ea9195d428ab12b7318d";

            var secureKey = privateKey.ToSecureString();

            var result = StringUtils.ConvertToUnsecureString(secureKey);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CanConvertAddressToPrettyAddress()
        {
            var address = "NAUARLU4RMH2CW2UFWAIDAD73C5JYYSZ7ISSDYME";
            var result = StringUtils.GetResultsWithHyphen(address);
            var expected = "NAUARL-U4RMH2-CW2UFW-AIDAD7-3C5JYY-SZ7ISS-DYME";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CanConvertPrettyAddressToAddress()
        {
            var address = "NAUARL-U4RMH2-CW2UFW-AIDAD7-3C5JYY-SZ7ISS-DYME";
            var result = StringUtils.GetResultsWithoutHyphen(address);
            var expected = "NAUARLU4RMH2CW2UFWAIDAD73C5JYYSZ7ISSDYME";
            Assert.AreEqual(expected, result);
        }
    }
}
