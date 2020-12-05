#if NETFRAMEWORK

using System.Drawing;
using System.Drawing.Imaging;

namespace Gsemac.Drawing.Imaging {

    public class GrayscaleImageFilter :
        IImageFilter {

        public Image Apply(Image sourceImage) {

            Image resultImage = ImageUtilities.ConvertToNonIndexedPixelFormat(sourceImage, disposeOriginal: true);

            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                new[]{0.3f, 0.3f, 0.3f, 0.0f, 0.0f},
                new[]{0.59f, 0.59f, 0.59f, 0.0f, 0.0f},
                new[]{0.11f, 0.11f, 0.11f, 0.0f, 0.0f},
                new[]{0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                new[]{0.0f, 0.0f, 0.0f, 0.0f, 1.0f},
            });

            using (ImageAttributes attributes = new ImageAttributes()) {

                attributes.SetColorMatrix(colorMatrix);

                // Draw the modified image directly on top of the original image.

                using (Graphics graphics = Graphics.FromImage(resultImage))
                    graphics.DrawImage(resultImage, new Rectangle(0, 0, resultImage.Width, resultImage.Height), 0, 0, resultImage.Width, resultImage.Height, GraphicsUnit.Pixel, attributes);

            }

            return resultImage;

        }

    }

}

#endif