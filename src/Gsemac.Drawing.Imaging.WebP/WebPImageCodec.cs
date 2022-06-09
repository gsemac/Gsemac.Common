﻿#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
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

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

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

                IAnimationInfo animationInfo = GetAnimationInfo(decoder, webPData);

                return new WebPImage(image, animationInfo);

            }

        }

        // Private members

        private IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return new[] {
                ImageFormat.WebP,
            };

        }

        private void EncodeWebPBitmap(Bitmap bitmap, Stream stream, IImageEncoderOptions encoderOptions) {

            bool useLosslessEncoding = encoderOptions.Quality >= 100 ||
                encoderOptions.OptimizationMode == ImageOptimizationMode.Lossless;

            using (WebP encoder = new WebP())
            using (MemoryStream webPStream = new MemoryStream(useLosslessEncoding ? encoder.EncodeLossless(bitmap) : encoder.EncodeLossy(bitmap, encoderOptions.Quality)))
                webPStream.CopyTo(stream);

        }
        private Image DecodeWebPBitmap(WebP decoder, byte[] webPData) {

            return decoder.Decode(webPData);

        }

        private static IAnimationInfo GetAnimationInfo(WebP decoder, byte[] webPData) {

            decoder.GetInfo(webPData, out _, out _, out _, out bool hasAnimation, out string _);

            if (!hasAnimation)
                return AnimationInfo.Static;

            // Get the animation info.

            using (IWebPDemuxer demuxer = new WebPDemuxer(webPData)) {

                int frameCount = demuxer.GetI(WebPFormatFeature.FrameCount);
                int loopCount = demuxer.GetI(WebPFormatFeature.LoopCount);

                IWebPFrame frame = demuxer.GetFrame(1); // WebP frame indices are 1-based

                return new AnimationInfo(frameCount, loopCount, frame.Duration);

            }

        }

    }

}

#endif