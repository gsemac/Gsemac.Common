using Gsemac.IO;
using ImageMagick;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    internal class MagickImage :
        ImageBase {

        // Public members

        public override int Width => GetWidth();
        public override int Height => GetHeight();
        public override IFileFormat Format { get; }
        public override IImageCodec Codec { get; }

        public override TimeSpan AnimationDelay => GetAnimationDelay();
        public override int AnimationIterations => GetAnimationIterations();
        public override int FrameCount => GetFrameCount();

        public MagickImage(Stream stream, IFileFormat imageFormat, IImageCodec codec) :
            this(stream, imageFormat, codec, ImageDecoderOptions.Default) {
        }
        public MagickImage(Stream stream, IFileFormat imageFormat, IImageCodec codec, IImageDecoderOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (codec is null)
                throw new ArgumentNullException(nameof(codec));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // If we were able to determine the desired file format, decode the image using that format explicitly.
            // Otherwise, allow ImageMagick to determine the file format on its own.
            // This is important because ImageMagick isn't able to automatically determine the file format for all formats due to signature conflicts (e.g. ICO).
            // https://github.com/dlemstra/Magick.NET/issues/368

            MagickFormat magickFormat = ImageMagickUtilities.GetMagickFormatFromFileFormat(imageFormat);

            MagickImageCollection images = new MagickImageCollection();

            MagickReadSettings readSettings = new MagickReadSettings() {
                Format = magickFormat,
            };

            if (options.Mode == ImageDecoderMode.Metadata) {

                // Just load the metadata from the image.

                images.Ping(stream, readSettings);

            }
            else if (options.Mode == ImageDecoderMode.Static) {

                // Read the initial image, and let ImageMagick figure out how to read it most efficiently.
                // Attempting to use MagickImageCollection to just read the first frame works for some files,
                // but only when the first frame contains complete image data (i.e. no diff information).

                ImageMagick.MagickImage initialImage = new ImageMagick.MagickImage(stream, magickFormat);

                // We have to reset the stream to do the ping after the read, or else ImageMagick complains about invalid image headers.
                // Therefore we either need to read the entire image, or seek backwards (for seekable streams). 

                if (stream.CanSeek) {

                    stream.Seek(0, SeekOrigin.Begin);

                    images.Ping(stream, readSettings);

                }

                // The initial image is inserted after the call to Ping so that it is not replaced.
                // We don't have to worry about calling Dispose because MagickImageCollection's Dispose method calls Dispose on all images.

                images.Insert(0, initialImage);

            }
            else {

                // Simply read the image fully, including all frames and metadata.

                images.Read(stream, readSettings);

            }

            this.images = images;

            Format = imageFormat;
            Codec = codec;

        }

        public override IImage Clone() {

            return new MagickImage((MagickImageCollection)images.Clone(), Format, Codec);

        }

#if NETFRAMEWORK

        public override Bitmap ToBitmap() {

            return images.FirstOrDefault()?.ToBitmap();

        }

#endif

        // Internal members

        internal IMagickImage BaseImage => images.First();

        // Protected members

        protected override void Dispose(bool disposing) {

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

        private int GetWidth() {

            return images.FirstOrDefault()?.Width ?? 0;

        }
        private int GetHeight() {

            return images.FirstOrDefault()?.Height ?? 0;

        }

        private TimeSpan GetAnimationDelay() {

            return TimeSpan.FromMilliseconds(BaseImage.AnimationDelay * 10);

        }
        private int GetAnimationIterations() {

            return BaseImage.AnimationIterations;

        }
        private int GetFrameCount() {

            int frameCount = images.Count;

            // If we have multiple frames but no animation, set the frame count to 1.
            // The idea is that images containing multiple resolutions should not have this information reflected in their animation info.

            if (images.All(image => image.AnimationDelay <= 0))
                frameCount = Math.Min(frameCount, 1);

            return frameCount;

        }

    }

}