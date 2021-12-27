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

        // Split

        [TestMethod]
        public void TestSplit() {

            string str = "1,2,3,,";
            string[] expected = { "1", "2", "3", "", "", };
            IEnumerable<string> actual = StringUtilities.Split(str, ',');

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitOnWhiteSpace() {

            string str = "1 2  3\t4";
            string[] expected = { "1", "2", "", "3", "4", };
            IEnumerable<string> actual = StringUtilities.Split(str, (string[])null);

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithRemoveEmptyEntriesOption() {

            string str = "1,2,3,,";
            string[] expected = { "1", "2", "3", };
            IEnumerable<string> actual = StringUtilities.Split(str, ',', StringSplitOptions.RemoveEmptyEntries);

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithTrimEntriesOption() {

            string str = "1,   2,       3,        , ";
            string[] expected = { "1", "2", "3", "", "", };
            IEnumerable<string> actual = StringUtilities.Split(str, ',', StringSplitOptions.TrimEntries);

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithPrependDelimiterOption() {

            string str = "a/b/c";
            string[] expected = { "a", "/b", "/c" };
            IEnumerable<string> actual = StringUtilities.Split(str, '/', StringSplitOptions.PrependDelimiter);

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithAppendDelimiterOption() {

            string str = "a/b/c";
            string[] expected = { "a/", "b/", "c" };
            IEnumerable<string> actual = StringUtilities.Split(str, '/', StringSplitOptions.AppendDelimiter);

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithAppendDelimiterOptionWithStartingDelimiter() {

            string str = "/a/b/c";
            string[] expected = { "/", "a/", "b/", "c" };
            IEnumerable<string> actual = StringUtilities.Split(str, '/', StringSplitOptions.AppendDelimiter);

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithAppendDelimiterOptionWithEndingDelimiter() {

            string str = "a/b/c/";
            string[] expected = { "a/", "b/", "c/", "" };
            IEnumerable<string> actual = StringUtilities.Split(str, '/', StringSplitOptions.AppendDelimiter);

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithAppendDelimiterAndRemoveEmptyEntriesOptions() {

            string str = "1,2,3,,4,5,,6";
            string[] expected = { "1,", "2,", "3,", "4,", "5,", "6" };
            IEnumerable<string> actual = StringUtilities.Split(str, ',', StringSplitOptions.AppendDelimiter | StringSplitOptions.RemoveEmptyEntries);

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithRespectEnclosingPunctuationOption() {

            string str = "[1,2,3],(4,5,[6,7]),\"8,9\"";
            string[] expected = { "[1,2,3]", "(4,5,[6,7])", "\"8,9\"" };
            IEnumerable<string> actual = StringUtilities.Split(str, ',', StringSplitOptions.RespectEnclosingPunctuation);

            CollectionAssert.AreEqual(expected, actual.ToArray());

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
        public void TestIsNumericWithPositiveNumber() {

            Assert.IsTrue(StringUtilities.IsNumeric("1"));

        }
        [TestMethod]
        public void TestIsNumericWithNumberWithNegativeSign() {

            Assert.IsTrue(StringUtilities.IsNumeric("-1"));

        }
        [TestMethod]
        public void TestIsNumericWithNumberWithPositiveSign() {

            // By default, only the negative sign is allowed.
            // The reason is that users do not normally express positive numbers in this way.

            Assert.IsFalse(StringUtilities.IsNumeric("+1"));

        }
        [TestMethod]
        public void TestIsNumericWithNumberWithMultipleSigns() {

            Assert.IsFalse(StringUtilities.IsNumeric("--1"));

        }
        [TestMethod]
        public void TestIsNumericWithNumberWithSurroundingWhiteSpace() {

            Assert.IsTrue(StringUtilities.IsNumeric("    -1     "));

        }
        [TestMethod]
        public void TestIsNumericWithNumberWithDecimalPoint() {

            Assert.IsTrue(StringUtilities.IsNumeric("1.0"));

        }
        [TestMethod]
        public void TestIsNumericWithNumberWithMultipleDecimalPoints() {

            Assert.IsFalse(StringUtilities.IsNumeric("1.0.0"));

        }
        [TestMethod]
        public void TestIsNumericWithEmptyString() {

            Assert.IsFalse(StringUtilities.IsNumeric(string.Empty));

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