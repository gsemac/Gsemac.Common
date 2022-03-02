using Gsemac.IO;
using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public abstract class ImageBase :
        IImage {

        // Public members

        public virtual IAnimationInfo Animation => AnimationInfo.Static;

        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract IFileFormat Format { get; }
        public abstract IImageCodec Codec { get; }

        public Size Size => new Size(Width, Height);

        public abstract IImage Clone();

#if NETFRAMEWORK
        public abstract Bitmap ToBitmap();
#endif

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {
        }

    }

}