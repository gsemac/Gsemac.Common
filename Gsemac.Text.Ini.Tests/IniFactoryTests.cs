using Gsemac.Text.Ini.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Gsemac.Text.Ini.Extensions;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniFactoryTests {

        // Public members

        // Parse

        [TestMethod]
        public void TestParseReadsGlobalProperties() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithGlobalProperties.ini"));

            Assert.AreEqual(3, ini.Global.Properties.Count);
            Assert.AreEqual("value1", ini.Get("key1"));
            Assert.AreEqual("value2", ini.Get("key2"));
            Assert.AreEqual("value3", ini.Get("key3"));

        }
        [TestMethod]
        public void TestParseReadsSections() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithSections.ini"));

            Assert.AreEqual(2, ini.Sections.Count);
            Assert.AreEqual("value1", ini.Get("section1", "key1"));
            Assert.AreEqual("value2", ini.Get("section1", "key2"));
            Assert.AreEqual("value3", ini.Get("section2", "key3"));

        }
        [TestMethod]
        public void TestParseReadsGlobalPropertiesAndSections() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithGlobalPropertiesAndSections.ini"));

            Assert.AreEqual("value1", ini.Get("key1"));
            Assert.AreEqual("value2", ini.Get("section1", "key2"));

        }

        // Private members

        private static string ReadAllText(string filePath) {

            return File.ReadAllText(Path.Combine(SamplePaths.IniSamplesDirectoryPath, filePath));

        }

    }

}