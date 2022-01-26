#if NETFRAMEWORK

using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public class WatermarkImageFilter :
        IImageFilter {

        // Public members

        public WatermarkImageFilter(Image overlayImage) {

            this.overlayImage = overlayImage;

        }

        public IImage Apply(IImage image) {

            Image newImage = null;

            try {

                newImage = ImageUtilities.ConvertToNonIndexedPixelFormat(image);

                using (Graphics graphics = Graphics.FromImage(newImage)) {

                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                    graphics.DrawImage(overlayImage,
                        new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                        new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                        GraphicsUnit.Pixel);

                }

                return ImageFactory.FromBitmap(newImage);

            }
            catch (Exception) {

                if (newImage is object)
                    newImage.Dispose();

                throw;

            }

        }

        // Private members

        private readonly Image overlayImage;

    }

}

#endif