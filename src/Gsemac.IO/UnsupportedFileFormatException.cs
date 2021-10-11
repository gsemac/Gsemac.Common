using System;
using System.Runtime.Serialization;

namespace Gsemac.IO {

    [Serializable]
    public sealed class UnsupportedFileFormatException :
        FileFormatException {

        // Public members

        public UnsupportedFileFormatException(string message) :
            base(message) {
        }
        public UnsupportedFileFormatException(string message, Exception innerException) :
            base(message, innerException) {
        }
        public UnsupportedFileFormatException() :
            base(GetExceptionMessage(fileFormat: null)) {
        }
        public UnsupportedFileFormatException(IFileFormat fileFormat) :
            base(GetExceptionMessage(fileFormat)) {
        }

        // Private members

        private UnsupportedFileFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext) {
        }

        private static string GetExceptionMessage(IFileFormat fileFormat) {

            return fileFormat is null ?
                Properties.ExceptionMessages.UnsupportedFileFormat :
                string.Format(Properties.ExceptionMessages.UnsupportedFileFormatWithFormat, fileFormat);

        }

    }

}