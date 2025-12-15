using Gsemac.Text.Ini.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Gsemac.Text.Ini.Tests
{

    [TestClass]
    public class IniFactoryTests {

        // Public members

        // Parse

        [TestMethod]
        public void TestParseReadsGlobalProperties() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithGlobalProperties.ini"));

            Assert.HasCount(3, ini.Global.Properties);

            Assert.AreEqual("value1", ini.GetValue("key1"));
            Assert.AreEqual("value2", ini.GetValue("key2"));
            Assert.AreEqual("value3", ini.GetValue("key3"));

        }
        [TestMethod]
        public void TestParseReadsSections() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithSections.ini"));

            Assert.HasCount(2, ini.Sections);

            Assert.AreEqual("value1", ini.GetValue("section1", "key1"));
            Assert.AreEqual("value2", ini.GetValue("section1", "key2"));
            Assert.AreEqual("value3", ini.GetValue("section2", "key3"));

        }
        [TestMethod]
        public void TestParseReadsGlobalPropertiesAndSections() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithGlobalPropertiesAndSections.ini"));

            Assert.AreEqual("value1", ini.GetValue("key1"));
            Assert.AreEqual("value2", ini.GetValue("section1", "key2"));

        }

        [TestMethod]
        public void TestParseWithMixedWhiteSpaceAndTrimWhiteSpaceDisabled() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithMixedWhiteSpace.ini"), new IniOptions() {
                TrimWhiteSpace = true,
            });

            Assert.HasCount(2, ini.Sections);

            Assert.IsTrue(ini.Sections.Contains("section1"));
            Assert.IsTrue(ini.Sections.Contains("section2"));

            Assert.AreEqual("value1", ini.GetValue("section1", "key1"));
            Assert.AreEqual("value2", ini.GetValue("section1", "key2"));
            Assert.AreEqual("value3", ini.GetValue("section2", "key3"));
            Assert.AreEqual("value4", ini.GetValue("section2", "key4"));

        }
        [TestMethod]
        public void TestParseWithMixedWhiteSpaceAndTrimWhiteSpaceEnabled() {

            // Only whitespace surrounding property values is preserved.

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithMixedWhiteSpace.ini"), new IniOptions() {
                TrimWhiteSpace = false,
            });

            Assert.HasCount(2, ini.Sections);

            Assert.IsTrue(ini.Sections.Contains("section1"));
            Assert.IsTrue(ini.Sections.Contains("section2"));

            Assert.AreEqual("	value1", ini.GetValue("section1", "key1"));
            Assert.AreEqual(" value2", ini.GetValue("section1", "key2"));
            Assert.AreEqual("   value3   ", ini.GetValue("section2", "key3"));
            Assert.AreEqual("value4", ini.GetValue("section2", "key4"));

        }
        [TestMethod]
        public void TestParseWithUnixLineSeparators() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithUnixLineSeparators.ini"));

            Assert.HasCount(2, ini.Sections);

            Assert.IsTrue(ini.Sections.Contains("section1"));
            Assert.IsTrue(ini.Sections.Contains("section2"));

            Assert.AreEqual("value1", ini.GetValue("section1", "key1"));
            Assert.AreEqual("value2", ini.GetValue("section1", "key2"));
            Assert.AreEqual("value3", ini.GetValue("section2", "key3"));
            Assert.AreEqual("value4", ini.GetValue("section2", "key4"));

        }

        [TestMethod]
        public void TestParseWithCommentsAndCommentsEnabled() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithComments.ini"), new IniOptions() {
                EnableComments = true,
                EnableInlineComments = true,
            });

            Assert.IsTrue(ini.Sections.Contains("section"));

            Assert.AreEqual("value", ini.GetValue("section", "key"));

        }
        [TestMethod]
        public void TestParseWithCommentsAndCommentsDisabled() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithComments.ini"), new IniOptions() {
                EnableComments = false,
            });

            Assert.IsTrue(ini.ContainsKey("[section] ; comment"));
            Assert.IsTrue(ini.ContainsKey("; comment"));

            Assert.AreEqual("value ; comment", ini.GetValue("key"));

        }

        [TestMethod]
        public void TestParseWithEscapeSequencesAndEscapeSequencesEnabled() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithEscapedStrings.ini"), new IniOptions() {
                EnableEscapeSequences = true,
            });

            string sectionName = "section[]";

            Assert.HasCount(4, ini.Sections[sectionName].Properties);

            Assert.AreEqual("value1", ini.GetValue(sectionName, "key1="));
            Assert.AreEqual("val;ue2", ini.GetValue(sectionName, "key2"));
            Assert.AreEqual("val\r\nue3", ini.GetValue(sectionName, "key3"));
            Assert.AreEqual("value4", ini.GetValue(sectionName, ";key4"));

        }
        [TestMethod]
        public void TestParseWithEscapedStringsAndEscapeSequencesDisabled() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithEscapedStrings.ini"), new IniOptions() {
                EnableEscapeSequences = false,
            });

            string sectionName = @"section\[\]";

            ini.Save("out.ini");

            Assert.AreEqual(5, ini.Sections[sectionName]?.Properties.Count ?? 0);

            Assert.AreEqual("=value1", ini.GetValue(sectionName, @"key1\"));
            Assert.AreEqual(@"val\;ue2", ini.GetValue(sectionName, "key2"));
            Assert.AreEqual(@"val\", ini.GetValue(sectionName, "key3"));
            Assert.AreEqual("value4", ini.GetValue(sectionName, @"\;key4"));

            Assert.IsTrue(ini.ContainsKey(sectionName, "ue3"));

        }

        [TestMethod]
        public void TestParseReadsSectionNameWithMultipleSectionNameEndCharacters() {

            IIni ini = IniFactory.Default.Parse("[section]]]]");

            Assert.HasCount(1, ini.Sections);
            Assert.IsTrue(ini.Sections.Contains("section]]]"));

        }
        [TestMethod]
        public void TestParseReadsSectionNameWithInlineComment() {

            IIni ini = IniFactory.Default.Parse("[section]]]] ; comment", new IniOptions() {
                EnableInlineComments = true,
            });

            Assert.HasCount(1, ini.Sections);
            Assert.IsTrue(ini.Sections.Contains("section]]]"));

        }
        [TestMethod]
        public void TestParseReadsInvalidSectionNameAsProperty() {

            IIni ini = IniFactory.Default.Parse("[section=value");

            Assert.IsEmpty(ini.Sections);
            Assert.AreEqual("value", ini.GetValue("[section"));

        }

        [TestMethod]
        public void TestParseReadsPropertyWithNameValueDelimiterInValue() {

            IIni ini = IniFactory.Default.Parse("key==value");

            Assert.AreEqual("=value", ini.GetValue("key"));

        }
        [TestMethod]
        public void TestParseReadsPropertyWithInlineCommentAndInlineCommentsEnabled() {

            IIni ini = IniFactory.Default.Parse("key=value ;comment", new IniOptions() {
                EnableInlineComments = true,
            });

            Assert.AreEqual("value", ini.GetValue("key"));

        }
        [TestMethod]
        public void TestParseReadsPropertyWithInlineCommentAndInlineCommentsDisabled() {

            IIni ini = IniFactory.Default.Parse("key=value ;comment", new IniOptions() {
                EnableInlineComments = false,
            });

            Assert.AreEqual("value ;comment", ini.GetValue("key"));

        }
        [TestMethod]
        public void TestParseWithEmptyProperties() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithEmptyProperties.ini"));

            Assert.HasCount(3, ini.Global.Properties);

            Assert.AreEqual(string.Empty, ini.GetValue("key1"));
            Assert.AreEqual(string.Empty, ini.GetValue("key2"));
            Assert.AreEqual(string.Empty, ini.GetValue("key3"));

        }

        [TestMethod]
        public void TestParseWithDuplicateSectionsMergesProperties() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithDuplicateSections.ini"));

            Assert.HasCount(1, ini.Sections);
            Assert.HasCount(2, ini.Sections.First().Properties);

            Assert.AreEqual("value1", ini.Sections.First()["key1"]);
            Assert.AreEqual("value2", ini.Sections.First()["key2"]);

        }
        [TestMethod]
        public void TestParseWithDuplicatePropertiesReplacesValue() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithDuplicateProperties.ini"));

            Assert.AreEqual("value2", ini.GetValue("key"));

        }

        // Private members

        private static string ReadAllText(string filePath) {

            return File.ReadAllText(Path.Combine(SamplePaths.IniSamplesDirectoryPath, filePath));

        }

    }

}