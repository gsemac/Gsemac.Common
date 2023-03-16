#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging.Filters
{

    public class ResizeImageFilter :
        IImageFilter {

        // Public members

        public ResizeImageFilter(IImageResizingOptions resizingOptions) {

            if (resizingOptions is null)
                throw new ArgumentNullException(nameof(resizingOptions));

            options = resizingOptions;

        }

        public IImage Apply(IImage image) {

            using (Bitmap sourceBitmap = image.ToBitmap())
            using (Image resizedBitmap = sourceBitmap.Resize(options))
                return ImageFactory.Default.FromBitmap(resizedBitmap);

        }

        // Private members

        private readonly IImageResizingOptions options;

    }

}

#endif