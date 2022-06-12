using Gsemac.Drawing.Properties;
using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public abstract class ImageBase :
        IImage {

        // Public members

        public abstract int Width { get; }
        public abstract int Height { get; }
        public Size Size => new Size(Width, Height);
        public abstract IFileFormat Format { get; }
        public abstract IImageCodec Codec { get; }

        public virtual TimeSpan AnimationDelay { get; } = TimeSpan.Zero;
        public virtual int AnimationIterations { get; } = 0;
        public virtual int FrameCount { get; } = 1;

        public virtual IEnumerable<IImage> GetFrames() {

            yield return this;

        }

        public abstract IImage Clone();

        public virtual Bitmap ToBitmap() {

            throw new NotSupportedException(string.Format(ExceptionMessages.TypeCannotBeConvertedToBitmap, GetType().Name));

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) { }

    }

}