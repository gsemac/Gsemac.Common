using Gsemac.IO.Extensions;
using Gsemac.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class PeekBufferStreamReaderTests {

        // Public members

        // Peek

        [TestMethod]
        public void TestPeekWithMultipleCharacters() {

            using Stream stream = StringUtilities.StringToStream("hello world!");
            using PeekBufferStreamReader reader = new(stream);

            Assert.AreEqual("hello", reader.PeekString(5));
            Assert.AreEqual("hello ", reader.ReadString(6));
            Assert.AreEqual("world", reader.PeekString(5));

        }

    }

}