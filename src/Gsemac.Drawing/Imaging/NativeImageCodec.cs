#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class NativeImageCodec :
        IImageCodec {

        public IEnumerable<string> SupportedFileTypes => ImageCodec.NativelySupportedFileTypes;

        public Image Decode(Stream stream) {

            return Image.FromStream(stream);

        }
        public void Encode(Image image, Stream stream, IImageEncoderOptions options) {

            Encode(image, stream, ImageFormat.Png, options);

        }
        public void Encode(Image image, Stream stream, ImageFormat imageFormat, IImageEncoderOptions options) {

            using (EncoderParameters encoderParameters = new EncoderParameters(1))
            using (EncoderParameter qualityParameter = new EncoderParameter(Encoder.Quality, options.Quality)) {

                encoderParameters.Param[0] = qualityParameter;

                ImageCodecInfo encoder = GetEncoderForImageFormat(imageFormat);

                if (encoder is null)
                    throw new ArgumentException(nameof(imageFormat));

                image.Save(stream, encoder, encoderParameters);

            }

        }

        // Private members

        private ImageCodecInfo GetEncoderForImageFormat(ImageFormat imageFormat) {

            ImageCodecInfo decoder = ImageCodecInfo.GetImageDecoders()
                .Where(codec => codec.FormatID == imageFormat.Guid)
                .FirstOrDefault();

            return decoder;

        }

    }

}

#endif