﻿#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Gsemac.Drawing.Imaging {

    public class GrayscaleImageFilter :
        IImageFilter {

        public Image Apply(Image sourceImage) {

            Image resultImage = sourceImage;

            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                new[]{0.3f, 0.3f, 0.3f, 0.0f, 0.0f},
                new[]{0.59f, 0.59f, 0.59f, 0.0f, 0.0f},
                new[]{0.11f, 0.11f, 0.11f, 0.0f, 0.0f},
                new[]{0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                new[]{0.0f, 0.0f, 0.0f, 0.0f, 1.0f},
            });

            using (ImageAttributes attributes = new ImageAttributes()) {

                attributes.SetColorMatrix(colorMatrix);

                if (sourceImage.HasIndexedPixelFormat()) {

                    // We can't create a graphics object from an image with an indexed pixel format, so we need to create a new bitmap.

                    using (sourceImage)
                        resultImage = new Bitmap(sourceImage);

                }

                // Draw the modified image directly on top of the original image.

                using (Graphics graphics = Graphics.FromImage(resultImage))
                    graphics.DrawImage(resultImage, new Rectangle(0, 0, resultImage.Width, resultImage.Height), 0, 0, resultImage.Width, resultImage.Height, GraphicsUnit.Pixel, attributes);

            }

            return resultImage;

        }

    }

}

#endif