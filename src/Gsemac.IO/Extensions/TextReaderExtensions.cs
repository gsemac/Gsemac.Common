using Gsemac.IO.Properties;
using Gsemac.Text.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Extensions {

    public static class TextReaderExtensions {

        // Public members

        public static bool Skip(this TextReader reader) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return reader.Read() != -1;

        }
        public static bool Skip(this TextReader reader, int count) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            for (int i = 0; i < count; ++i)
                if (reader.Read() == -1)
                    break;

            return reader.Peek() != -1;

        }
        public static void SkipWhiteSpace(this TextReader reader) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            while (true) {

                int nextChar = reader.Peek();

                if (nextChar == -1 || !char.IsWhiteSpace((char)nextChar))
                    break;

                reader.Read();

            }

        }

        public static string ReadLine(this TextReader reader, IReadLineOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return reader.ReadLine(new char[0], options);

        }

        public static string ReadLine(this TextReader reader, char delimiter) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return reader.ReadLine(delimiter, ReadLineOptions.Default);

        }
        public static string ReadLine(this TextReader reader, char delimiter, IReadLineOptions options) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return reader.ReadLine(new[] { delimiter }, options);

        }
        public static string ReadLine(this TextReader reader, char[] delimiters) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return reader.ReadLine(delimiters, ReadLineOptions.Default);

        }
        public static string ReadLine(this TextReader reader, char[] delimiters, IReadLineOptions options) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (delimiters is null)
                throw new ArgumentNullException(nameof(delimiters));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            StringBuilder resultBuilder = new StringBuilder();

            bool insideEscapeSequence = false;

            while (!reader.EndOfText()) {

                // Check if we've hit a delimiter.

                char nextChar = (char)reader.Peek();
                bool nextCharIsEscaped = insideEscapeSequence && options.AllowEscapedDelimiters;
                bool nextCharIsNewLine = nextChar.IsNewLine();

                if (!nextCharIsEscaped && (delimiters.Any(c => c == nextChar) || nextCharIsNewLine && options.BreakOnNewLine)) {

                    if (options.ConsumeDelimiter) {

                        // Consume the delimiter.

                        reader.Read();

                        // If we're breaking on a multi-character newline, consume the second character.

                        if (nextCharIsNewLine && options.BreakOnNewLine && ((char)reader.Peek()).IsNewLine())
                            reader.Read();

                    }

                    break;

                }

                // Consume the next character.

                reader.Read();

                resultBuilder.Append(nextChar);

                // Allow multi-character newlines to be escaped when the "BreakOnNewLine" option is enabled.

                if (nextChar == options.EscapeCharacter && !insideEscapeSequence)
                    insideEscapeSequence = true;
                else if (!(nextCharIsNewLine && ((char)reader.Peek()).IsNewLine() && options.BreakOnNewLine))
                    insideEscapeSequence = false;

            }

            return resultBuilder.ToString();

        }

        public static string ReadLine(this TextReader reader, string delimiter) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return reader.ReadLine(delimiter, ReadLineOptions.Default);

        }
        public static string ReadLine(this TextReader reader, string delimiter, IReadLineOptions options) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return reader.ReadLine(new[] { delimiter }, options);

        }
        public static string ReadLine(this TextReader reader, string[] delimiters) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return reader.ReadLine(delimiters, ReadLineOptions.Default);

        }
        public static string ReadLine(this TextReader reader, string[] delimiters, IReadLineOptions options) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (delimiters is null)
                throw new ArgumentNullException(nameof(delimiters));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (!options.ConsumeDelimiter)
                throw new ArgumentException(ExceptionMessages.ReadLineWithStringDelimiterMustConsumeDelimiter, nameof(options));

            StringBuilder resultBuilder = new StringBuilder();

            bool insideEscapeSequence = false;
            bool delimiterFound = false;

            // Create the list of delimiters we'll actually be checking based on the options provided.

            IEnumerable<string> finalDelimiters = delimiters;

            if (options.BreakOnNewLine)
                finalDelimiters = finalDelimiters.Concat(new[] { "\r", "\n", "\r\n", });

            while (!reader.EndOfText()) {

                // Consume the next character.

                char nextChar = (char)reader.Read();
                bool nextCharIsEscaped = insideEscapeSequence && options.AllowEscapedDelimiters;
                bool nextCharIsNewLine = nextChar.IsNewLine();

                resultBuilder.Append(nextChar);

                if (!nextCharIsEscaped) {

                    // If we encounter the last character of any of our delimiters, check if we've read the full delimiter.

                    foreach (string delimiter in finalDelimiters) {

                        if (delimiter.Length <= 0)
                            continue;

                        if (!delimiter.Last().Equals(nextChar))
                            continue;

                        delimiterFound = resultBuilder.ToString(resultBuilder.Length - delimiter.Length, delimiter.Length).Equals(delimiter);

                        if (delimiterFound) {

                            // Remove the delimiter from the result.

                            resultBuilder.Remove(resultBuilder.Length - delimiter.Length, delimiter.Length);

                            // If we're breaking on a multi-character newline, consume the second character.

                            if (nextChar.IsNewLine() && ((char)reader.Peek()).IsNewLine())
                                reader.Read();

                            break;

                        }

                    }

                }

                if (delimiterFound)
                    break;

                // Allow multi-character newlines to be escaped when the "BreakOnNewLine" option is enabled.

                if (nextChar == options.EscapeCharacter && !insideEscapeSequence)
                    insideEscapeSequence = true;
                else if (!(nextCharIsNewLine && ((char)reader.Peek()).IsNewLine() && options.BreakOnNewLine))
                    insideEscapeSequence = false;

            }

            return resultBuilder.ToString();

        }

        public static string ReadString(this TextReader reader, int count) {

            char[] buffer = new char[count];

            int charsRead = reader.Read(buffer, 0, count);

            if (charsRead <= 0)
                return string.Empty;

            return new string(buffer, 0, charsRead);

        }

        public static bool TryPeek(this TextReader reader, out char value) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            value = default;

            int nextChar = reader.Peek();

            if (nextChar >= 0)
                value = (char)nextChar;

            return nextChar >= 0;

        }
        public static bool TryRead(this TextReader reader, out char value) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            value = default;

            int nextChar = reader.Read();

            if (nextChar >= 0)
                value = (char)nextChar;

            return nextChar >= 0;

        }

        public static bool IsNext(this TextReader reader, char value) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            int nextChar = reader.Peek();

            if (nextChar < 0)
                return false;

            return (char)nextChar == value;

        }

        public static bool EndOfText(this TextReader reader) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return !TryPeek(reader, out _);

        }

    }

}