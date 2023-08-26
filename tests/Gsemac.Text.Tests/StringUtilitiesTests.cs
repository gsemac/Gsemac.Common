using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void TestBeforeWithStringStartingWithSubstring() {

            Assert.AreEqual(string.Empty, StringUtilities.Before("hello, world!", "hello"));

        }
        [TestMethod]
        public void TestBeforeWithEmptyString() {

            Assert.AreEqual(string.Empty, StringUtilities.Before(string.Empty, ", "));

        }
        [TestMethod]
        public void TestBeforeWithEmptySubstring() {

            Assert.AreEqual("hello, world!", StringUtilities.Before("hello, world!", string.Empty));

        }
        [TestMethod]
        public void TestBeforeWithNullString() {

            Assert.AreEqual(null, StringUtilities.Before(null, ", "));

        }
        [TestMethod]
        public void TestBeforeWithNullSubstring() {

            Assert.AreEqual("hello, world!", StringUtilities.Before("hello, world!", null));

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
        public void TestAfterWithStringEndingWithSubstring() {

            Assert.AreEqual(string.Empty, StringUtilities.After("hello, world!", "world!"));

        }
        [TestMethod]
        public void TestAfterWithEmptyString() {

            Assert.AreEqual(string.Empty, StringUtilities.After(string.Empty, ", "));

        }
        [TestMethod]
        public void TestAfterWithEmptySubstring() {

            Assert.AreEqual("hello, world!", StringUtilities.After("hello, world!", string.Empty));

        }
        [TestMethod]
        public void TestAfterWithNullString() {

            Assert.AreEqual(null, StringUtilities.After(null, ", "));

        }
        [TestMethod]
        public void TestAfterWithNullSubstring() {

            Assert.AreEqual("hello, world!", StringUtilities.After("hello, world!", null));

        }

        // Count

        [TestMethod]
        public void TestCountWithNullString() {

            Assert.AreEqual(0, StringUtilities.Count(null, "substring"));

        }
        [TestMethod]
        public void TestCountWithNullSubstring() {

            Assert.AreEqual(0, StringUtilities.Count("string", null));

        }

        // Reverse

        [TestMethod]
        public void TestReverseWithStringWithAsciiCharacters() {

            Assert.AreEqual("dlrow olleh", StringUtilities.Reverse("hello world"));

        }
        [TestMethod]
        public void TestReverseWithStringWithMultiByteCharacters() {

            Assert.AreEqual("selbarאֳsiM seL", StringUtilities.Reverse("Les Misאֳrables"));

        }
        [TestMethod]
        public void TestReverseWithNullString() {

            Assert.AreEqual(null, StringUtilities.Reverse(null));

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
        public void TestSplitWithNullString() {

            Assert.AreEqual(0, StringUtilities.Split(null, ',').Count());

        }
        [TestMethod]
        public void TestSplitWithRemoveEmptyEntriesOption() {

            string str = "1,2,3,,";
            string[] expected = { "1", "2", "3", };

            IEnumerable<string> actual = StringUtilities.Split(str, ',', new StringSplitOptionsEx() {
                RemoveEmptyEntries = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithTrimEntriesOption() {

            string str = "1,   2,       3,        , ";
            string[] expected = { "1", "2", "3", "", "", };

            IEnumerable<string> actual = StringUtilities.Split(str, ',', new StringSplitOptionsEx() {
                TrimEntries = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithPrependDelimiterOption() {

            string str = "a/b/c";
            string[] expected = { "a", "/b", "/c" };

            IEnumerable<string> actual = StringUtilities.Split(str, '/', new StringSplitOptionsEx() {
                SplitBeforeDelimiter = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithAppendDelimiterOption() {

            string str = "a/b/c";
            string[] expected = { "a/", "b/", "c" };

            IEnumerable<string> actual = StringUtilities.Split(str, '/', new StringSplitOptionsEx() {
                SplitAfterDelimiter = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithAppendDelimiterOptionWithStartingDelimiter() {

            string str = "/a/b/c";
            string[] expected = { "/", "a/", "b/", "c" };

            IEnumerable<string> actual = StringUtilities.Split(str, '/', new StringSplitOptionsEx() {
                SplitAfterDelimiter = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithAppendDelimiterOptionWithEndingDelimiter() {

            string str = "a/b/c/";
            string[] expected = { "a/", "b/", "c/", "" };

            IEnumerable<string> actual = StringUtilities.Split(str, '/', new StringSplitOptionsEx() {
                SplitAfterDelimiter = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithAppendDelimiterAndRemoveEmptyEntriesOptions() {

            string str = "1,2,3,,4,5,,6";
            string[] expected = { "1,", "2,", "3,", "4,", "5,", "6" };

            IEnumerable<string> actual = StringUtilities.Split(str, ',', new StringSplitOptionsEx() {
                RemoveEmptyEntries = true,
                SplitAfterDelimiter = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithRespectEnclosingPunctuationOption() {

            string str = "[1,2,3],(4,5,[6,7]),\"8,9\"";
            string[] expected = { "[1,2,3]", "(4,5,[6,7])", "\"8,9\"" };

            IEnumerable<string> actual = StringUtilities.Split(str, ',', new StringSplitOptionsEx() {
                AllowEnclosedDelimiters = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithRespectEnclosingPunctuationOptionWithoutEnclosingPunctuation() {

            string str = "item 1,item 2,item 3";
            string[] expected = { "item 1", "item 2", "item 3" };

            IEnumerable<string> actual = StringUtilities.Split(str, ',', new StringSplitOptionsEx() {
                AllowEnclosedDelimiters = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithRespectEnclosingPunctuationOptionWithUnmatchedEnclosingPunctuation() {

            string str = "1,(2,3";
            string[] expected = { "1", "(2", "3" };

            IEnumerable<string> actual = StringUtilities.Split(str, ',', new StringSplitOptionsEx() {
                AllowEnclosedDelimiters = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }
        [TestMethod]
        public void TestSplitWithRespectEnclosingPunctuationOptionWithPartiallyUnmatchedEnclosingPunctuation() {

            string str = "1,(2,((3,4)";
            string[] expected = { "1", "(2", "((3,4)" };

            IEnumerable<string> actual = StringUtilities.Split(str, ',', new StringSplitOptionsEx() {
                AllowEnclosedDelimiters = true,
            });

            CollectionAssert.AreEqual(expected, actual.ToArray());

        }

        // ReplaceFirst

        [TestMethod]
        public void TestReplaceFirstWithStringWithSingleMatchingSubstring() {

            Assert.AreEqual("goodbye world", StringUtilities.ReplaceFirst("hello world", "hello", "goodbye"));

        }
        [TestMethod]
        public void TestReplaceFirstWithStringWithMultipleMatchingSubstrings() {

            Assert.AreEqual("goodbye hello world", StringUtilities.ReplaceFirst("hello hello world", "hello", "goodbye"));

        }
        [TestMethod]
        public void TestReplaceFirstWithNullString() {

            Assert.AreEqual(null, StringUtilities.ReplaceFirst(null, "hello", "goodbye"));

        }
        [TestMethod]
        public void TestReplaceFirstWithNullSubstring() {

            Assert.ThrowsException<ArgumentException>(() => StringUtilities.ReplaceFirst("", null, ""));

        }
        [TestMethod]
        public void TestReplaceFirstWithNullReplacement() {

            Assert.AreEqual(" world", StringUtilities.ReplaceFirst("hello world", "hello", null));

        }

        // ReplaceLast

        [TestMethod]
        public void TestReplaceLastWithStringWithSingleMatchingSubstring() {

            Assert.AreEqual("hello everyone", StringUtilities.ReplaceLast("hello world", "world", "everyone"));

        }
        [TestMethod]
        public void TestReplaceLastWithStringWithMultipleMatchingSubstrings() {

            Assert.AreEqual("hello world everyone", StringUtilities.ReplaceLast("hello world world", "world", "everyone"));

        }
        [TestMethod]
        public void TestReplaceLastWithNullString() {

            Assert.AreEqual(null, StringUtilities.ReplaceLast(null, "world", "everyone"));

        }
        [TestMethod]
        public void TestReplaceLastWithNullSubstring() {

            Assert.ThrowsException<ArgumentException>(() => StringUtilities.ReplaceLast("", null, ""));

        }
        [TestMethod]
        public void TestReplaceLastWithNullReplacement() {

            Assert.AreEqual("hello ", StringUtilities.ReplaceLast("hello world", "world", null));

        }

        // NormalizeWhiteSpace

        [TestMethod]
        public void TestNormalizeWhiteSpaceWithStringContainingWhiteSpace() {

            Assert.AreEqual(" hello world ", StringUtilities.NormalizeWhiteSpace("    hello   world     "));

        }
        [TestMethod]
        public void TestNormalizeWhiteSpaceWithNullString() {

            Assert.AreEqual(null, StringUtilities.NormalizeWhiteSpace(null));

        }
        [TestMethod]
        public void TestNormalizeWhiteSpaceWithStringContainingWhiteSpaceAndCustomReplacement() {

            Assert.AreEqual("helloworld", StringUtilities.NormalizeWhiteSpace("    hello   world     ", string.Empty));

        }
        [TestMethod]
        public void TestNormalizeWhiteSpaceWithTrimOption() {

            Assert.AreEqual("hello world", StringUtilities.NormalizeWhiteSpace("    hello   world     ", NormalizeSpaceOptions.Trim));

        }

        // NormalizeLineBreaks

        [TestMethod]
        public void TestNormalizeLineBreaksWithMixedLineBreaks() {

            Assert.AreEqual($"a{Environment.NewLine}b{Environment.NewLine}c",
                StringUtilities.NormalizeLineBreaks("a\r\n\nb\n\r\nc"));

        }
        [TestMethod]
        public void TestNormalizeLineBreaksWithNullString() {

            Assert.AreEqual(null, StringUtilities.NormalizeLineBreaks(null));

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

        // TrimStart

        [TestMethod]
        public void TestTrimStartWithStringBeginningWithSubstring() {

            Assert.AreEqual("worldhellohello", StringUtilities.TrimStart("hellohelloworldhellohello", "hello"));

        }
        [TestMethod]
        public void TestTrimStartWithStringNotBeginningWithSubstring() {

            Assert.AreEqual("worldhello", StringUtilities.TrimStart("worldhello", "hello"));

        }
        [TestMethod]
        public void TestTrimStartWithNullString() {

            Assert.AreEqual(null, StringUtilities.TrimStart(null, "hello"));

        }
        [TestMethod]
        public void TestTrimStartWithNullSubstring() {

            Assert.AreEqual("helloworld", StringUtilities.TrimStart("helloworld", null));

        }

        // TrimEnd

        [TestMethod]
        public void TestTrimEndWithStringBeginningWithSubstring() {

            Assert.AreEqual("hellohelloworld", StringUtilities.TrimEnd("hellohelloworldhellohello", "hello"));

        }
        [TestMethod]
        public void TestTrimEndWithStringNotBeginningWithSubstring() {

            Assert.AreEqual("helloworld", StringUtilities.TrimEnd("helloworld", "hello"));

        }
        [TestMethod]
        public void TestTrimEndWithNullString() {

            Assert.AreEqual(null, StringUtilities.TrimEnd(null, "world"));

        }
        [TestMethod]
        public void TestTrimEndWithNullSubstring() {

            Assert.AreEqual("helloworld", StringUtilities.TrimEnd("helloworld", null));

        }

        // TrimOrDefault

        [TestMethod]
        public void TestTrimOrDefaultWithStringWithSurroundingWhiteSpace() {

            Assert.AreEqual("hello  world", StringUtilities.TrimOrDefault("  hello  world  "));

        }
        [TestMethod]
        public void TestTrimOrDefaultWithEmptyString() {

            Assert.AreEqual("", StringUtilities.TrimOrDefault(""));

        }
        [TestMethod]
        public void TestTrimOrDefaultWithNullString() {

            Assert.AreEqual(null, StringUtilities.TrimOrDefault(null));

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
        [TestMethod]
        public void TestIsNumericWithNullString() {

            Assert.IsFalse(StringUtilities.IsNumeric(null));

        }
        [TestMethod]
        public void TestIsNumericWithNullStringAndNumberStyle() {

            Assert.IsFalse(StringUtilities.IsNumeric(null, NumberStyles.Integer));

        }

        // CanBeEncodedAs

        [TestMethod]
        public void TestCanBeEncodedAsWithAsciiStringAndAsciiEncoding() {

            Assert.IsTrue(StringUtilities.CanBeEncodedAs("hello, world!", Encoding.ASCII));

        }
        [TestMethod]
        public void TestCanBeEncodedAsWithAsciiStringAndUtf8Encoding() {

            Assert.IsTrue(StringUtilities.CanBeEncodedAs("hello, world!", Encoding.UTF8));

        }
        [TestMethod]
        public void TestCanBeEncodedAsWitUtf8StringAndAsciiEncoding() {

            Assert.IsFalse(StringUtilities.CanBeEncodedAs("こんにちは世界！", Encoding.ASCII));

        }
        [TestMethod]
        public void TestCanBeEncodedAsWitUtf8StringAndUtf8Encoding() {

            Assert.IsTrue(StringUtilities.CanBeEncodedAs("こんにちは世界！", Encoding.UTF8));

        }

        // PadDigits

        [TestMethod]
        public void TestPadDigitsWithEmptyString() {

            Assert.AreEqual("000", StringUtilities.PadDigits(string.Empty, 3));

        }
        [TestMethod]
        public void TestPadDigitsWithNullString() {

            Assert.AreEqual("000", StringUtilities.PadDigits(null, 3));

        }
        [TestMethod]
        public void TestPadDigitsWithInteger() {

            Assert.AreEqual("001", StringUtilities.PadDigits("1", 3));

        }
        [TestMethod]
        public void TestPadDigitsWithDecimalPoint() {

            Assert.AreEqual("001.2", StringUtilities.PadDigits("1.2", 3));

        }
        [TestMethod]
        public void TestPadDigitsWithLeadingZeros() {

            Assert.AreEqual("01", StringUtilities.PadDigits("000001", 2));

        }
        [TestMethod]
        public void TestPadDigitsWithPaddingLengthLessThanExistingLength() {

            Assert.AreEqual("123", StringUtilities.PadDigits("123", 2));

        }
        [TestMethod]
        public void TestPadDigitsWithIntegerLiteral() {

            Assert.AreEqual("001", StringUtilities.PadDigits(1, 3));

        }

        // ComputeMD5Hash

        [TestMethod]
        public void TestComputeMD5HashWithEmptyString() {

            // The MD5 hash of "nothing" is known to be "d41d8cd98f00b204e9800998ecf8427e".
            // https://stackoverflow.com/a/10910079/5383169

            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", StringUtilities.ComputeMD5Hash(string.Empty));

        }
        [TestMethod]
        public void TestComputeMD5HashWithNullString() {

            // Null strings should be treated the same as empty stringss.

            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", StringUtilities.ComputeMD5Hash(null));

        }
        [TestMethod]
        public void TestComputeMD5HashWithString() {

            Assert.AreEqual("fc3ff98e8c6a0d3087d515c0473f8677", StringUtilities.ComputeMD5Hash("hello world!"));

        }

    }

}