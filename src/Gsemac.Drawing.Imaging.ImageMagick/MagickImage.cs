using Gsemac.IO;
using ImageMagick;
using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    internal class MagickImage :
        IImage {

        // Public members

        public IAnimationInfo Animation => GetAnimationInfo();
        public int Width => image.Width;
        public int Height => image.Height;
        public Size Size => new Size(Width, Height);
        public IFileFormat Format { get; }
        public IImageCodec Codec { get; }
        internal ImageMagick.MagickImage BaseImage => image;

        public MagickImage(ImageMagick.MagickImage image, IImageCodec codec) :
            this(image, GetImageFormatFromMagickFormat(image.Format), codec) {
        }
        public MagickImage(ImageMagick.MagickImage image, IFileFormat imageFormat, IImageCodec codec) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (codec is null)
                throw new ArgumentNullException(nameof(codec));

            this.image = image;
            this.Format = imageFormat;
            this.Codec = codec;

        }

        public IImage Clone() {

            return Format is null ?
                  new MagickImage((ImageMagick.MagickImage)image.Clone(), Codec) :
                  new MagickImage((ImageMagick.MagickImage)image.Clone(), Format, Codec);

        }
#if NETFRAMEWORK
        public Bitmap ToBitmap() {

            return image.ToBitmap();

        }
#endif

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    image.Dispose();

                }

                disposedValue = true;

            }
        }

        // Private members

        private readonly ImageMagick.MagickImage image;
        private bool disposedValue = false;

        private IAnimationInfo GetAnimationInfo() {

            int frameCount = 0;
            TimeSpan frameDelay = TimeSpan.FromMilliseconds(image.AnimationDelay * 10);
            int loopCount = image.AnimationIterations;

            // TODO: Get the frame count using MagickImageCollection's Ping method.
            // This requires access to the stream that the image was created from.

            return new AnimationInfo(frameCount, loopCount, frameDelay);

        }

        private static IFileFormat GetImageFormatFromMagickFormat(MagickFormat magickFormat) {

            string ext = ImageMagickUtilities.GetFileExtensionFromMagickFormat(magickFormat);

            if (string.IsNullOrEmpty(ext))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return FileFormatFactory.Default.FromFileExtension(ext);

        }

    }

}