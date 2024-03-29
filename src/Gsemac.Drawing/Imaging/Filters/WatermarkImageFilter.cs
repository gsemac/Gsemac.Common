﻿#if NETFRAMEWORK

using System.Drawing;

namespace Gsemac.Drawing.Imaging.Filters
{

    public class WatermarkImageFilter :
        IImageFilter {

        // Public members

        public WatermarkImageFilter(Image overlayImage) {

            this.overlayImage = overlayImage;

        }

        public IImage Apply(IImage image) {

            using (Image newImage = ImageUtilities.ConvertToNonIndexedPixelFormat(image))
            using (Graphics graphics = Graphics.FromImage(newImage)) {

                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                graphics.DrawImage(overlayImage,
                    new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                    new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                    GraphicsUnit.Pixel);

                return ImageFactory.Default.FromBitmap(newImage);

            }

        }

        // Private members

        private readonly Image overlayImage;

    }

}

#endif