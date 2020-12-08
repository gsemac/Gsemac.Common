using System;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public static class ImageFilter {

        public static IImage ApplyAll(IImage image, params IImageFilter[] filters) {

            return ApplyAll(image, filters);

        }
        public static IImage ApplyAll(IImage image, IEnumerable<IImageFilter> filters) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (!(filters is null)) {

                foreach (IImageFilter filter in filters)
                    image = filter.Apply(image);

            }

            return image;

        }

    }

}