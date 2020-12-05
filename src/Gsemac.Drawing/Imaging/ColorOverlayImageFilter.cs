#if NETFRAMEWORK

using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public class ColorOverlayImageFilter :
        IImageFilter {

        // Public members

        public ColorOverlayImageFilter(Color overlayColor) {

            this.overlayColor = overlayColor;

        }

        public Image Apply(Image sourceImage) {

            Image resultImage = ImageUtilities.ConvertToNonIndexedPixelFormat(sourceImage, disposeOriginal: true);

            using (Graphics graphics = Graphics.FromImage(resultImage))
            using (Brush brush = new SolidBrush(overlayColor))
                graphics.FillRectangle(brush, new Rectangle(0, 0, resultImage.Width, resultImage.Height));

            return resultImage;

        }

        // Private members

        private readonly Color overlayColor;

    }

}

#endif