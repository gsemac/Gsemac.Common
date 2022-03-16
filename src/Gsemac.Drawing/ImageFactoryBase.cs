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

        public IImage FromStream(Stream stream, IFileFormat imageFormat = null) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (imageFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out imageFormat);

            IImageCodec imageCodec = imageCodecFactory.FromFileFormat(imageFormat);

            if (imageCodec is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return imageCodec.Decode(stream);

        }

#if NETFRAMEWORK

        public IImage FromBitmap(Image bitmap, IBitmapToImageOptions options = null) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (options is null)
                options = BitmapToImageOptions.Default;

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