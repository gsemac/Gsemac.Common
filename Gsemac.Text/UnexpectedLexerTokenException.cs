using System;

namespace Gsemac.Text {

    [Serializable]
    public class UnexpectedLexerTokenException :
        Exception {

        // Public members

        public UnexpectedLexerTokenException(string message) :
            base(message) {
        }
        public UnexpectedLexerTokenException(string message, Exception innerException) : base(message, innerException) {
        }
        public UnexpectedLexerTokenException() {
        }

        // Protected members

        protected UnexpectedLexerTokenException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}