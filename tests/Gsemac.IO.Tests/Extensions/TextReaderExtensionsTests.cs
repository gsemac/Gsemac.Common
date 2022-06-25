using Gsemac.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Gsemac.IO.Extensions.Tests {

    [TestClass]
    public class TextReaderExtensionsTests {

        // Public members

        // Skip

        [TestMethod]
        public void TestSkipSingleCharacter() {

            using TextReader reader = CreateReader("hello world");

            reader.Skip();

            Assert.AreEqual("ello world", reader.ReadLine());

        }
        [TestMethod]
        public void TestSkipMultipleCharacters() {

            using TextReader reader = CreateReader("hello world");

            reader.Skip(5);

            Assert.AreEqual(" world", reader.ReadLine());

        }

        // ReadLine

        [TestMethod]
        public void TestReadLineWithSingleCharDelimiter() {

            using TextReader reader = CreateReader("hello world");

            Assert.AreEqual("hello wo", reader.ReadLine('r'));

        }
        [TestMethod]
        public void TestReadLineWithMultipleCharDelimiters() {

            using TextReader reader = CreateReader("hello world");

            Assert.AreEqual("hello worl", reader.ReadLine(new[] {
                'a',
                'b',
                'c',
                'd',
            }));

        }
        [TestMethod]
        public void TestReadLineWithCharDelimiterConsumesDelimiter() {

            using TextReader reader = CreateReader("hello world");

            reader.ReadLine(new[] {
                'r'
            });

            Assert.AreEqual("ld", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithCharDelimiterDoesNotConsumeDelimiterWithConsumeDelimiterOptionDisabled() {

            using TextReader reader = CreateReader("hello world");

            reader.ReadLine(new[] {
                'r'
            }, new ReadLineOptions() {
                ConsumeDelimiter = false,
            });

            Assert.AreEqual("rld", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithCharDelimiterWithEscapedDelimiter() {

            using TextReader reader = CreateReader(@"hell\o world");

            string value = reader.ReadLine(new[] {
                'o'
            }, new ReadLineOptions() {
                IgnoreEscapedDelimiters = true,
            });

            Assert.AreEqual(@"hell\o w", value);

        }

        [TestMethod]
        public void TestReadLineWithNewLineCharDelimiter() {

            // Previously, multi-character newlines (i.e. "\r\n") were given special treatment whereby "\n" immediately following an "\r" would be consumed.
            // This was so a single backslash could escape a multi-character newline, allowing for line continuations.
            // However, this functionality has been removed because it interfered with situations like this one.
            // The "BreakOnNewLine" option can be used to enable similar functionality.

            using TextReader reader = CreateReader("hello\r\nworld");

            Assert.AreEqual("hello\r", reader.ReadLine('\n', new ReadLineOptions() {
                BreakOnNewLine = false,
            }));

        }
        [TestMethod]
        public void TestReadLineWithCharDelimiterAndTextContainingNewLineAndBreakOnNewLineOptionEnabled() {

            using TextReader reader = CreateReader("hello\nworld");

            Assert.AreEqual("hello", reader.ReadLine(Array.Empty<char>(), new ReadLineOptions() {
                BreakOnNewLine = true,
            }));

            Assert.AreEqual("world", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithCharDelimiterAndTextContainingMultiCharacterNewLineAndBreakOnNewLineOptionEnabled() {

            using TextReader reader = CreateReader("hello\r\nworld");

            Assert.AreEqual("hello", reader.ReadLine(Array.Empty<char>(), new ReadLineOptions() {
                BreakOnNewLine = true,
            }));

            Assert.AreEqual("world", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithCharDelimiterAndTextContainingEscapedMultiCharacterNewLine() {

            // A single backslash should escape single and multi-character newline representations.

            using TextReader reader = CreateReader("a\\\r\nb\r\nc");

            Assert.AreEqual("a\\\r\nb", reader.ReadLine(Array.Empty<char>(), new ReadLineOptions() {
                IgnoreEscapedDelimiters = true,
                BreakOnNewLine = true,
            }));

        }

        [TestMethod]
        public void TestReadLineWithSingleStringDelimiter() {

            using TextReader reader = CreateReader("hello world");

            Assert.AreEqual("hello w", reader.ReadLine("or"));

        }
        [TestMethod]
        public void TestReadLineWithMultipleStringDelimiters() {

            using TextReader reader = CreateReader("hello world");

            Assert.AreEqual("hello w", reader.ReadLine(new[] {
                "oa",
                "orl",
                "ld",
                "abc",
            }));

        }
        [TestMethod]
        public void TestReadLineWithStringDelimiterConsumesDelimiter() {

            using TextReader reader = CreateReader("hello world");

            reader.ReadLine(new[] {
                "orl",
            });

            Assert.AreEqual("d", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithStringDelimiterWithConsumeDelimiterOptionDisabledThrows() {

            using TextReader reader = CreateReader("hello world");

            Assert.ThrowsException<ArgumentException>(() => reader.ReadLine(new[] {
                "orl",
            }, new ReadLineOptions() {
                ConsumeDelimiter = false,
            }));

        }
        [TestMethod]
        public void TestReadLineWithStringDelimiterWithEscapedDelimiter() {

            using TextReader reader = CreateReader(@"hell\o world");

            string value = reader.ReadLine(new[] {
                "o",
            }, new ReadLineOptions() {
                IgnoreEscapedDelimiters = true,
            });

            Assert.AreEqual(@"hell\o w", value);

        }

        [TestMethod]
        public void TestReadLineWithNewLineStringDelimiter() {

            using TextReader reader = CreateReader("hello\r\nworld");

            Assert.AreEqual("hello\r", reader.ReadLine("\n", new ReadLineOptions() {
                BreakOnNewLine = false,
            }));

        }
        [TestMethod]
        public void TestReadLineWithStringDelimiterAndTextContainingNewLineAndBreakOnNewLineOptionEnabled() {

            using TextReader reader = CreateReader("hello\nworld");

            IReadLineOptions readLineOptions = new ReadLineOptions() {
                BreakOnNewLine = true,
            };

            Assert.AreEqual("hello", reader.ReadLine(Array.Empty<string>(), readLineOptions));
            Assert.AreEqual("world", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithStringDelimiterAndTextContainingMultiCharacterNewLineAndBreakOnNewLineOptionEnabled() {

            using TextReader reader = CreateReader("hello\r\nworld");

            IReadLineOptions readLineOptions = new ReadLineOptions() {
                BreakOnNewLine = true,
            };

            Assert.AreEqual("hello", reader.ReadLine(Array.Empty<string>(), readLineOptions));
            Assert.AreEqual("world", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithStringDelimiterAndTextContainingEscapedMultiCharacterNewLine() {

            // A single backslash should escape single and multi-character newline representations.

            using TextReader reader = CreateReader("a\\\r\nb\r\nc");

            IReadLineOptions readLineOptions = new ReadLineOptions() {
                IgnoreEscapedDelimiters = true,
                BreakOnNewLine = true,
            };

            Assert.AreEqual("a\\\r\nb", reader.ReadLine(Array.Empty<string>(), new ReadLineOptions() {
                IgnoreEscapedDelimiters = true,
                BreakOnNewLine = true,
            }));

        }

        // Private members

        private static TextReader CreateReader(string str) {

            return new StreamReader(StringUtilities.StringToStream(str), leaveOpen: false);

        }

    }

}