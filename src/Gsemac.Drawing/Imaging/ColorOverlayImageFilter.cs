#if NETFRAMEWORK

using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public class ColorOverlayImageFilter :
        IImageFilter {

        // Public members

        public ColorOverlayImageFilter(Color overlayColor) {

            this.overlayColor = overlayColor;

        }

        public IImage Apply(IImage sourceImage) {

            Image resultImage = ImageUtilities.ConvertImageToNonIndexedPixelFormat(sourceImage, disposeOriginal: true);

            using (Graphics graphics = Graphics.FromImage(resultImage))
            using (Brush brush = new SolidBrush(overlayColor))
                graphics.FillRectangle(brush, new Rectangle(0, 0, resultImage.Width, resultImage.Height));

            return ImageUtilities.CreateImageFromBitmap(resultImage);

        }

        // Private members

        private readonly Color overlayColor;

    }

}

#endif