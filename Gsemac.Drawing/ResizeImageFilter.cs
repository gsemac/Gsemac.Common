#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gsemac.Drawing {

    public class ResizeImageFilter :
        IImageFilter {

        // Public members

        public ResizeImageFilter(int? width = null, int? height = null) {

            this.width = width;
            this.height = height;

        }

        public Image Apply(Image sourceImage) {

            if (!width.HasValue && !height.HasValue)
                return sourceImage;

            using (sourceImage)
                return ImageUtilities.ResizeImage(sourceImage, width, height);

        }

        // Private members

        private readonly int? width;
        private readonly int? height;

    }

}

#endif