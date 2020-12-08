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

            ColorMatrix colorMatrix = new ColorMatrix() {
                Matrix33 = opacity,
            };

            using (ImageAttributes attributes = new ImageAttributes()) {

                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                // Draw the modified image directly on top of the original image.

                Image imageWithAlphaChannel = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);

                using (sourceImage)
                using (Graphics graphics = Graphics.FromImage(imageWithAlphaChannel)) {

                    graphics.Clear(Color.Transparent);
                    graphics.DrawImage(sourceImage, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, attributes);

                }

                return imageWithAlphaChannel;

            }

        }

        // Private members

        private readonly float opacity;

    }

}

#endif