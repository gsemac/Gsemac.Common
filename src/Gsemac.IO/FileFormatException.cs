using Gsemac.IO.Properties;
using System;
using System.Runtime.Serialization;

namespace Gsemac.IO {

    [Serializable]
    public class FileFormatException :
        Exception {

        // Public members

        public FileFormatException() :
            this(ExceptionMessages.IncorrectFileFormat) {
        }
        public FileFormatException(string message) :
            base(message) {
        }
        public FileFormatException(string message, Exception innerException) :
            base(message, innerException) {
        }

        // Protected members

        protected FileFormatException(SerializationInfo info, StreamingContext context) :
            base(info, context) {
        }

    }

}