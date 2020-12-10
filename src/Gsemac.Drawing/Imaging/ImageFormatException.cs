using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public class ImageFormatException :
        FileFormatException {

        // Public members

        public ImageFormatException() :
            this("The image format is not supported.") {
        }
        public ImageFormatException(string message) :
            base(message) {
        }

    }

}