#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using Gsemac.Drawing.Imaging;
using Gsemac.Drawing.Imaging.Extensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Gsemac.Drawing {

    public class BitmapDataStream :
        Stream {

        // Public members

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => bitmapData.Stride * bitmap.Height;
        public override long Position {
            get => position;
            set => throw new NotSupportedException();
        }

        public BitmapDataStream(Bitmap bitmap) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (ImageUtilities.HasIndexedPixelFormat(bitmap)) {

                this.bitmap = (Bitmap)ImageUtilities.ConvertToNonIndexedPixelFormat(bitmap, disposeSourceImage: false);
                this.disposeBitmap = true;

            }
            else {

                this.bitmap = bitmap;

            }

            this.bitmapData = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), ImageLockMode.ReadOnly, this.bitmap.PixelFormat);

        }

        public override int Read(byte[] buffer, int offset, int count) {

            if (position >= Length)
                return 0;

            if (position + count > Length)
                count = (int)(Length - position);

            IntPtr ptr = IntPtr.Add(bitmapData.Scan0, (int)position);

            Marshal.Copy(ptr, buffer, offset, count);

            position = Math.Min(position + count, Length);

            return count;

        }
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Close() {

            // Unlock the bitmap.

            bitmap.UnlockBits(bitmapData);

            base.Close();

        }
        public override void Flush() { }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing) {

                if (disposeBitmap)
                    bitmap.Dispose();

            }

            base.Dispose(disposing);

        }

        // Private members

        private readonly Bitmap bitmap;
        private readonly BitmapData bitmapData;
        private readonly bool disposeBitmap = false;
        private long position = 0;

    }

}

#endif