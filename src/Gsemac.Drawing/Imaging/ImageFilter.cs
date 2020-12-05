#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public static class ImageFilter {

        public static Image ApplyAll(Image image, params IImageFilter[] filters) {

            return ApplyAll(image, filters);

        }
        public static Image ApplyAll(Image image, IEnumerable<IImageFilter> filters) {

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

#endif