using Gsemac.IO.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace Gsemac.IO.Tests.Extensions {

    [TestClass]
    public class StreamExtensionsTests {

        [TestMethod]
        public void TestToArrayWithNullStream() {

            Assert.ThrowsException<ArgumentNullException>(() => ((Stream)null).ToArray());

        }
        [TestMethod]
        public void TestToArrayWithEmptyStream() {

            using (Stream ms = new MemoryStream())
                Assert.AreEqual(0, ms.ToArray().Length);

        }
        [TestMethod]
        public void TestToArrayWithStreamWithPositivePosition() {

            // The entire contents of the stream are returned regardless of its position.

            using (Stream ms = new MemoryStream(Encoding.UTF8.GetBytes("hello world"))) {

                ms.Seek(5, SeekOrigin.Begin);

                Assert.AreEqual("hello world", Encoding.UTF8.GetString(ms.ToArray()));

            }

        }

    }

}