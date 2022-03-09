#if NETFRAMEWORK

using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public class ResizeImageFilter :
        IImageFilter {

        // Public members

        public ResizeImageFilter(int? width = null, int? height = null, ImageSizingMode options = ImageSizingMode.None) {

            this.width = (width ?? 0) <= 0 ? null : width;
            this.height = (height ?? 0) <= 0 ? null : height;
            this.options = options;

        }
        public ResizeImageFilter(float? horizontalScale = null, float? verticalScale = null) {

            this.horizontalScale = horizontalScale;
            this.verticalScale = verticalScale;

        }

        public IImage Apply(IImage image) {

            if (!width.HasValue && !height.HasValue && !horizontalScale.HasValue && !verticalScale.HasValue)
                return image.Clone();

            if (options == ImageSizingMode.ResizeIfLarger) {

                if ((!width.HasValue || image.Width <= width.Value) && (!height.HasValue || image.Height <= height.Value))
                    return image.Clone();

            }

            if (options == ImageSizingMode.ResizeIfSmaller) {

                if ((!width.HasValue || image.Width >= width.Value) && (!height.HasValue || image.Height >= height.Value))
                    return image.Clone();

            }

            int? newWidth = width;
            int? newHeight = height;

            if (!newWidth.HasValue && horizontalScale.HasValue)
                newWidth = (int)(image.Width * horizontalScale.Value);

            if (!newHeight.HasValue && verticalScale.HasValue)
                newHeight = (int)(image.Height * verticalScale.Value);

            // If the image hasn't been resized at all, just return the source image.

            if (!newWidth.HasValue && !newHeight.HasValue)
                return image.Clone();

            using (Bitmap sourceBitmap = image.ToBitmap())
            using (Image resizedBitmap = ImageUtilities.ResizeImage(sourceBitmap, newWidth, newHeight))
                return ImageFactory.Default.FromBitmap(resizedBitmap);

        }

        // Private members

        private readonly int? width;
        private readonly int? height;
        private readonly float? horizontalScale;
        private readonly float? verticalScale;
        private readonly ImageSizingMode options = ImageSizingMode.None;

    }

}

#endif