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

        public WebPImage(IImage image, WebP decoder, byte[] webPData) {

            this.image = image;

            // Get the animation info.

            decoder.GetInfo(webPData, out _, out _, out _, out bool hasAnimation, out string _);

            if (hasAnimation) {

                using (IWebPDemuxer demuxer = new WebPDemuxer(webPData)) {

                    FrameCount = demuxer.GetI(WebPFormatFeature.FrameCount);
                    AnimationIterations = demuxer.GetI(WebPFormatFeature.LoopCount);

                    IWebPFrame frame = demuxer.GetFrame(1); // WebP frame indices are 1-based

                    AnimationDelay = frame.Duration;

                }

            }

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