using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Text.Extensions;
using Gsemac.Text.Lexers;
using System;
using System.Collections.Generic;
using System.IO;
using static Gsemac.Text.Ini.IniConstants;

namespace Gsemac.Text.Ini.Lexers {

    internal class IniLexer :
        LexerBase<IIniLexerToken>,
        IIniLexer {

        // Public members

        public IniLexer(Stream stream) :
            this(stream, IniOptions.Default) {
        }
        public IniLexer(Stream stream, IIniOptions options) :
            this(new StreamReader(stream), options) {
        }
        public IniLexer(string value) :
            this(value, IniOptions.Default) {
        }
        public IniLexer(string value, IIniOptions options) :
        this(new StringReader(value), options) {
        }
        public IniLexer(TextReader reader) :
            this(reader, IniOptions.Default) {
        }
        public IniLexer(TextReader reader, IIniOptions options) :
            base(new LookaheadTextReader(reader)) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        // Protected members

        new protected LookaheadTextReader Reader => (LookaheadTextReader)base.Reader;

        protected override void ReadNext(Queue<IIniLexerToken> tokens) {

            Reader.SkipWhiteSpace();

            if (!Reader.EndOfText()) {

                if (Reader.IsNext(SectionNameStart)) {

                    ReadSection(tokens, Reader);

                }
                else if (options.CommentMarker.Length > 0 && options.EnableComments && Reader.IsNext(options.CommentMarker)) {

                    ReadComment(tokens, Reader);

                }
                else {

                    ReadProperty(tokens, Reader);

                }

            }

        }

        // Private members

        private readonly IIniOptions options = new IniOptions();

        private bool ReadSectionName(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            // A section name must appear on its own line, followed by whitespace or an inline comment (if enabled).
            // If we read an invalid section name, it will be treated as a property instead (with or without a value, depending on how it's formatted).

            string[] delimiters = options.EnableInlineComments ?
                new[] { options.CommentMarker, } :
                new string[0];

            // Read the section name.

            string value = reader.ReadLine(delimiters, new ReadLineOptions() {
                BreakOnNewLine = true,
                ConsumeDelimiter = false,
                AllowEscapedDelimiters = options.EnableEscapeSequences,
            }).Trim();

            // The section name is valid if it ends with an UNESCAPED section name end character ("]").

            bool sectionNameIsValid = value.EndsWith(SectionNameEnd) &&
                (!options.EnableEscapeSequences || !value.EndsWith(EscapeCharacter + SectionNameEnd));

            if (sectionNameIsValid) {

                // Remove the outer section name markers from the section name.

                value = IniUtilities.FormatValue(value.Substring(1, value.Length - 2), options, isPropertyValue: false, writingValue: false);

                tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionNameStart, SectionNameStart));
                tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionName, value));
                tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionNameEnd, SectionNameEnd));

            }
            else {

                // The section name is invalid, so reinterpret it as a property.

                using (LookaheadTextReader propertyReader = new LookaheadTextReader(value))
                    ReadProperty(tokens, propertyReader);

            }

            return sectionNameIsValid;

        }
        private void ReadCommentMarker(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            if (reader.EndOfText() || options.CommentMarker.Length <= 0)
                return;

            reader.SkipWhiteSpace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.CommentMarker, reader.ReadString(options.CommentMarker.Length)));

        }
        private void ReadCommentContent(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            string value = reader.ReadLine();

            // Whitespace surrounding comments is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.Comment, value.Trim()));

        }
        private bool ReadPropertyName(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            string[] delimiters = options.EnableInlineComments ?
                new[] { options.NameValueSeparator, options.CommentMarker } :
                new[] { options.NameValueSeparator };

            string value = reader.ReadLine(delimiters, new ReadLineOptions() {
                BreakOnNewLine = true,
                ConsumeDelimiter = false,
                AllowEscapedDelimiters = options.EnableEscapeSequences,
            });

            value = IniUtilities.FormatValue(value, options, isPropertyValue: false, writingValue: false);

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyName, value));

            // If we reached the end of the line rather than the end of the property, the property does not have a value.

            if (reader.TryPeek(out char nextChar) && nextChar.IsNewLine())
                return false;

            return true;

        }
        private bool ReadNameValueSeparator(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            if (reader.EndOfText())
                return false;

            // Skip whitespace leading up to the separator, but do NOT skip newlines, because we don't want to read the following property.

            string[] delimiters = new[] {
                options.NameValueSeparator
            };

            reader.ReadLine(delimiters, new ReadLineOptions() {
                BreakOnNewLine = true,
                ConsumeDelimiter = false,
            });

            // If we reached the end of the line rather than a property value separator, the property does not have a value.

            if (!reader.IsNext(options.NameValueSeparator))
                return false;

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValueSeparator, reader.ReadString(options.NameValueSeparator.Length)));

            return true;

        }
        private bool ReadPropertyValue(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            string[] delimiters = options.EnableInlineComments ?
                new string[] { options.CommentMarker } :
                new string[0];

            string value = reader.ReadLine(delimiters, new ReadLineOptions() {
                BreakOnNewLine = true,
                ConsumeDelimiter = false,
                AllowEscapedDelimiters = options.EnableEscapeSequences,
            });

            value = IniUtilities.FormatValue(value, options, isPropertyValue: true, writingValue: false);

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValue, value));

            return true;

        }
        private bool ReadSection(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            return ReadSectionName(tokens, reader);

        }
        private void ReadComment(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            ReadCommentMarker(tokens, reader);
            ReadCommentContent(tokens, reader);

        }
        private bool ReadProperty(Queue<IIniLexerToken> tokens, LookaheadTextReader reader) {

            return ReadPropertyName(tokens, reader) &&
                ReadNameValueSeparator(tokens, reader) &&
                ReadPropertyValue(tokens, reader);

        }

    }

}