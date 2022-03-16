using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using Gsemac.IO.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing {

    public abstract class ImageFactoryBase :
        IImageFactory {

        // Public members

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return imageCodecFactory.GetSupportedFileFormats();

        }

        public IImage FromStream(Stream stream, IImageDecoderOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            IFileFormat imageFormat = options.Format;

            if (imageFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out imageFormat);

            IImageCodec imageCodec = imageCodecFactory.FromFileFormat(imageFormat);

            if (imageCodec is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return imageCodec.Decode(stream, options);

        }

#if NETFRAMEWORK

        public IImage FromBitmap(Image bitmap, IBitmapToImageOptions options) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return new GdiImage(bitmap, options.Format, options.Codec);

        }

#endif

        // Protected members

        protected ImageFactoryBase(IImageCodecFactory imageCodecFactory) {

            if (imageCodecFactory is null)
                throw new ArgumentNullException(nameof(imageCodecFactory));

            this.imageCodecFactory = imageCodecFactory;

        }

        // Private members

        private readonly IImageCodecFactory imageCodecFactory;

    }

}