namespace Gsemac.IO {

    public class UnsupportedFileFormatException :
        FileFormatException {

        // Public members

        public UnsupportedFileFormatException() :
            this("The file format is not supported.") {
        }
        public UnsupportedFileFormatException(string message) :
            base(message) {
        }

    }

}