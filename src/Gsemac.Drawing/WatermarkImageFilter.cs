#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gsemac.Drawing {

    public class WatermarkImageFilter :
        IImageFilter {

        // Public members

        public WatermarkImageFilter(Image overlayImage) {

            this.overlayImage = overlayImage;

        }

        public Image Apply(Image sourceImage) {

            Image resultImage = sourceImage;

            if (sourceImage.HasIndexedPixelFormat()) {

                // We can't create a graphics object from an image with an indexed pixel format, so we need to create a new bitmap.

                using (sourceImage)
                    resultImage = new Bitmap(sourceImage);

            }

            // Draw the modified image directly on top of the original image.

            using (Graphics graphics = Graphics.FromImage(resultImage)) {

                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                graphics.DrawImage(overlayImage,
                    new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                    new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                    GraphicsUnit.Pixel);

            }

            return resultImage;

        }

        // Private members

        private readonly Image overlayImage;

    }

}

#endif