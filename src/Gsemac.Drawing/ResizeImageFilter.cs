#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gsemac.Drawing {

    public class ResizeImageFilter :
        IImageFilter {

        // Public members

        public ResizeImageFilter(int? width = null, int? height = null, ImageSizingMode options = ImageSizingMode.None) {

            this.width = width;
            this.height = height;
            this.options = options;

        }

        public Image Apply(Image sourceImage) {

            if (!width.HasValue && !height.HasValue)
                return sourceImage;

            if (options == ImageSizingMode.ResizeIfLarger) {

                if ((!width.HasValue || sourceImage.Width <= width.Value) && (!height.HasValue || sourceImage.Height <= height.Value))
                    return sourceImage;

            }

            if (options == ImageSizingMode.ResizeIfSmaller) {

                if ((!width.HasValue || sourceImage.Width >= width.Value) && (!height.HasValue || sourceImage.Height >= height.Value))
                    return sourceImage;

            }

            using (sourceImage)
                return ImageUtilities.ResizeImage(sourceImage, width, height);

        }

        // Private members

        private readonly int? width;
        private readonly int? height;
        private readonly ImageSizingMode options;

    }

}

#endif