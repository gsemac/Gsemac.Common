using System;

namespace Gsemac.IO {

    public sealed class UnsupportedFileFormatException :
        FileFormatException {

        // Public members

        public UnsupportedFileFormatException() :
            this(Properties.ExceptionMessages.UnsupportedFileFormat) {
        }
        public UnsupportedFileFormatException(string message) :
            base(message) {
        }
        public UnsupportedFileFormatException(string message, Exception innerException) :
            base(message, innerException) {
        }

    }

}