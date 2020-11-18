#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gsemac.Drawing {

    public static class ImageUtilities {

        public static Image OpenImage(string filePath, bool openWithoutLocking = false) {

            if (openWithoutLocking) {

                // This technique allows us to open the image without locking it, allowing the original image to be overwritten.

                using (Image image = Image.FromFile(filePath))
                    return new Bitmap(image);

            }
            else {

                return new Bitmap(filePath);

            }

        }
        public static Image ResizeImage(Image sourceImage, int? width = null, int? height = null) {

            int newWidth = sourceImage.Width;
            int newHeight = sourceImage.Height;

            if (width.HasValue && height.HasValue) {

                newWidth = width.Value;
                newHeight = height.Value;

            }
            else if (width.HasValue) {

                float scaleFactor = (float)width.Value / sourceImage.Width;

                newWidth = width.Value;
                newHeight = (int)(sourceImage.Height * scaleFactor);

            }
            else if (height.HasValue) {

                float scaleFactor = (float)height.Value / sourceImage.Height;

                newWidth = (int)(sourceImage.Width * scaleFactor);
                newHeight = height.Value;

            }

            return new Bitmap(sourceImage, new Size(newWidth, newHeight));

        }

    }

}

#endif