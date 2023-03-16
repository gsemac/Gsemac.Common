#if NETFRAMEWORK

using Gsemac.IO;
using System;
using System.Drawing;
using WebPWrapper;

namespace Gsemac.Drawing.Imaging {

    internal class WebPImage :
        ImageBase {

        // Public members

        public override int Width => image.Width;
        public override int Height => image.Height;
        public override IFileFormat Format => image.Format;
        public override IImageCodec Codec => image.Codec;

        public override TimeSpan AnimationDelay { get; } = TimeSpan.Zero;
        public override int AnimationIterations { get; } = 0;
        public override int FrameCount { get; } = 1;

        public override IImage Clone() {

            return image.Clone();

        }
        public override Bitmap ToBitmap() {

            return image.ToBitmap();

        }

        public WebPImage(IImage image, int frameCount, int animationIterations, TimeSpan animationDelay) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            this.image = image;

            FrameCount = frameCount;
            AnimationIterations = animationIterations;
            AnimationDelay = animationDelay;

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing) {

                image.Dispose();

            }

            base.Dispose(disposing);

        }

        // Private members

        private readonly IImage image;

    }

}

#endif