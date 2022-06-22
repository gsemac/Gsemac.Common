﻿using Gsemac.Text.Extensions;
using Gsemac.Text.Lexers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Text.Ini.Lexers {

    internal class IniLexer :
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

        public override IIniLexerToken Peek() {

            if (!tokens.Any())
                ReadNextTokens();

            return tokens.Any() ? tokens.Peek() : null;

        }
        public override bool ReadToken(out IIniLexerToken token) {

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
                else if (options.CommentMarker.Length > 0 && options.AllowComments && nextChar == options.CommentMarker.First()) {

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
                value = IniUtilities.Unescape(value);

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
        private void ReadCommentMarker() {

            if (EndOfStream || options.CommentMarker.Length <= 0)
                return;

            Reader.SkipWhiteSpace();

            ReadStringAndThrowOnUnexpectedCharacters(options.CommentMarker);

            tokens.Enqueue(new IniLexerToken(IniLexerTokenType.CommentMarker, options.CommentMarker));

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
                value = IniUtilities.Unescape(value);

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

        private void ReadStringAndThrowOnUnexpectedCharacters(string stringToRead) {

            foreach (char c in stringToRead) {

                int nextChar = Reader.Read();

                if (nextChar == -1)
                    break;

                if ((char)nextChar != c)
                    throw new UnexpectedCharacterException((char)nextChar);

            }

        }

    }

}