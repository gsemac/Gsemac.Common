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

        public IImage Apply(IImage sourceImage) {

            ColorMatrix colorMatrix = new ColorMatrix() {
                Matrix33 = opacity,
            };

            using (ImageAttributes attributes = new ImageAttributes()) {

                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                System.Drawing.Image imageWithAlphaChannel = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);

                using (Bitmap sourceBitmap = sourceImage.ToBitmap(disposeOriginal: true))
                using (Graphics graphics = Graphics.FromImage(imageWithAlphaChannel)) {

                    graphics.Clear(Color.Transparent);
                    graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), 0, 0, sourceBitmap.Width, sourceBitmap.Height, GraphicsUnit.Pixel, attributes);

                }

                return Image.FromBitmap(imageWithAlphaChannel);

            }

        }

        // Private members

        private readonly float opacity;

    }

}

#endif