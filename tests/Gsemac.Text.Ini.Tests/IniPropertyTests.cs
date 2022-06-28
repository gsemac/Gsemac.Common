using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniPropertyTests {

        // Value

        [TestMethod]
        public void TestChangesToValuesIsReflectedInValue() {

            IIniProperty property = new IniProperty("key", "1");

            property.Values.Add("2");
            property.Values.Add("3");
            property.Values.Add("4");

            Assert.AreEqual("1, 2, 3, 4", property.Value);

        }
        [TestMethod]
        public void TestPropertyCreatedWithoutValueHasEmptyStringAsValue() {

            // The "Value" property should never return null.
            // We can prove an INI property does not exist by checking for its key rather than checking for a null value.

            IIniProperty property = new IniProperty("key");

            Assert.AreEqual(string.Empty, property.Value);

        }
        [TestMethod]
        public void TestPropertyCreatedWithNullValueHasEmptyStringAsValue() {

            // The "Value" property should never return null.
            // We can prove an INI property does not exist by checking for its key rather than checking for a null value.

            IIniProperty property = new IniProperty("key", null);

            Assert.AreEqual(string.Empty, property.Value);

        }

        // Values

        [TestMethod]
        public void TestChangesToValueIsReflectedInValues() {

            IIniProperty property = new IniProperty("key");

            property.Values.Add("1");
            property.Values.Add("2");
            property.Values.Add("3");

            property.Value = "4";

            Assert.AreEqual(1, property.Values.Count);
            Assert.AreEqual("4", property.Values.First());

        }

        [TestMethod]
        public void TestPropertyCreatedWithoutValueHasNoValues() {

            IIniProperty property = new IniProperty("key");

            Assert.AreEqual(0, property.Values.Count);

        }
        [TestMethod]
        public void TestPropertyCreatedWithEmptyValueHasNoValues() {

            IIniProperty property = new IniProperty("key", string.Empty);

            Assert.AreEqual(0, property.Values.Count);

        }
        [TestMethod]
        public void TestPropertyCreatedWithNullValueHasNoValues() {

            IIniProperty property = new IniProperty("key", null);

            Assert.AreEqual(0, property.Values.Count);

        }

    }

}