using Gsemac.IO;
using System;

namespace Gsemac.Drawing.Imaging.Internal {

    public static class ImageExceptions {

        public static Exception UnsupportedImageFormat => new FileFormatException("The image format is not supported.");

    }

}