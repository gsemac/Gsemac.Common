﻿using Gsemac.Drawing.Imaging;
using ImageMagick;
using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class MagickImage :
        IImage {

        // Public members

        public int Width => image.Width;
        public int Height => image.Height;
        public Size Size => new Size(Width, Height);
        public IImageFormat Format => throw new NotImplementedException();
        public IImageCodec Codec { get; }
        public ImageMagick.MagickImage BaseImage => image;

        public MagickImage(ImageMagick.MagickImage image, IImageCodec codec) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            this.image = image;
            this.Codec = codec;

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

#if NETFRAMEWORK
        public Bitmap ToBitmap(bool disposeOriginal = false) {

            Bitmap bitmap = image.ToBitmap();

            if (disposeOriginal)
                Dispose();

            return bitmap;

        }
#endif

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    image.Dispose();

                }

                disposedValue = true;

            }
        }

        // Private members

        private readonly ImageMagick.MagickImage image;
        private bool disposedValue = false;

    }

}