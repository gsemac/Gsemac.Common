﻿#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gsemac.Drawing.Imaging {

    public class WatermarkImageFilter :
        IImageFilter {

        // Public members

        public WatermarkImageFilter(Image overlayImage) {

            this.overlayImage = overlayImage;

        }

        public IImage Apply(IImage sourceImage) {

            Image resultImage = ImageUtilities.ConvertImageToNonIndexedPixelFormat(sourceImage, disposeOriginal: true);

            // Draw the modified image directly on top of the original image.

            using (Graphics graphics = Graphics.FromImage(resultImage)) {

                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                graphics.DrawImage(overlayImage,
                    new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                    new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                    GraphicsUnit.Pixel);

            }

            return new GdiImage(resultImage);

        }

        // Private members

        private readonly Image overlayImage;

    }

}

#endif