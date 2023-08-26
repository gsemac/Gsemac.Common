using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Net.Http.Headers.Tests {

    [TestClass]
    public class ContentDispositionHeaderValueTests {

        // Parse

        [TestMethod]
        public void TestParseWithContentDispositionType() {

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("attachment");

            Assert.AreEqual(ContentDispositionType.Attachment, value.Type);

        }
        [TestMethod]
        public void TestParseWithNameParameter() {

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("form-data; name=\"fieldName\"; filename=\"filename.jpg\"");

            Assert.AreEqual(ContentDispositionType.FormData, value.Type);
            Assert.AreEqual("fieldName", value.Name);

        }
        [TestMethod]
        public void TestParseWithFileNameParameter() {

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("form-data; filename=filename.jpg");

            Assert.AreEqual(ContentDispositionType.FormData, value.Type);
            Assert.AreEqual("filename.jpg", value.FileName);

        }
        [TestMethod]
        public void TestParseWithQuotedFileNameParameter() {

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("form-data; filename=\"filename.jpg\"");

            Assert.AreEqual(ContentDispositionType.FormData, value.Type);
            Assert.AreEqual("filename.jpg", value.FileName);

        }
        [TestMethod]
        public void TestParseWithFileNameStarParameterAndFileNameParameter() {

            // The "filename*" parameter takes precedence.

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("form-data; filename=\"filename.jpg\"; filename*=filename2.jpg");

            Assert.AreEqual(ContentDispositionType.FormData, value.Type);
            Assert.AreEqual("filename2.jpg", value.FileName);

        }
        [TestMethod]
        public void TestParseWithFileNameStarParameterWithSpecifiedEncoding() {

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("attachment; filename*=UTF-8''Na%C3%AFve%20file.txt");

            Assert.AreEqual(ContentDispositionType.Attachment, value.Type);
            Assert.AreEqual("Naïve file.txt", value.FileName);

        }

        // ToString

        [TestMethod]
        public void TestToStringWithContentDispositionType() {

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("attachment");

            Assert.AreEqual("attachment", value.ToString());

        }
        [TestMethod]
        public void TestToStringWithFileNameParameter() {

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("form-data; filename=filename.jpg");

            Assert.AreEqual("form-data; filename=\"filename.jpg\"", value.ToString());

        }
        [TestMethod]
        public void TestToStringWithFileNameStarParameterWithSpecifiedEncoding() {

            ContentDispositionHeaderValue value = ContentDispositionHeaderValue.Parse("attachment; filename*=UTF-8''Na%C3%AFve%20file.txt");

            Assert.AreEqual("attachment; filename*=\"UTF-8''Na%C3%AFve%20file.txt\"", value.ToString());

        }

    }

}