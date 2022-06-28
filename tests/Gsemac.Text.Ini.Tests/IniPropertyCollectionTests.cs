using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniPropertyCollectionTests {

        // Contains

        [TestMethod]
        public void TestLookupIsCaseSensitiveByDefault() {

            IIniPropertyCollection items = new IniPropertyCollection() {
                new IniProperty("key", "value"),
            };

            Assert.IsTrue(items.Contains("key"));
            Assert.IsFalse(items.Contains("KEY"));

        }
        [TestMethod]
        public void TestLookupWithCaseInsensitiveKeyComparer() {

            IIniPropertyCollection items = new IniPropertyCollection(StringComparer.OrdinalIgnoreCase) {
                new IniProperty("key", "value"),
            };

            Assert.IsTrue(items.Contains("key"));
            Assert.IsTrue(items.Contains("KEY"));

        }

    }

}