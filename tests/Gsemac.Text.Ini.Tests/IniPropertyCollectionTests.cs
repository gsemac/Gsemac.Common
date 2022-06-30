using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniPropertyCollectionTests {

        // this[]

        [TestMethod]
        public void TestGetPropertyWithInvalidPropertyNameReturnsNull() {

            IIniProperty property = new IniPropertyCollection()[string.Empty];

            Assert.IsTrue(property is null);

        }
        [TestMethod]
        public void TestGetPropertyWithNullPropertyNameReturnsNull() {

            IIniProperty property = new IniPropertyCollection()[null];

            Assert.IsTrue(property is null);

        }
        [TestMethod]
        public void TestGetPropertyWithEmptyAndNullPropertyNamesReturnsSameObject() {

            IIniPropertyCollection items = new IniPropertyCollection {
                { string.Empty, "value1" },
                { null, "value2" }
            };

            IIniProperty property1 = items[string.Empty];
            IIniProperty property2 = items[null];

            Assert.IsTrue(ReferenceEquals(property1, property2));

        }

        // Contains

        [TestMethod]
        public void TestLookupIsCaseInsensitiveByDefault() {

            IIniPropertyCollection items = new IniPropertyCollection() {
                new IniProperty("key", "value"),
            };

            Assert.IsTrue(items.Contains("key"));
            Assert.IsTrue(items.Contains("KEY"));

        }
        [TestMethod]
        public void TestLookupWithCaseSensitiveKeyComparer() {

            IIniPropertyCollection items = new IniPropertyCollection(StringComparer.Ordinal) {
                new IniProperty("key", "value"),
            };

            Assert.IsTrue(items.Contains("key"));
            Assert.IsFalse(items.Contains("KEY"));

        }

    }

}