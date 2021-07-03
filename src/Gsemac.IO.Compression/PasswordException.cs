using System;
using System.Runtime.Serialization;

namespace Gsemac.IO.Compression {

    [Serializable]
    public class PasswordException :
        Exception {

        // Public members

        public PasswordException(string message) :
            base(message) {
        }
        public PasswordException(string message, Exception innerException) :
            base(message, innerException) {
        }
        public PasswordException() :
            this(Properties.ExceptionMessages.PasswordIsMissingOrIncorrect) {
        }

        // Protected members

        protected PasswordException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }
    }

}