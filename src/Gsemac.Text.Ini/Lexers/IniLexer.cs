using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Text.Extensions;
using Gsemac.Text.Lexers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Text.Ini.Lexers {

    internal class IniLexer :
        LexerBase<IIniLexerToken>,
        IIniLexer {

        // Public members

        public IniLexer(Stream stream) :
            base(new LookaheadStreamReader(stream)) {
        }
        public IniLexer(Stream stream, IIniOptions options) :
            this(stream) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        public override IIniLexerToken Peek() {

            if (!tokens.Any())
                ReadNextTokens();

            return tokens.Any() ? tokens.Peek() : null;

        }
        public override bool Read(out IIniLexerToken token) {

            if (!tokens.Any())
                ReadNextTokens();

            if (tokens.Any()) {

                token = tokens.Dequeue();

                return true;

            }
            else {

                token = null;

                return false;

            }

        }

        // Protected members

        new protected LookaheadStreamReader Reader => (LookaheadStreamReader)base.Reader;

        // Private members

        private const string SectionNameStart = "[";
        private const string SectionNameEnd = "]";
        public const string NewLine = "\n";

        private readonly Queue<IIniLexerToken> tokens = new Queue<IIniLexerToken>();
        private readonly IIniOptions options = new IniOptions();

        private void ReadNextTokens() {

            Reader.SkipWhiteSpace();

            if (!Reader.EndOfText()) {

                if (ReaderHasString(SectionNameStart)) {

                    ReadSection();

                }
                else if (options.CommentMarker.Length > 0 && options.AllowComments && ReaderHasString(options.CommentMarker)) {

                    ReadComment();

                }
                else {

                    ReadProperty();

                }

            }

        }

        private bool ReadSectionStart() {

            if (Reader.EndOfText())
                return false;

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionNameStart, ((char)Reader.Read()).ToString()));

            return true;

        }
        private bool ReadSectionName() {

            string value = Reader.ReadLine(new[] { SectionNameEnd }, new ReadLineOptions() {
                BreakOnNewLine = true,
                ConsumeDelimiter = false,
                IgnoreEscapedDelimiters = options.AllowEscapeSequences,
            });

            if (options.AllowEscapeSequences)
                value = IniUtilities.Unescape(value);

            // Whitespace surrounding section names is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionName, value.Trim()));

            // If we reached the end of the line rather than the end of the section, the section was not closed.

            if (Reader.TryPeek(out char nextChar) && nextChar.IsNewLine())
                return false;

            return true;

        }
        private bool ReadSectionEnd() {

            if (Reader.EndOfText())
                return false;

            // If we reached the end of the line rather than the end of the section, the section was not closed.

            if (Reader.TryPeek(out char nextChar) && nextChar.IsNewLine())
                return false;

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionNameEnd, ((char)Reader.Read()).ToString()));

            return true;

        }
        private void ReadCommentMarker() {

            if (Reader.EndOfText() || options.CommentMarker.Length <= 0)
                return;

            Reader.SkipWhiteSpace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.CommentMarker, Reader.ReadString(options.CommentMarker.Length)));

        }
        private void ReadCommentContent() {

            string value = Reader.ReadLine();

            // Whitespace surrounding comments is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.Comment, value.Trim()));

        }
        private bool ReadPropertyName() {

            string[] delimiters = options.AllowComments ?
                new[] { options.PropertyValueSeparator, options.CommentMarker } :
                new[] { options.PropertyValueSeparator };

            string value = Reader.ReadLine(delimiters, new ReadLineOptions() {
                BreakOnNewLine = true,
                ConsumeDelimiter = false,
                IgnoreEscapedDelimiters = options.AllowEscapeSequences,
            });

            if (options.AllowEscapeSequences)
                value = IniUtilities.Unescape(value);

            // Whitespace surrounding property names is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyName, value.Trim()));

            // If we reached the end of the line rather than the end of the property, the property does not have a value.

            if (Reader.TryPeek(out char nextChar) && nextChar.IsNewLine())
                return false;

            return true;

        }
        private bool ReadPropertyValueSeparator() {

            if (Reader.EndOfText())
                return false;

            // If we reached the end of the line rather than a property value separator, the property does not have a value.

            if (Reader.TryPeek(out char nextChar)) {

                if (nextChar.IsNewLine() || !ReaderHasString(options.PropertyValueSeparator))
                    return false;

            }

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValueSeparator, ((char)Reader.Read()).ToString()));

            return true;

        }
        private bool ReadPropertyValue() {

            string[] delimiters = options.AllowComments ?
                new string[] { options.CommentMarker, NewLine } :
                new string[0];

            string value = Reader.ReadLine(delimiters, new ReadLineOptions() {
                BreakOnNewLine = true,
                ConsumeDelimiter = false,
                IgnoreEscapedDelimiters = options.AllowEscapeSequences,
            });

            if (options.AllowEscapeSequences)
                value = IniUtilities.Unescape(value);

            // Whitespace surrounding property values is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValue, value.Trim()));

            return true;

        }

        private bool ReadSection() {

            return ReadSectionStart() &&
                ReadSectionName() &&
                ReadSectionEnd();

        }
        private void ReadComment() {

            ReadCommentMarker();
            ReadCommentContent();

        }
        private bool ReadProperty() {

            return ReadPropertyName() &&
                ReadPropertyValueSeparator() &&
                ReadPropertyValue();

        }

        private bool ReaderHasString(string value) {

            return Reader.PeekString(value.Length).Equals(value);

        }

    }

}