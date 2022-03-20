using Gsemac.Text.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Text.Ini {

    public class IniLexer :
        LexerBase<IIniLexerToken>,
        IIniLexer {

        // Public members

        public IniLexer(Stream stream) :
            base(stream) {
        }
        public IniLexer(Stream stream, IIniOptions options) :
            this(stream) {

            this.options = options;

        }

        public override IIniLexerToken PeekNextToken() {

            if (!tokens.Any())
                ReadNextTokens();

            return tokens.Any() ? tokens.Peek() : null;

        }
        public override bool ReadNextToken(out IIniLexerToken token) {

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

        // Private members

        private readonly Queue<IIniLexerToken> tokens = new Queue<IIniLexerToken>();
        private readonly IIniOptions options = new IniOptions();

        private void ReadNextTokens() {

            Reader.SkipWhiteSpace();

            if (Reader.TryPeek(out char nextChar)) {

                if (nextChar == '[') {

                    ReadSection();

                }
                else if (nextChar == ';' && options.AllowComments) {

                    ReadComment();

                }
                else {

                    ReadProperty();

                }

            }

        }

        private bool ReadSectionStart() {

            if (EndOfStream)
                return false;

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionStart, ((char)Reader.Read()).ToString()));

            return true;

        }
        private bool ReadSectionName() {

            string value = Reader.ReadLine(new char[] { ']', '\r', '\n' }, allowEscapeSequences: true);

            if (options.Unescape)
                value = IniDocument.Unescape(value);

            // Whitespace surrounding section names is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionName, value.Trim()));

            // If we reached the end of the line rather than the end of the section, the section was not closed.

            if (Reader.TryPeek(out char nextChar) && nextChar.IsNewLine())
                return false;

            return true;

        }
        private bool ReadSectionEnd() {

            if (EndOfStream)
                return false;

            // If we reached the end of the line rather than the end of the section, the section was not closed.

            if (Reader.TryPeek(out char nextChar) && nextChar.IsNewLine())
                return false;

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionEnd, ((char)Reader.Read()).ToString()));

            return true;

        }
        private void ReadCommentStart() {

            if (EndOfStream)
                return;

            Reader.SkipWhiteSpace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.CommentStart, ((char)Reader.Read()).ToString()));

        }
        private void ReadCommentContent() {

            string value = Reader.ReadLine();

            // Whitespace surrounding comments is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.Comment, value.Trim()));

        }
        private bool ReadPropertyName() {

            char[] delimiters = options.AllowComments ? new char[] { '=', ';', '\r', '\n' } : new char[] { '=', '\r', '\n' };
            string value = Reader.ReadLine(delimiters, allowEscapeSequences: true);

            if (options.Unescape)
                value = IniDocument.Unescape(value);

            // Whitespace surrounding property names is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyName, value.Trim()));

            // If we reached the end of the line rather than the end of the property, the property does not have a value.

            if (Reader.TryPeek(out char nextChar) && nextChar.IsNewLine())
                return false;

            return true;

        }
        private bool ReadPropertyValueSeparator() {

            if (EndOfStream)
                return false;

            // If we reached the end of the line rather than a property value separator, the property does not have a value.

            if (Reader.TryPeek(out char nextChar)) {

                if (nextChar.IsNewLine() || nextChar != '=')
                    return false;

            }

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValueSeparator, ((char)Reader.Read()).ToString()));

            return true;

        }
        private bool ReadPropertyValue() {

            char[] delimiters = options.AllowComments ? new char[] { ';', '\r', '\n' } : new char[] { '\r', '\n' };
            string value = Reader.ReadLine(delimiters, allowEscapeSequences: true);

            if (options.Unescape)
                value = IniDocument.Unescape(value);

            // Whitespace surrounding property values is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValue, value.Trim()));

            return true;

        }

        private void ReadSection() {

            if (ReadSectionStart())
                if (ReadSectionName())
                    ReadSectionEnd();

        }
        private void ReadComment() {

            ReadCommentStart();
            ReadCommentContent();

        }
        private void ReadProperty() {

            if (ReadPropertyName())
                if (ReadPropertyValueSeparator())
                    ReadPropertyValue();

        }

    }

}