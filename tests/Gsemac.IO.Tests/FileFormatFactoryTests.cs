using Gsemac.IO.FileFormats;
using Gsemac.IO.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class FileFormatFactoryTests {

        [TestMethod]
        public void TestFromFileIgnoreFilesExtensionForExistingFiles() {

            // If a given file exists, attempt to read it and determine the file type from its signature.
            // It should get the correct file type even if the file extension does not match.

            string filePath = Path.Combine(SamplePaths.ImagesSamplesDirectoryPath, "svg_with_incorrect_extension.jpg");
            IFileFormat format = FileFormatFactory.Default.FromFile(filePath);

            Assert.IsTrue(File.Exists(filePath));
            Assert.AreEqual(ImageFormat.Svg, format);

        }
        [TestMethod]
        public void TestFromFileUsesFileExtensionForNonExistentFile() {

            // If a given file does not exist, derive the file type from the extension instead.

            string filePath = "this_file_does_not_exist.svg";
            IFileFormat format = FileFormatFactory.Default.FromFile(filePath);

            Assert.IsFalse(File.Exists(filePath));
            Assert.AreEqual(ImageFormat.Svg, format);

        }

    }

    [TestClass]
    public abstract class FileFormatFactoryFromFileTestsBase {

        // Public members

        [TestMethod]
        public void TestFromFileReturnsExpectedFileFormat() {

            IFileFormatFactory factory = new FileFormatFactory(new[] {
                fileFormat,
            });

            Assert.AreEqual(fileFormat, factory.FromFile(Path.Combine(SamplePaths.ImagesSamplesDirectoryPath, testFileName)));

        }

        // Protected members

        protected FileFormatFactoryFromFileTestsBase(IFileFormat fileFormat, string testFileName) {

            this.fileFormat = fileFormat;
            this.testFileName = testFileName;

        }

        // Private members

        private readonly IFileFormat fileFormat;
        private readonly string testFileName;

    }

    [TestClass]
    public class SvgFileFormatFactoryFromFileTests :
        FileFormatFactoryFromFileTestsBase {

        public SvgFileFormatFactoryFromFileTests() :
            base(ImageFormat.Svg, "static.svg") {
        }

    }

}