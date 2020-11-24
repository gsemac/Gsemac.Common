using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Text.Ini {

    public class IniLexer :
        LexerBase<IIniLexerToken>,
        IIniLexer {

        // Public members

        public IniLexer(Stream stream) :
            base(stream) {
        }

        public override IIniLexerToken PeekNextToken() {

            if (!tokens.Any())
                ReadNextTokens();

            return tokens.Any() ? tokens.Peek() : null;

        }
        public override bool ReadNextToken(out IIniLexerToken token) {

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

        private void ReadNextTokens() {

            if (!EndOfStream) {

                SkipWhitespace();

                switch ((char)Reader.Peek()) {

                    case '[':
                        ReadSection();
                        break;

                    case ';':
                        ReadComment();
                        break;

                    default:
                        ReadProperty();
                        break;

                }

            }

        }

        private void ReadSectionStart() {

            if (EndOfStream)
                return;

            SkipWhitespace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionStart, ((char)Reader.Read()).ToString()));

        }
        private void ReadSectionName() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!EndOfStream && !new[] { ']', '\n' }.Any(c => c == (char)Reader.Peek()))
                valueBuilder.Append((char)Reader.Read());

            // Whitespace surrounding section names is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionName, valueBuilder.ToString().Trim()));

        }
        private void ReadSectionEnd() {

            if (EndOfStream)
                return;

            SkipWhitespace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionEnd, ((char)Reader.Read()).ToString()));

        }
        private void ReadCommentStart() {

            if (EndOfStream)
                return;

            SkipWhitespace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.CommentStart, ((char)Reader.Read()).ToString()));

        }
        private void ReadCommentContent() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!EndOfStream && (char)Reader.Peek() != '\n')
                valueBuilder.Append((char)Reader.Read());

            // Whitespace surrounding comments is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.Comment, valueBuilder.ToString().Trim()));

        }
        private void ReadPropertyName() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!EndOfStream && !new[] { '=', '\n' }.Any(c => c == (char)Reader.Peek()))
                valueBuilder.Append((char)Reader.Read());

            // Whitespace surrounding property names is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyName, valueBuilder.ToString().Trim()));

        }
        private void ReadPropertyValueSeparator() {

            if (EndOfStream)
                return;

            SkipWhitespace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValueSeparator, ((char)Reader.Read()).ToString()));

        }
        private void ReadPropertyValue() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!EndOfStream && (char)Reader.Peek() != '\n')
                valueBuilder.Append((char)Reader.Read());

            // Whitespace surrounding property values is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValue, valueBuilder.ToString().Trim()));

        }

        private void ReadSection() {

            ReadSectionStart();
            ReadSectionName();
            ReadSectionEnd();

        }
        private void ReadComment() {

            ReadCommentStart();
            ReadCommentContent();

        }
        private void ReadProperty() {

            ReadPropertyName();
            ReadPropertyValueSeparator();
            ReadPropertyValue();

        }

    }

}