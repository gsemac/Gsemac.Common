using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniSectionCollectionTests {

        // this[]

        [TestMethod]
        public void TestGetTransientSectionDoesNotResultInVisibleSection() {

            IIniSectionCollection items = new IniSectionCollection();

            // Accessing a section that doesn't exist in the collection should create a "transient" section.
            // These sections functionally don't appear in the collection until items are added to them.
            // This is to permit the items["section"]["key"] syntax even in the case the section doesn't already exist.

            IIniSection transientSection = items["section_name"];

            Assert.AreEqual(0, items.Count);

            Assert.IsTrue(items.Get(transientSection.Name) is null);

            Assert.IsFalse(items.Contains(transientSection.Name));
            Assert.IsFalse(items.Contains(transientSection));

        }

    }

}