using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.IO.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Gsemac.IO.Tests {

    [TestClass]
    public abstract class FileFormatFactoryFromFileTestsBase {

        // Public members

        [TestMethod]
        public void TestFileFormatFromFileReturnsExpectedFileFormat() {

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
            base(ImageFormat.Svg, "test.svg") {
        }

    }

}