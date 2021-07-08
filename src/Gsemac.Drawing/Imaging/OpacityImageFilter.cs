#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Gsemac.Drawing.Imaging {

    public class OpacityImageFilter :
        IImageFilter {

        // Public members

        public OpacityImageFilter(float opacity) {

            this.opacity = opacity;

        }

        public IImage Apply(IImage image) {

            Image newImage = null;

            try {

                ColorMatrix colorMatrix = new ColorMatrix() {
                    Matrix33 = opacity,
                };

                using (ImageAttributes attributes = new ImageAttributes()) {

                    attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    // Create a new bitmap with an alpha channel.

                    newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

                    using (Image sourceBitmap = ImageUtilities.ConvertImageToNonIndexedPixelFormat(image))
                    using (Graphics graphics = Graphics.FromImage(newImage)) {

                        graphics.Clear(Color.Transparent);
                        graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), 0, 0, sourceBitmap.Width, sourceBitmap.Height, GraphicsUnit.Pixel, attributes);

                    }

                    return ImageUtilities.CreateImageFromBitmap(newImage);

                }

            }
            catch (Exception) {

                if (newImage is object)
                    newImage.Dispose();

                throw;

            }

        }

        // Private members

        private readonly float opacity;

    }

}

#endif