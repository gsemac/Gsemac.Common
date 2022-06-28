using Gsemac.Text.Ini.Extensions;
using Gsemac.Text.Ini.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniFactoryTests {

        // Public members

        // Parse

        [TestMethod]
        public void TestParseReadsGlobalProperties() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithGlobalProperties.ini"));

            Assert.AreEqual(3, ini.Global.Properties.Count);
            Assert.AreEqual("value1", ini.GetValue("key1"));
            Assert.AreEqual("value2", ini.GetValue("key2"));
            Assert.AreEqual("value3", ini.GetValue("key3"));

        }
        [TestMethod]
        public void TestParseReadsSections() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithSections.ini"));

            Assert.AreEqual(2, ini.Sections.Count);
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
        public void TestParseWithMixedWhiteSpace() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithMixedWhiteSpace.ini"));

            Assert.AreEqual(2, ini.Sections.Count);

            Assert.IsTrue(ini.Sections.Contains("section1"));
            Assert.IsTrue(ini.Sections.Contains("section2"));

            Assert.AreEqual("value1", ini.GetValue("section1", "key1"));
            Assert.AreEqual("value2", ini.GetValue("section1", "key2"));
            Assert.AreEqual("value3", ini.GetValue("section2", "key3"));
            Assert.AreEqual("value4", ini.GetValue("section2", "key4"));

        }
        [TestMethod]
        public void TestParseWithUnixLineSeparators() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithUnixLineSeparators.ini"));

            Assert.AreEqual(2, ini.Sections.Count);

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

            Assert.IsTrue(ini.Contains("[section] ; comment"));
            Assert.IsTrue(ini.Contains("; comment"));

            Assert.AreEqual("value ; comment", ini.GetValue("key"));

        }

        [TestMethod]
        public void TestParseWithEscapeSequencesAndEscapeSequencesEnabled() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithEscapedStrings.ini"), new IniOptions() {
                EnableEscapeSequences = true,
            });

            string sectionName = "section[]";

            Assert.AreEqual(4, ini.Sections.Get(sectionName).Properties.Count);

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

            Assert.AreEqual(5, ini.Sections.Get(sectionName)?.Properties.Count ?? 0);

            Assert.AreEqual("=value1", ini.GetValue(sectionName, @"key1\"));
            Assert.AreEqual(@"val\;ue2", ini.GetValue(sectionName, "key2"));
            Assert.AreEqual(@"val\", ini.GetValue(sectionName, "key3"));
            Assert.AreEqual("value4", ini.GetValue(sectionName, @"\;key4"));

            Assert.IsTrue(ini.Contains(sectionName, "ue3"));

        }

        [TestMethod]
        public void TestParseReadsSectionNameWithMultipleSectionNameEndCharacters() {

            IIni ini = IniFactory.Default.Parse("[section]]]]");

            Assert.AreEqual(1, ini.Sections.Count);
            Assert.IsTrue(ini.Sections.Contains("section]]]"));

        }
        [TestMethod]
        public void TestParseReadsSectionNameWithInlineComment() {

            IIni ini = IniFactory.Default.Parse("[section]]]] ; comment", new IniOptions() {
                EnableInlineComments = true,
            });

            Assert.AreEqual(1, ini.Sections.Count);
            Assert.IsTrue(ini.Sections.Contains("section]]]"));

        }
        [TestMethod]
        public void TestParseReadsInvalidSectionNameAsProperty() {

            IIni ini = IniFactory.Default.Parse("[section=value");

            Assert.AreEqual(0, ini.Sections.Count);
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

            Assert.AreEqual(3, ini.Global.Properties.Count);

            Assert.AreEqual(string.Empty, ini.GetValue("key1"));
            Assert.AreEqual(string.Empty, ini.GetValue("key2"));
            Assert.AreEqual(string.Empty, ini.GetValue("key3"));

        }

        [TestMethod]
        public void TestParseWithDuplicateSectionsMergesProperties() {

            IIni ini = IniFactory.Default.Parse(ReadAllText("IniWithDuplicateSections.ini"));

            Assert.AreEqual(1, ini.Sections.Count);
            Assert.AreEqual(2, ini.Sections.First().Properties.Count);

            Assert.AreEqual("value1", ini.Sections.First().GetValue("key1"));
            Assert.AreEqual("value2", ini.Sections.First().GetValue("key2"));

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