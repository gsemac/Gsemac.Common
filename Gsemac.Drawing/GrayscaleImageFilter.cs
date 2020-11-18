#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Gsemac.Drawing {

    public class GrayscaleImageFilter :
        IImageFilter {

        public Image Apply(Image sourceImage) {

            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                new[]{0.3f, 0.3f, 0.3f, 0.0f, 0.0f},
                new[]{0.59f, 0.59f, 0.59f, 0.0f, 0.0f},
                new[]{0.11f, 0.11f, 0.11f, 0.0f, 0.0f},
                new[]{0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                new[]{0.0f, 0.0f, 0.0f, 0.0f, 1.0f},
            });

            using (ImageAttributes attributes = new ImageAttributes()) {

                attributes.SetColorMatrix(colorMatrix);

                using (Graphics graphics = Graphics.FromImage(sourceImage))
                    graphics.DrawImage(sourceImage, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, attributes);

            }

            return sourceImage;

        }

    }

}

#endif