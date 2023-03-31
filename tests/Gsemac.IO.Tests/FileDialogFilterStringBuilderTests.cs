using Gsemac.IO.FileFormats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.IO.Tests {

    [TestClass]
    public class FileDialogFilterStringBuilderTests {

        [TestMethod]
        public void TestBuildWithMultipleFileFormats() {

            string filterString = new FileDialogFilterStringBuilder()
                .WithFileFormat(ImageFormat.Png)
                .WithFileFormat(ImageFormat.Jpeg)
                .WithAllFilesOption()
                .Build();

            Assert.AreEqual("PNG Images (*.png)|(*.png)|JPEG Images (*.jpeg;*.jfi;*.jfif;*.jif;*.jpe;*.jpg)|(*.jpeg;*.jfi;*.jfif;*.jif;*.jpe;*.jpg)|All Files (*.*)|*.*", filterString);

        }
        [TestMethod]
        public void TestBuildWithDuplicateFileFormats() {

            // Duplicate formats should be removed.

            string filterString = new FileDialogFilterStringBuilder()
                .WithFileFormat(ImageFormat.Jpeg)
                .WithFileFormat(ImageFormat.Jpeg)
                .WithAllFilesOption()
                .Build();

            Assert.AreEqual("JPEG Images (*.jpeg;*.jfi;*.jfif;*.jif;*.jpe;*.jpg)|(*.jpeg;*.jfi;*.jfif;*.jif;*.jpe;*.jpg)|All Files (*.*)|*.*", filterString);

        }

    }

}