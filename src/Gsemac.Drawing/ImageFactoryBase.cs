﻿using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing {

    public abstract class ImageFactoryBase :
        IImageFactory {

        // Public members

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

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

        // Protected members

        protected ImageFactoryBase() :
            this(null) {
        }
        protected ImageFactoryBase(IServiceProvider serviceProvider) {

            imageCodecFactory = new ImageCodecFactory(serviceProvider);

        }

        // Private members

        private readonly IImageCodecFactory imageCodecFactory;

    }

}