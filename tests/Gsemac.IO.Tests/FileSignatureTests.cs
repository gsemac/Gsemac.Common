using Gsemac.IO.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class FileSignatureTests {

        // IsMatch

        [TestMethod]
        public void TestIsMatchWithFullMatch() {

            IFileSignature signature = new FileSignature(0x1, 0x2, 0x3);

            Assert.IsTrue(signature.IsMatch(new byte[] { 0x1, 0x2, 0x3 }));

        }
        [TestMethod]
        public void TestIsMatchWithFullMatchWithNullBytes() {

            // Null bytes are treated as "any", and will match any byte.

            IFileSignature signature = new FileSignature(0x1, null, 0x3);

            Assert.IsTrue(signature.IsMatch(new byte[] { 0x1, 0x2, 0x3 }));

        }
        [TestMethod]
        public void TestIsMatchWithPartialMatch() {

            IFileSignature signature = new FileSignature(0x1, 0x2, 0x3);

            Assert.IsFalse(signature.IsMatch(new byte[] { 0x1, 0x2 }));

        }

    }

}