using Gsemac.IO.FileFormats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class TextFileSignatureTests {

        // IsMatch

        [TestMethod]
        public void TestIsMatchWithFullMatch() {

            TextFileSignature signature = new TextFileSignature("<html ");

            using (Stream stream = StreamUtilities.StringToStream("<html "))
                Assert.IsTrue(signature.IsMatch(stream));

        }
        [TestMethod]
        public void TestIsMatchWithPartialMatch() {

            TextFileSignature signature = new TextFileSignature("<html ");

            using (Stream stream = StreamUtilities.StringToStream("<ht"))
                Assert.IsFalse(signature.IsMatch(stream));

        }
        [TestMethod]
        public void TestIsMatchWithFullMatchAndLeadingWhiteSpace() {

            TextFileSignature signature = new TextFileSignature("<html ", FileSignatureOptions.IgnoreLeadingWhiteSpace);

            using (Stream stream = StreamUtilities.StringToStream("              <html "))
                Assert.IsTrue(signature.IsMatch(stream));

        }
        [TestMethod]
        public void TestIsMatchWithFullMatchAndCaseSensitivity() {

            TextFileSignature signature = new TextFileSignature("<html ");

            using (Stream stream = StreamUtilities.StringToStream("<HTML "))
                Assert.IsFalse(signature.IsMatch(stream));

        }
        [TestMethod]
        public void TestIsMatchWithFullMatchAndCaseIsensitivity() {

            TextFileSignature signature = new TextFileSignature("<html ", FileSignatureOptions.CaseInsensitive);

            using (Stream stream = StreamUtilities.StringToStream("<HTML "))
                Assert.IsTrue(signature.IsMatch(stream));

        }

    }

}