using Gsemac.Text.Properties;
using System;
using System.Runtime.Serialization;

namespace Gsemac.Text.Lexers {

    [Serializable]
    public class UnexpectedCharacterException :
        Exception {

        // Public members

        public UnexpectedCharacterException() :
            base(ExceptionMessages.UnexpectedLexerChar) {
        }
        public UnexpectedCharacterException(char value) :
            base(string.Format(ExceptionMessages.UnexpectedLexerCharWithValue, value)) {
        }
        public UnexpectedCharacterException(char value, Exception innerException) :
            base(string.Format(ExceptionMessages.UnexpectedLexerCharWithValue, value), innerException) {
        }

        // Protected members

        protected UnexpectedCharacterException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}