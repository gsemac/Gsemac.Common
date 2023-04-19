using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Text.Lexers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Gsemac.Net.Http.Lexers {

    internal class CookieLexer :
        LexerBase<CookieLexerToken> {

        // Public members

        public CookieLexer(Stream stream) :
            base(stream) {
        }
        public CookieLexer(TextReader reader) :
            base(reader) {
        }

        // Protected members

        protected override void ReadNext(Queue<CookieLexerToken> tokens) {

            // Designed based on the specification given here:
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Set-Cookie

            // The parsing is designed to give similar results to "CookieContainer.SetCookies", so values are not unescaped and outer quotes are not stripped.
            // This provides a means of parsing cookies without using the "SetCookies" method, which requires the cookie domain match the given URI.

            if (ReadCookie(tokens))
                ReadSeparator(tokens, CookieSeparator, CookieLexerTokenType.CookieSeparator);

        }

        // Private members

        private const char AttributeSeparator = ';';
        private const char AttributeValueSeparator = '=';
        private const char CookieSeparator = ',';
        private const char ValueSeparator = '=';

        private bool ReadCookie(Queue<CookieLexerToken> tokens) {

            if (Reader.EndOfText())
                return false;

            // Read the "name=value" part of the cookie.
            // Cookies may be of the form "name" or "name=value".

            bool readCookie = ReadName(tokens, CookieLexerTokenType.Name);

            if (readCookie) {

                if (ReadSeparator(tokens, ValueSeparator, CookieLexerTokenType.NameValueSeparator))
                    ReadValue(tokens);

                // If there are attributes, read the attributes.
                // Attributes may be of the form "name" or "name=value".

                while (ReadSeparator(tokens, AttributeSeparator, CookieLexerTokenType.AttributeSeparator)) {

                    if (ReadName(tokens, CookieLexerTokenType.AttributeName)) {

                        bool isExpiresAttribute = tokens.LastOrDefault()?.
                            Value.Equals("expires", System.StringComparison.OrdinalIgnoreCase) ?? false;

                        if (ReadSeparator(tokens, AttributeValueSeparator, CookieLexerTokenType.AttributeValueSeparator))
                            ReadAttributeValue(tokens, isExpiresAttribute);

                    }

                }

            }

            return readCookie;

        }
        private bool ReadName(Queue<CookieLexerToken> tokens, CookieLexerTokenType tokenType) {

            Reader.SkipWhiteSpace();

            if (Reader.EndOfText())
                return false;

            string value = Reader.ReadLine(new[] {
                AttributeSeparator,
                CookieSeparator,
                ValueSeparator
            }, new ReadLineOptions() {
                ConsumeDelimiter = false,
            }).Trim();

            tokens.Enqueue(new CookieLexerToken(tokenType, value));

            return true;

        }
        private bool ReadSeparator(Queue<CookieLexerToken> tokens, char separator, CookieLexerTokenType tokenType) {

            Reader.SkipWhiteSpace();

            if (!Reader.IsNext(separator))
                return false;

            string value = ((char)Reader.Read()).ToString(CultureInfo.InvariantCulture);

            tokens.Enqueue(new CookieLexerToken(tokenType, value));

            return true;

        }
        private bool ReadValue(Queue<CookieLexerToken> tokens) {

            // Values are allowed to be empty.

            Reader.SkipWhiteSpace();

            string value = Reader.ReadLine(new[] {
                AttributeSeparator,
                CookieSeparator,
            }, new ReadLineOptions() {
                ConsumeDelimiter = false,
            }).Trim();

            tokens.Enqueue(new CookieLexerToken(CookieLexerTokenType.Value, value));

            return true;

        }
        private bool ReadAttributeValue(Queue<CookieLexerToken> tokens, bool isExpiresAttribute) {

            // Values are allowed to be empty.
            // Only the "expires" attribute is allowed to contain commas.

            Reader.SkipWhiteSpace();

            char[] separators = isExpiresAttribute ?
                new[] {
                    AttributeSeparator,
                } :
                new[] {
                    AttributeSeparator,
                    CookieSeparator,
                };

            string value = Reader.ReadLine(separators, new ReadLineOptions() {
                ConsumeDelimiter = false,
            }).Trim();

            tokens.Enqueue(new CookieLexerToken(CookieLexerTokenType.AttributeValue, value));

            return true;

        }

    }

}