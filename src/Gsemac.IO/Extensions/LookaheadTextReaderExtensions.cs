using Gsemac.Text.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Extensions {

    public static class LookaheadTextReaderExtensions {

        // Public members

        public static string ReadLine(this LookaheadTextReader reader, string delimiter) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return reader.ReadLine(new[] { delimiter });

        }
        public static string ReadLine(this LookaheadTextReader reader, string[] delimiters) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return reader.ReadLine(delimiters, ReadLineOptions.Default);

        }
        public static string ReadLine(this LookaheadTextReader reader, string[] delimiters, IReadLineOptions options) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (delimiters is null)
                throw new ArgumentNullException(nameof(delimiters));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            StringBuilder resultBuilder = new StringBuilder();

            bool insideEscapeSequence = false;
            bool delimiterFound = false;

            // Create the list of delimiters we'll actually be checking based on the options provided.

            IEnumerable<string> finalDelimiters = delimiters;

            if (options.BreakOnNewLine)
                finalDelimiters = finalDelimiters.Concat(new[] { "\r", "\n", "\r\n", });

            while (!reader.EndOfText()) {

                // Read the next character.

                char nextChar = (char)reader.Peek();
                bool nextCharIsEscaped = insideEscapeSequence && options.IgnoreEscapedDelimiters;
                bool nextCharIsNewLine = nextChar.IsNewLine();

                if (!nextCharIsEscaped) {

                    // If any of the delimiters start with the next character, check if we've reached the full delimiter.

                    foreach (string delimiter in finalDelimiters) {

                        if (delimiter.Length <= 0)
                            continue;

                        if (!delimiter.First().Equals(nextChar))
                            continue;

                        delimiterFound = delimiter.Equals(reader.PeekString(delimiter.Length));

                        if (delimiterFound) {

                            if (options.ConsumeDelimiter) {

                                reader.Skip(delimiter.Length);

                                // If we're breaking on a multi-character newline, consume the second character.

                                if (nextChar.IsNewLine() && ((char)reader.Peek()).IsNewLine())
                                    reader.Read();

                            }

                            break;

                        }

                    }

                }

                if (delimiterFound)
                    break;

                // Consume the next character.

                resultBuilder.Append((char)reader.Read());

                // Allow multi-character newlines to be escaped when the "BreakOnNewLine" option is enabled.

                if (nextChar == '\\' && !insideEscapeSequence)
                    insideEscapeSequence = true;
                else if (!(nextCharIsNewLine && ((char)reader.Peek()).IsNewLine() && options.BreakOnNewLine))
                    insideEscapeSequence = false;

            }

            return resultBuilder.ToString();

        }

        public static string PeekString(this LookaheadTextReader reader, int count) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            char[] buffer = new char[count];

            int charsRead = reader.Peek(buffer, 0, count);

            if (charsRead <= 0)
                return string.Empty;

            return new string(buffer, 0, charsRead);

        }

        public static bool IsNext(this LookaheadTextReader reader, string value) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            return reader.PeekString(value.Length).Equals(value);

        }

    }

}