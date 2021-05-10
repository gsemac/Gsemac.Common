using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class UnbufferedStreamReaderTests {

        [TestMethod]
        public void TestPeekWithSingleByteCharacter() {

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("abc")))
            using (UnbufferedStreamReader sr = new UnbufferedStreamReader(ms, Encoding.UTF8)) {

                Assert.AreEqual('a'.ToString(), ((char)sr.Peek()).ToString());

            }

        }
        [TestMethod]
        public void TestPeekWithMultibyteCharacter() {

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("À")))
            using (UnbufferedStreamReader sr = new UnbufferedStreamReader(ms, Encoding.UTF8)) {

                Assert.AreEqual('À'.ToString(), ((char)sr.Peek()).ToString());

            }

        }

    }

}