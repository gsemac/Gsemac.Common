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

        private void ReadNextTokens() {

            SkipWhitespace();

            if (!EndOfStream) {

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

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionStart, ReadCharacter()));

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

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionEnd, ReadCharacter()));

        }
        private void ReadCommentStart() {

            if (EndOfStream)
                return;

            SkipWhitespace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.CommentStart, ReadCharacter()));

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

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValueSeparator, ReadCharacter()));

        }
        private void ReadPropertyValue() {

            StringBuilder valueBuilder = new StringBuilder();

            while (!EndOfStream && !new[] { ';', '\n' }.Any(c => c == (char)Reader.Peek()))
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