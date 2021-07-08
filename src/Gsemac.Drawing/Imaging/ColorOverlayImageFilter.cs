#if NETFRAMEWORK

using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public class ColorOverlayImageFilter :
        IImageFilter {

        // Public members

        public ColorOverlayImageFilter(Color overlayColor) {

            this.overlayColor = overlayColor;

        }

        public IImage Apply(IImage image) {

            Image newImage = null;

            try {

                newImage = ImageUtilities.ConvertImageToNonIndexedPixelFormat(image);

                using (Graphics graphics = Graphics.FromImage(newImage))
                using (Brush brush = new SolidBrush(overlayColor))
                    graphics.FillRectangle(brush, new Rectangle(0, 0, newImage.Width, newImage.Height));

                return ImageUtilities.CreateImageFromBitmap(newImage);

            }
            catch (Exception) {

                if (newImage is object)
                    newImage.Dispose();

                throw;

            }

        }

        // Private members

        private readonly Color overlayColor;

    }

}

#endif