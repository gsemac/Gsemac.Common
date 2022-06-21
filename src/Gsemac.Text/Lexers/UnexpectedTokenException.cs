using Gsemac.Text.Properties;
using System;
using System.Runtime.Serialization;

namespace Gsemac.Text.Lexers {

    [Serializable]
    public class UnexpectedTokenException :
        Exception {

        // Public members

        public UnexpectedTokenException() :
            this(ExceptionMessages.UnexpectedToken) {
        }
        public UnexpectedTokenException(string message) :
            base(message) {
        }
        public UnexpectedTokenException(string message, Exception innerException) :
            base(message, innerException) {
        }

        // Protected members

        protected UnexpectedTokenException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

    }

}