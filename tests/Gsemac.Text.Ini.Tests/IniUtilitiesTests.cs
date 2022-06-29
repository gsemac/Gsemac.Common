using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsemac.Text.Ini.Tests {

    [TestClass]
    public class IniUtilitiesTests {

        // Escape

        [TestMethod]
        public void TestEscapeEscapesReservedCharacters() {

            string input = @"C:\path\[name=]";
            string escaped = IniUtilities.Escape(input);

            Assert.AreEqual(escaped, @"C:\\path\\\[name\=\]");

        }
        [TestMethod]
        public void TestEscapeEscapesCustomDelimiters() {

            string input = "key:value # comment";

            string escaped = IniUtilities.Escape(input, new IniOptions() {
                CommentMarker = "#",
                NameValueSeparator = ":",
            });

            Assert.AreEqual(escaped, @"key\:value \# comment");

        }

        // Unescape

        [TestMethod]
        public void TestUnescapeReturnsOriginalValue() {

            string input = @"C:\path\[name=]";
            string escaped = IniUtilities.Escape(input);

            Assert.AreEqual(input, IniUtilities.Unescape(escaped));

        }
        [TestMethod]
        public void TestUnescapeUnescapesCustomDelimiters() {

            string input = "key:value # comment";

            string escaped = IniUtilities.Escape(input, new IniOptions() {
                CommentMarker = "#",
                NameValueSeparator = ":",
            });

            Assert.AreEqual(input, IniUtilities.Unescape(escaped));

        }

    }

}