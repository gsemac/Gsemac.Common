using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class FileFormatTests {

        // Equals

        [TestMethod]
        public void TestEqualsWithSameFileExtension() {

            IFileFormatFactory fileFormatFactory = FileFormatFactory.Default;

            IFileFormat lhs = fileFormatFactory.FromFileExtension(".jpg");
            IFileFormat rhs = fileFormatFactory.FromFileExtension(".jpg");

            Assert.IsTrue(lhs.Equals(rhs));

        }
        [TestMethod]
        public void TestEqualsWithEquivalentFileExtension() {

            IFileFormatFactory fileFormatFactory = FileFormatFactory.Default;

            IFileFormat lhs = fileFormatFactory.FromFileExtension(".jpg");
            IFileFormat rhs = fileFormatFactory.FromFileExtension(".jpeg");

            Assert.IsTrue(lhs.Equals(rhs));

        }
        [TestMethod]
        public void TestEqualsWithDifferentFileExtensions() {

            IFileFormatFactory fileFormatFactory = FileFormatFactory.Default;

            IFileFormat lhs = fileFormatFactory.FromFileExtension(".jpg");
            IFileFormat rhs = fileFormatFactory.FromFileExtension(".png");

            Assert.IsFalse(lhs.Equals(rhs));

        }

    }

}