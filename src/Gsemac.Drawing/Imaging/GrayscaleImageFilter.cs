#if NETFRAMEWORK

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Gsemac.Drawing.Imaging {

    public class GrayscaleImageFilter :
        IImageFilter {

        public IImage Apply(IImage image) {

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

                using (Image newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb)) {

                    using (Image sourceBitmap = ImageUtilities.ConvertToNonIndexedPixelFormat(image))
                    using (Graphics graphics = Graphics.FromImage(newImage)) {

                        graphics.Clear(Color.Transparent);

                        graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), 0, 0, sourceBitmap.Width, sourceBitmap.Height, GraphicsUnit.Pixel, attributes);

                    }

                    return ImageFactory.FromBitmap(newImage);

                }

            }

        }

    }

}

#endif