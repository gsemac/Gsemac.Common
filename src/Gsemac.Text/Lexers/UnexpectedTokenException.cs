using Gsemac.Text.Properties;
using System;
using System.Runtime.Serialization;

namespace Gsemac.Text.Lexers {

    [Serializable]
    public class UnexpectedTokenException :
        Exception {

        // Public members

        public UnexpectedTokenException() :
            this(ExceptionMessages.UnexpectedLexerToken) {
        }
        public UnexpectedTokenException(string tokenValue) :
            base(string.Format(ExceptionMessages.UnexpectedLexerTokenWithValue, tokenValue)) {
        }
        public UnexpectedTokenException(string tokenValue, Exception innerException) :
            base(string.Format(ExceptionMessages.UnexpectedLexerTokenWithValue, tokenValue), innerException) {
        }

        // Protected members

        protected UnexpectedTokenException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}