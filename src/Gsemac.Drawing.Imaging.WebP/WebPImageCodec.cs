#if NETFRAMEWORK

using Gsemac.Drawing.Imaging.Properties;
using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WebPWrapper;

namespace Gsemac.Drawing.Imaging {

    [RequiresAssemblies("libwebp", "libwebpdemux", "libwebpdecoder")]
    [RequiresAssemblyOrTypes("WebPWrapper", "WebPWrapper.WebP")]
    public class WebPImageCodec :
        PluginBase,
        IImageCodec {

        // Public members

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (Bitmap bitmap = image.ToBitmap())
                EncodeWebPBitmap(bitmap, stream, encoderOptions);

        }
        public IImage Decode(Stream stream, IImageDecoderOptions options) {

            byte[] webPData = stream.ToArray();

            using (WebP decoder = new WebP())
            using (Image decodedWebPBitmap = DecodeWebPBitmap(decoder, webPData)) {

                IImage image = ImageFactory.Default.FromBitmap(decodedWebPBitmap, format: ImageFormat.WebP, codec: this);

                int frameCount = 1;
                int animationIterations = 0;
                TimeSpan animationDelay = TimeSpan.Zero;

                // Decode the animation data.

                decoder.GetInfo(webPData, out _, out _, out _, out bool hasAnimation, out string _);

                if (hasAnimation) {

                    // TODO: WebPWrapper doesn't support decoding animated images
                    // Fail on animated images if we're attempting to read anything more than metadata.

                    if (options.Mode != ImageDecoderMode.Metadata)
                        throw new FileFormatException(ExceptionMessages.CannotDecodeAnimatedWebPImage);

                    using (IWebPDemuxer demuxer = new WebPDemuxer(webPData)) {

                        frameCount = demuxer.GetI(WebPFormatFeature.FrameCount);
                        animationIterations = demuxer.GetI(WebPFormatFeature.LoopCount);

                        IWebPFrame frame = demuxer.GetFrame(1); // WebP frame indices are 1-based

                        animationDelay = frame.Duration;

                    }

                }

                return new WebPImage(image, frameCount, animationIterations, animationDelay);

            }

        }

        // Private members

        private IEnumerable<ICodecCapabilities> GetSupportedImageFormats() {

            return new[] {
                ImageFormat.WebP,
            }
            .OrderBy(f => f.Extensions.First())
            .Distinct()
            .Select(f => new CodecCapabilities(f, canRead: true, canWrite: true));

        }

        private void EncodeWebPBitmap(Bitmap bitmap, Stream stream, IImageEncoderOptions encoderOptions) {

            bool useLosslessEncoding = encoderOptions.Quality >= 100 ||
                encoderOptions.OptimizationMode == OptimizationMode.Lossless;

            using (WebP encoder = new WebP())
            using (MemoryStream webPStream = new MemoryStream(useLosslessEncoding ? encoder.EncodeLossless(bitmap) : encoder.EncodeLossy(bitmap, encoderOptions.Quality)))
                webPStream.CopyTo(stream);

        }
        private Image DecodeWebPBitmap(WebP decoder, byte[] webPData) {

            return decoder.Decode(webPData);

        }

    }

}

#endif