using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Text.Extensions {

    public static class TextReaderExtensions {

        // Public members

        public static bool Skip(this TextReader textReader) {

            if (textReader is null)
                throw new ArgumentNullException(nameof(textReader));

            return textReader.Read() != -1;

        }
        public static void SkipWhiteSpace(this TextReader textReader) {

            if (textReader is null)
                throw new ArgumentNullException(nameof(textReader));

            while (true) {

                int nextChar = textReader.Peek();

                if (nextChar == -1 || !char.IsWhiteSpace((char)nextChar))
                    break;

                textReader.Read();

            }

        }

        public static string ReadLine(this TextReader textReader, char delimiter) {

            if (textReader is null)
                throw new ArgumentNullException(nameof(textReader));

            return ReadLine(textReader, new[] { delimiter });

        }
        public static string ReadLine(this TextReader textReader, params char[] delimiters) {

            if (textReader is null)
                throw new ArgumentNullException(nameof(textReader));

            return ReadLine(textReader, delimiters, allowEscapeSequences: false);

        }
        public static string ReadLine(this TextReader textReader, char[] delimiters, bool allowEscapeSequences) {

            if (textReader is null)
                throw new ArgumentNullException(nameof(textReader));

            StringBuilder valueBuilder = new StringBuilder();
            bool insideEscapeSequence = false;

            while (textReader.Peek() != -1 && ((insideEscapeSequence && allowEscapeSequences) || !delimiters.Any(c => c == (char)textReader.Peek()))) {

                char nextChar = (char)textReader.Read();

                valueBuilder.Append(nextChar);

                // Treat "\r\n" as a single character than can be escaped.

                if (nextChar == '\r' && (char)textReader.Peek() == '\n')
                    valueBuilder.Append((char)textReader.Read());

                if (nextChar == '\\' && !insideEscapeSequence)
                    insideEscapeSequence = true;
                else
                    insideEscapeSequence = false;

            }

            return valueBuilder.ToString();

        }

        public static bool TryPeek(this TextReader textReader, out char value) {

            if (textReader is null)
                throw new ArgumentNullException(nameof(textReader));

            value = default;

            int nextChar = textReader.Peek();

            if (nextChar >= 0)
                value = (char)nextChar;

            return nextChar >= 0;

        }
        public static bool TryRead(this TextReader textReader, out char value) {

            if (textReader is null)
                throw new ArgumentNullException(nameof(textReader));

            value = default;

            int nextChar = textReader.Read();

            if (nextChar >= 0)
                value = (char)nextChar;

            return nextChar >= 0;

        }

    }

}