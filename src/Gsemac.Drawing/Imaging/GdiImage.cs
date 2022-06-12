using Gsemac.IO;
using Gsemac.IO.FileFormats;
using System;
using System.Drawing;
using System.Linq;

using FrameDimension = System.Drawing.Imaging.FrameDimension;

namespace Gsemac.Drawing.Imaging {

    internal class GdiImage :
        ImageBase {

        // Public members

        public override int Width => image.Width;
        public override int Height => image.Height;
        public override IFileFormat Format { get; }
        public override IImageCodec Codec { get; }

        public override TimeSpan AnimationDelay { get; } = TimeSpan.Zero;
        public override int AnimationIterations { get; } = 0;
        public override int FrameCount { get; } = 1;

        public GdiImage(Image originalImage, Image nonDeferredImage, IFileFormat imageFormat, IImageCodec imageCodec) {

            if (originalImage is null)
                throw new ArgumentNullException(nameof(originalImage));

            if (nonDeferredImage is null)
                throw new ArgumentNullException(nameof(nonDeferredImage));

            if (imageFormat is null)
                imageFormat = GetImageFormatFromImageFormat(originalImage.RawFormat);

            image = (Image)nonDeferredImage.Clone();

            Format = imageFormat;
            Codec = imageCodec ?? GetImageCodec(imageFormat);

            FrameCount = GetFrameCount(originalImage);

            if (FrameCount > 1) {

                // These methods can throw due to GetPropertyItem, which throws if a given property item isn't found.
                // However, by ensuring we have frames first, we should be guaranteed that they don't throw.

                AnimationDelay = GetAnimationDelay(originalImage);
                AnimationIterations = GetAnimationIterations(originalImage);

            }

        }
        public GdiImage(Image originalImage, Image nonDeferredImage, IImageCodec imageCodec) :
            this(originalImage, nonDeferredImage, null, imageCodec) {
        }
        public GdiImage(Image image, IImageCodec imageCodec) :
           this(image, image, null, imageCodec) {
        }
        public GdiImage(Image image, IFileFormat imageFormat, IImageCodec imageCodec) :
            this(image, image, imageFormat, imageCodec) {
        }

        public override IImage Clone() {

            return new GdiImage((Bitmap)image.Clone(), Format, Codec);

        }
        public override Bitmap ToBitmap() {

            // The cloned bitmap will share image data with the source bitmap.
            // This allows us to avoid copying the bitmap while allowing the user to call Dispose() on either Bitmap without affecting the other.

            return (Bitmap)image.Clone();

        }

        // Internal members

        internal Image BaseImage => image;

        // Protected members

        protected override void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    image.Dispose();

                }

                disposedValue = true;

            }
        }

        // Private members

        // https://docs.microsoft.com/en-us/windows/win32/gdiplus/-gdiplus-constant-property-item-descriptions

        private const int PropertyTagFrameDelay = 0x5100;
        private const int PropertyTagLoopCount = 0x5101;

        private readonly Image image;
        private bool disposedValue = false;

        private static IImageCodec GetImageCodec(IFileFormat imageFormat) {

            if (imageFormat is null)
                return new GdiImageCodec();

            return new GdiImageCodec(imageFormat);

        }
        private static IFileFormat GetImageFormatFromImageFormat(System.Drawing.Imaging.ImageFormat imageFormat) {

            if (imageFormat is null)
                return null;

            if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                return ImageFormat.Bmp;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                return ImageFormat.Gif;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                return ImageFormat.Png;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                return ImageFormat.Tiff;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                return ImageFormat.Ico;
            else
                return null;

        }

        private static TimeSpan GetAnimationDelay(Image image) {

            // Get the frame delay from the image properties.
            // https://stackoverflow.com/a/3785231/5383169 (Denis Palnitsky)
            // https://web.archive.org/web/20130820015012/http://madskristensen.net/post/Examine-animated-Gife28099s-in-C.aspx
            // We can get the delay for each individual frame, but we'll only get the delay for the first frame (index 0).
            // The delay will be in milliseconds.

            int frameDelay = BitConverter.ToInt32(image.GetPropertyItem(PropertyTagFrameDelay).Value, 0);

            return TimeSpan.FromMilliseconds(frameDelay * 10);

        }
        private static int GetAnimationIterations(Image image) {

            return BitConverter.ToInt16(image.GetPropertyItem(PropertyTagLoopCount).Value, 0);

        }
        private static int GetFrameCount(Image image) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            // Get the Time dimension specifically, as the other dimensions are not applicable to animation.

            FrameDimension frameDimension = image.FrameDimensionsList
                .Where(guid => guid == FrameDimension.Time.Guid)
                .Select(guid => new FrameDimension(guid))
                .FirstOrDefault();

            if (frameDimension is null)
                return 1;

            return image.GetFrameCount(frameDimension);

        }

    }

}