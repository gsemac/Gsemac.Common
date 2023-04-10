using Gsemac.IO.FileFormats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class JpegFileFormatTests {

        [TestMethod]
        public void TestExtensionReturnsJpgExtension() {

            // The default file extension for the JPEG format should be "jpg".
            // Even though this is a shortened version of the "jpeg" extension, it's the one most commonly used.

            Assert.AreEqual(".jpg", ImageFormat.Jpeg.Extensions.FirstOrDefault());

        }

    }

}