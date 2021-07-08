using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageFilter {

        public static IImage ApplyAll(IImage image, IEnumerable<IImageFilter> filters) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (filters is null)
                throw new ArgumentNullException(nameof(filters));

            if (!filters.Any())
                return image.Clone();

            IImage sourceImage = image;
            IImage newImage = null;

            foreach (IImageFilter filter in filters) {

                newImage = filter.Apply(sourceImage);

                // Dispose of intermediate images.

                if (!ReferenceEquals(sourceImage, image))
                    sourceImage.Dispose();

                sourceImage = newImage;

            }

            return newImage;

        }

    }

}