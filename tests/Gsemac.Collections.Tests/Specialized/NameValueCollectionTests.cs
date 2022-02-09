using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Collections.Specialized.Tests {

    [TestClass]
    public class NameValueCollectionTests {

        [TestMethod]
        public void TestGetValueByKeyAfterAddingWithNullKey() {

            INameValueCollection items = new NameValueCollection {
                { null, "test" }
            };

            Assert.AreEqual(items[null], "test");

        }

    }

}