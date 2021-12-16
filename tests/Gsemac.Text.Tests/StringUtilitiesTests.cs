using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Text.Tests {

    [TestClass]
    public class StringUtilitiesTests {

        // Before

        [TestMethod]
        public void TestBeforeWithStringContainingSubstring() {

            Assert.AreEqual("hello", StringUtilities.Before("hello, world!", ", "));

        }
        [TestMethod]
        public void TestBeforeWithStringNotContainingSubstring() {

            Assert.AreEqual("hello, world!", StringUtilities.Before("hello, world!", "x"));

        }
        [TestMethod]
        public void TestBeforeWithNullString() {

            Assert.AreEqual(string.Empty, StringUtilities.Before(string.Empty, ", "));

        }
        [TestMethod]
        public void TestBeforeWithNullSubstring() {

            Assert.AreEqual("hello, world!", StringUtilities.Before("hello, world!", string.Empty));

        }

        // After

        [TestMethod]
        public void TestAfterWithStringContainingSubstring() {

            Assert.AreEqual("world!", StringUtilities.After("hello, world!", ", "));

        }
        [TestMethod]
        public void TestAfterWithStringNotContainingSubstring() {

            Assert.AreEqual("hello, world!", StringUtilities.After("hello, world!", "x"));

        }
        [TestMethod]
        public void TestAfterWithNullString() {

            Assert.AreEqual(string.Empty, StringUtilities.After(string.Empty, ", "));

        }
        [TestMethod]
        public void TestAfterWithNullSubstring() {

            Assert.AreEqual("hello, world!", StringUtilities.After("hello, world!", string.Empty));

        }

        // SplitAfter

        [TestMethod]
        public void TestSplitAfter() {

            string input = "a/b/c";
            string[] expectedResult = { "a/", "b/", "c" };
            IEnumerable<string> result = StringUtilities.SplitAfter(input, '/');

            Assert.IsTrue(result.SequenceEqual(expectedResult));

        }
        [TestMethod]
        public void TestSplitAfterWithStartingDelimiter() {

            string input = "/a/b/c";
            string[] expectedResult = { "/", "a/", "b/", "c" };
            IEnumerable<string> result = StringUtilities.SplitAfter(input, '/');

            Assert.IsTrue(result.SequenceEqual(expectedResult));

        }
        [TestMethod]
        public void TestSplitAfterWithEndingDelimiter() {

            string input = "a/b/c/";
            string[] expectedResult = { "a/", "b/", "c/", "" };
            IEnumerable<string> result = StringUtilities.SplitAfter(input, '/');

            Assert.IsTrue(result.SequenceEqual(expectedResult));

        }

        // NormalizeLineBreaks

        [TestMethod]
        public void TestNormalizeLineBreaksWithMixedLineBreaks() {

            Assert.AreEqual($"a{Environment.NewLine}b{Environment.NewLine}c",
                StringUtilities.NormalizeLineBreaks("a\r\n\nb\n\r\nc"));

        }
        [TestMethod]
        public void TestNormalizeLineBreaksWithPreserveLineBreaksOption() {

            Assert.AreEqual($"a{Environment.NewLine}{Environment.NewLine}b{Environment.NewLine}{Environment.NewLine}c",
                StringUtilities.NormalizeLineBreaks("a\r\n\nb\n\r\nc", NormalizeSpaceOptions.PreserveLineBreaks));

        }
        [TestMethod]
        public void TestNormalizeLineBreaksWithPreserveParagraphBreaksOption() {

            Assert.AreEqual($"a{Environment.NewLine}{Environment.NewLine}b{Environment.NewLine}{Environment.NewLine}c",
                StringUtilities.NormalizeLineBreaks("a\r\n\nb\n\r\n\n\nc", NormalizeSpaceOptions.PreserveParagraphBreaks));

        }

        // Unescape

        [TestMethod]
        public void TestUnescapeWithDataString() {

            Assert.AreEqual("https://meyerweb.com/eric/tools/dencoder/", StringUtilities.Unescape("https%3A%2F%2Fmeyerweb.com%2Feric%2Ftools%2Fdencoder%2F"));

        }
        [TestMethod]
        public void TestUnescapeWithInvalidDataString() {

            Assert.AreEqual("%g%h1test", StringUtilities.Unescape("%g%h1test"));

        }
        [TestMethod]
        public void TestUnescapeWithHtmlEntities() {

            Assert.AreEqual("'・\"ñ'", StringUtilities.Unescape("&#039;&#12539;&QUOT;&ntilde;&#x27;"));

        }
        [TestMethod]
        public void TestUnescapeWithInvalidHtmlEntities() {

            Assert.AreEqual("&invalid;", StringUtilities.Unescape("&invalid;"));

        }
        [TestMethod]
        public void TestUnescapeWithBrokenEncoding() {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Assert.AreEqual("’–☆", StringUtilities.Unescape("â€™â€“â˜†"));

        }
        [TestMethod]
        public void TestUnescapeWithEscapedQuotes() {

            Assert.AreEqual(@"\""\'", StringUtilities.Unescape(@"\\""\\'"));

        }
        [TestMethod]
        public void TestUnescapeWithEmptyString() {

            Assert.AreEqual(string.Empty, StringUtilities.Unescape(string.Empty));

        }
        [TestMethod]
        public void TestUnescapeWithUnicodeEscapeSequence() {

            Assert.AreEqual(@"⌠", StringUtilities.Unescape(@"\u2320"));

        }
        [TestMethod]
        public void TestUnescapeWithUnicodeDataString() {

            Assert.AreEqual(@"⌠", StringUtilities.Unescape(@"%u2320"));

        }
        [TestMethod]
        public void TestUnescapeWithEscapedEscapeSequences() {

            Assert.AreEqual(@"\n", StringUtilities.Unescape(@"%5C%6e"));

        }

        // IsNumeric

        [TestMethod]
        public void TestIsNumericWithPositiveInteger() {

            Assert.IsTrue(StringUtilities.IsNumeric("1"));

        }
        [TestMethod]
        public void TestIsNumericWithNegativeInteger() {

            // By default, negative integers should not be counted as numeric ("-" is not a digit).
            // This can be controlled by providing a different set of NumberStyles.

            Assert.IsFalse(StringUtilities.IsNumeric("-1"));

        }
        [TestMethod]
        public void TestIsNumericWithSingleDecimalPoint() {

            Assert.IsTrue(StringUtilities.IsNumeric("1.0"));

        }
        [TestMethod]
        public void TestIsNumericWithMultipleDecimalPoints() {

            Assert.IsFalse(StringUtilities.IsNumeric("1.0.0"));

        }

        // PadLeadingDigits

        [TestMethod]
        public void TestPadLeadingDigitsWithEmptyString() {

            Assert.AreEqual("000", StringUtilities.PadLeadingDigits(string.Empty, 3));

        }
        [TestMethod]
        public void TestPadLeadingDigitsWithInteger() {

            Assert.AreEqual("001", StringUtilities.PadLeadingDigits("1", 3));

        }
        [TestMethod]
        public void TestPadLeadingDigitsWithDecimalPoint() {

            Assert.AreEqual("001.2", StringUtilities.PadLeadingDigits("1.2", 3));

        }
        [TestMethod]
        public void TestPadLeadingDigitsWithLeadingZeros() {

            Assert.AreEqual("01", StringUtilities.PadLeadingDigits("000001", 2));

        }
        [TestMethod]
        public void TestPadLeadingDigitsWithPaddingLengthLessThanExistingLength() {

            Assert.AreEqual("123", StringUtilities.PadLeadingDigits("123", 2));

        }

    }

}