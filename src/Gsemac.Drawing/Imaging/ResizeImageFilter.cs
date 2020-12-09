#if NETFRAMEWORK

using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public class ResizeImageFilter :
        IImageFilter {

        // Public members

        public ResizeImageFilter(int? width = null, int? height = null, ImageSizingMode options = ImageSizingMode.None) {

            this.width = width;
            this.height = height;
            this.options = options;

        }
        public ResizeImageFilter(float? horizontalScale = null, float? verticalScale = null) {

            this.horizontalScale = horizontalScale;
            this.verticalScale = verticalScale;

        }

        public IImage Apply(IImage sourceImage) {

            if (!width.HasValue && !height.HasValue && !horizontalScale.HasValue && !verticalScale.HasValue)
                return sourceImage;

            if (options == ImageSizingMode.ResizeIfLarger) {

                if ((!width.HasValue || sourceImage.Width <= width.Value) && (!height.HasValue || sourceImage.Height <= height.Value))
                    return sourceImage;

            }

            if (options == ImageSizingMode.ResizeIfSmaller) {

                if ((!width.HasValue || sourceImage.Width >= width.Value) && (!height.HasValue || sourceImage.Height >= height.Value))
                    return sourceImage;

            }

            int? newWidth = width;
            int? newHeight = height;

            if (!newWidth.HasValue && horizontalScale.HasValue)
                newWidth = (int)(sourceImage.Width * horizontalScale.Value);

            if (!newHeight.HasValue && verticalScale.HasValue)
                newHeight = (int)(sourceImage.Height * verticalScale.Value);

            return new GdiImage(ImageUtilities.ResizeImage(sourceImage.ToBitmap(disposeOriginal: true), newWidth, newHeight, disposeOriginal: true), sourceImage.Codec);

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