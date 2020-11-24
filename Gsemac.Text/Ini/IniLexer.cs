using Gsemac.Core.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Text.Ini {

    public class IniLexer :
        LexerBase<IIniLexerToken>,
        IIniLexer {

        // Public members

        public bool Unescape { get; set; } = true;
        public bool AllowComments { get; set; } = true;

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

                char? nextChar = PeekCharacter();

                if (nextChar == '[') {

                    ReadSection();

                }
                else if (nextChar == ';' && AllowComments) {

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

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionStart, ReadCharacter().Value.ToString()));

            return true;

        }
        private bool ReadSectionName() {

            string value = ReadUntilAny(']', '\r', '\n');

            if (Unescape)
                value = IniFile.Unescape(value);

            // Whitespace surrounding section names is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionName, value.Trim()));

            // If we reached the end of the line rather than the end of the section, the section was not closed.

            if (PeekCharacter()?.IsNewLine() ?? false)
                return false;

            return true;

        }
        private bool ReadSectionEnd() {

            if (EndOfStream)
                return false;

            // If we reached the end of the line rather than the end of the section, the section was not closed.

            if (PeekCharacter()?.IsNewLine() ?? false)
                return false;

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.SectionEnd, ReadCharacter().Value.ToString()));

            return true;

        }
        private void ReadCommentStart() {

            if (EndOfStream)
                return;

            SkipWhitespace();

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.CommentStart, ReadCharacter().Value.ToString()));

        }
        private void ReadCommentContent() {

            string value = Reader.ReadLine();

            // Whitespace surrounding comments is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.Comment, value.Trim()));

        }
        private bool ReadPropertyName() {

            string value = AllowComments ? ReadUntilAny('=', ';', '\n') : ReadUntilAny('=', '\n');

            if (Unescape)
                value = IniFile.Unescape(value);

            // Whitespace surrounding property names is ignored.

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyName, value.Trim()));

            // If we reached the end of the line rather than the end of the property, the property does not have a value.

            if (PeekCharacter()?.IsNewLine() ?? false)
                return false;

            return true;

        }
        private bool ReadPropertyValueSeparator() {

            if (EndOfStream)
                return false;

            // If we reached the end of the line rather than a property value separator, the property does not have a value.

            if (PeekCharacter()?.IsNewLine() ?? false)
                return false;

            if (PeekCharacter() != '=')
                return false;

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.PropertyValueSeparator, ReadCharacter().Value.ToString()));

            return true;

        }
        private bool ReadPropertyValue() {

            string value = AllowComments ? ReadUntilAny(';', '\n') : Reader.ReadLine();

            if (Unescape)
                value = IniFile.Unescape(value);

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