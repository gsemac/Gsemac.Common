using Gsemac.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gsemac.IO.Extensions.Tests {

    [TestClass]
    public class LookaheadStreamReaderExtensionsTests {

        // Public members

        // ReadLine

        [TestMethod]
        public void TestReadLineWithSingleDelimiter() {

            using var reader = CreateReader("hello world");

            Assert.AreEqual("hello wo", reader.ReadLine("r"));

        }
        [TestMethod]
        public void TestReadLineWithMultipleDelimiters() {

            using var reader = CreateReader("hello world");

            Assert.AreEqual("hello w", reader.ReadLine(new[] {
                "oa",
                "orl",
                "ld",
                "abc",
            }));

        }
        [TestMethod]
        public void TestReadLineWithDelimiterConsumesDelimiter() {

            using var reader = CreateReader("hello world");

            reader.ReadLine(new[] {
                "orl",
            });

            Assert.AreEqual("d", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithDelimiterDoesNotConsumeDelimiterWithConsumeDelimiterOptionDisabled() {

            using var reader = CreateReader("hello world");

            reader.ReadLine(new[] {
                'r'
            }, new ReadLineOptions() {
                ConsumeDelimiter = false,
            });

            Assert.AreEqual("rld", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithDelimiterWithEscapedDelimiter() {

            using var reader = CreateReader(@"hell\o world");

            string value = reader.ReadLine(new[] {
                "o",
            }, new ReadLineOptions() {
                IgnoreEscapedDelimiters = true,
            });

            Assert.AreEqual(@"hell\o w", value);

        }

        [TestMethod]
        public void TestReadLineWithNewLineDelimiter() {

            using var reader = CreateReader("hello\r\nworld");

            Assert.AreEqual("hello\r", reader.ReadLine("\n", new ReadLineOptions() {
                BreakOnNewLine = false,
            }));

        }
        [TestMethod]
        public void TestReadLineWithTextContainingNewLineAndBreakOnNewLineOptionEnabled() {

            using var reader = CreateReader("hello\nworld");

            IReadLineOptions readLineOptions = new ReadLineOptions() {
                BreakOnNewLine = true,
            };

            Assert.AreEqual("hello", reader.ReadLine(Array.Empty<string>(), readLineOptions));
            Assert.AreEqual("world", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithTextContainingMultiCharacterNewLineAndBreakOnNewLineOptionEnabled() {

            using var reader = CreateReader("hello\r\nworld");

            IReadLineOptions readLineOptions = new ReadLineOptions() {
                BreakOnNewLine = true,
            };

            Assert.AreEqual("hello", reader.ReadLine(Array.Empty<string>(), readLineOptions));
            Assert.AreEqual("world", reader.ReadLine());

        }
        [TestMethod]
        public void TestReadLineWithTextContainingEscapedMultiCharacterNewLine() {

            // A single backslash should escape single and multi-character newline representations.

            using var reader = CreateReader("a\\\r\nb\r\nc");

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

        private static LookaheadStreamReader CreateReader(string str) {

            return new LookaheadStreamReader(StringUtilities.StringToStream(str));

        }

    }

}