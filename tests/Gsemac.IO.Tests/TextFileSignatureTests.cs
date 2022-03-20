using Gsemac.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class TextFileSignatureTests {

        // IsMatch

        [TestMethod]
        public void TestIsMatchWithFullMatch() {

            TextFileSignature signature = new("<html ");

            using Stream stream = StringUtilities.StringToStream("<html ");

            Assert.IsTrue(signature.IsMatch(stream));

        }
        [TestMethod]
        public void TestIsMatchWithPartialMatch() {

            TextFileSignature signature = new("<html ");

            using Stream stream = StringUtilities.StringToStream("<ht");

            Assert.IsFalse(signature.IsMatch(stream));

        }
        [TestMethod]
        public void TestIsMatchWithFullMatchAndLeadingWhiteSpace() {

            TextFileSignature signature = new("<html ", FileSignatureOptions.IgnoreLeadingWhiteSpace);

            using Stream stream = StringUtilities.StringToStream("              <html ");

            Assert.IsTrue(signature.IsMatch(stream));

        }
        [TestMethod]
        public void TestIsMatchWithFullMatchAndCaseSensitivity() {

            TextFileSignature signature = new("<html ");

            using Stream stream = StringUtilities.StringToStream("<HTML ");

            Assert.IsFalse(signature.IsMatch(stream));

        }
        [TestMethod]
        public void TestIsMatchWithFullMatchAndCaseIsensitivity() {

            TextFileSignature signature = new("<html ", FileSignatureOptions.CaseInsensitive);

            using Stream stream = StringUtilities.StringToStream("<HTML ");

            Assert.IsTrue(signature.IsMatch(stream));

        }

    }

}