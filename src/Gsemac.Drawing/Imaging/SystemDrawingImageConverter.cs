#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class SystemDrawingImageConverter :
            IImageConverter {

        // Public members

        public IEnumerable<string> SupportedImageFormats => ImageUtilities.GetSupportedFileTypes();

        public void ConvertImage(string sourceFilePath, string destinationFilePath, IImageConversionOptions options) {

            if (string.IsNullOrEmpty(sourceFilePath))
                throw new ArgumentNullException(nameof(sourceFilePath));

            if (string.IsNullOrEmpty(destinationFilePath))
                throw new ArgumentNullException(nameof(destinationFilePath));

            sourceFilePath = Path.GetFullPath(sourceFilePath);
            destinationFilePath = Path.GetFullPath(destinationFilePath);

            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("The source file was not found.", sourceFilePath);

            string sourceExt = Path.GetExtension(sourceFilePath);
            bool overwriteSourceFile = sourceFilePath.Equals(destinationFilePath, StringComparison.OrdinalIgnoreCase);

            if (!ImageUtilities.IsSupportedFileType(sourceExt))
                throw new Exception("The image format is not supported.");

            Image image;

            using (image = ImageUtilities.OpenImage(sourceFilePath, openWithoutLocking: overwriteSourceFile)) {

                foreach (IImageFilter filter in options.Filters)
                    image = filter.Apply(image);

                ImageUtilities.SaveImage(image, destinationFilePath, options.EncoderOptions);

            }

        }

    }

}

#endif