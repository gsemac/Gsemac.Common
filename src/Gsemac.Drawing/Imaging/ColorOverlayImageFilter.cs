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

            using (Image newImage = ImageUtilities.ConvertToNonIndexedPixelFormat(image)) {

                using (Graphics graphics = Graphics.FromImage(newImage))
                using (Brush brush = new SolidBrush(overlayColor))
                    graphics.FillRectangle(brush, new Rectangle(0, 0, newImage.Width, newImage.Height));

                return ImageFactory.FromBitmap(newImage);

            }

        }

        // Private members

        private readonly Color overlayColor;

    }

}

#endif