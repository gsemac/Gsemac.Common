#if NETFRAMEWORK

using System.Drawing;
using System.Drawing.Imaging;

namespace Gsemac.Drawing.Imaging {

    public class OpacityImageFilter :
        IImageFilter {

        // Public members

        public OpacityImageFilter(float opacity) {

            this.opacity = opacity;

        }

        public Image Apply(Image sourceImage) {

            Image resultImage = ImageUtilities.ConvertImageToNonIndexedPixelFormat(sourceImage, disposeOriginal: true);

            ColorMatrix colorMatrix = new ColorMatrix() {
                Matrix33 = opacity,
            };

            using (ImageAttributes attributes = new ImageAttributes()) {

                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                // Draw the modified image directly on top of the original image.

                using (Graphics graphics = Graphics.FromImage(resultImage)) {

                    graphics.Clear(Color.Transparent);
                    graphics.DrawImage(resultImage, new Rectangle(0, 0, resultImage.Width, resultImage.Height), 0, 0, resultImage.Width, resultImage.Height, GraphicsUnit.Pixel, attributes);

                }

            }

            return resultImage;

        }

        // Private members

        private readonly float opacity;

    }

}

#endif