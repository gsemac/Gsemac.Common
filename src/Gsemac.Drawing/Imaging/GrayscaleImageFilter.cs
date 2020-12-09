#if NETFRAMEWORK

using System.Drawing;
using System.Drawing.Imaging;

namespace Gsemac.Drawing.Imaging {

    public class GrayscaleImageFilter :
        IImageFilter {

        public IImage Apply(IImage sourceImage) {

            Image resultImage = ImageUtilities.ConvertImageToNonIndexedPixelFormat(sourceImage, disposeOriginal: true);

            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                new[]{0.3f, 0.3f, 0.3f, 0.0f, 0.0f},
                new[]{0.59f, 0.59f, 0.59f, 0.0f, 0.0f},
                new[]{0.11f, 0.11f, 0.11f, 0.0f, 0.0f},
                new[]{0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                new[]{0.0f, 0.0f, 0.0f, 0.0f, 1.0f},
            });

            using (ImageAttributes attributes = new ImageAttributes()) {

                attributes.SetColorMatrix(colorMatrix);

                // Note that one of the reasons for drawing on top of new image is so that transluscent pixels from the old image aren't visible through the new one.

                Image imageWithAlphaChannel = new Bitmap(resultImage.Width, resultImage.Height, PixelFormat.Format32bppArgb);

                using (resultImage)
                using (Graphics graphics = Graphics.FromImage(imageWithAlphaChannel)) {

                    graphics.Clear(Color.Transparent);

                    graphics.DrawImage(resultImage, new Rectangle(0, 0, resultImage.Width, resultImage.Height), 0, 0, resultImage.Width, resultImage.Height, GraphicsUnit.Pixel, attributes);

                }

                return new GdiImage(imageWithAlphaChannel, sourceImage.Codec);

            }

        }

    }

}

#endif