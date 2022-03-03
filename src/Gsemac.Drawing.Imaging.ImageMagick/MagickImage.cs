using Gsemac.IO;
using ImageMagick;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    internal class MagickImage :
        IImage {

        // Public members

        public IAnimationInfo Animation => GetAnimationInfo();
        public int Width => GetWidth();
        public int Height => GetHeight();
        public Size Size => new Size(Width, Height);
        public IFileFormat Format { get; }
        public IImageCodec Codec { get; }
        internal IMagickImage BaseImage => images.First();

        public MagickImage(Stream stream, IFileFormat imageFormat, IImageCodec codec) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (codec is null)
                throw new ArgumentNullException(nameof(codec));

            // We'll read all image data from the first frame, and then just the metadata for the remaining frames.
            // This allows us to read the image quickly without using too much memory while being able to access animation data.

            // If we were able to determine the desired file format, decode the image using that format explicitly.
            // Otherwise, allow ImageMagick to determine the file format on its own.
            // This is important because ImageMagick isn't able to automatically determine the file format for all formats due to signature conflicts (e.g. ICO).
            // https://github.com/dlemstra/Magick.NET/issues/368

            MagickFormat magickFormat = ImageMagickUtilities.GetMagickFormatFromFileFormat(imageFormat);

            MagickImageCollection images = new MagickImageCollection();

            MagickReadSettings imageReadSettings = new MagickReadSettings() {
                Format = magickFormat,
                FrameCount = 1,
                FrameIndex = 0,
            };

            MagickReadSettings metadataReadSettings = new MagickReadSettings() {
                Format = magickFormat,
            };

            // We have to reset the stream to do the ping after the read, or else ImageMagick complains about invalid image headers.
            // Therefore we either need to read the entire image, or seek backwards (for seekable streams).

            if (stream.CanSeek) {

                images.Read(stream, imageReadSettings);

                stream.Seek(0, SeekOrigin.Begin);

                images.Ping(stream, metadataReadSettings);

            }
            else {

                images.Read(stream);

            }

            this.images = images;

            Format = imageFormat;
            Codec = codec;

        }

        public IImage Clone() {

            return new MagickImage((MagickImageCollection)images.Clone(), Format, Codec);

        }
#if NETFRAMEWORK
        public Bitmap ToBitmap() {

            return images.FirstOrDefault()?.ToBitmap();

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

                    images.Dispose();

                }

                disposedValue = true;

            }
        }

        // Private members

        private readonly MagickImageCollection images;
        private bool disposedValue = false;

        private MagickImage(MagickImageCollection images, IFileFormat imageFormat, IImageCodec codec) {

            // This constructor is used when cloning the image.

            if (images is null)
                throw new ArgumentNullException(nameof(images));

            if (codec is null)
                throw new ArgumentNullException(nameof(codec));

            this.images = images;

            Format = imageFormat;
            Codec = codec;

        }

        private IAnimationInfo GetAnimationInfo() {

            IMagickImage initialImage = images.FirstOrDefault();

            int frameCount = images.Count;
            TimeSpan frameDelay = TimeSpan.FromMilliseconds(initialImage.AnimationDelay * 10);
            int loopCount = initialImage.AnimationIterations;

            // If we have multiple frames but no animation, set the frame count to 1.
            // The idea is that images containing multiple resolutions should not have this information reflected in their animation info.

            if (images.All(image => image.AnimationDelay <= 0))
                frameCount = Math.Min(frameCount, 1);

            return new AnimationInfo(frameCount, loopCount, frameDelay);

        }
        private int GetWidth() {

            return images.FirstOrDefault()?.Width ?? 0;

        }
        private int GetHeight() {

            return images.FirstOrDefault()?.Height ?? 0;

        }

    }

}