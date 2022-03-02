#if NETFRAMEWORK

using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gsemac.Drawing.Imaging {

    internal class WebPImage :
        ImageBase {

        // Public members

        public override IAnimationInfo Animation { get; }
        public override int Width => image.Width;
        public override int Height => image.Height;
        public override IFileFormat Format => image.Format;
        public override IImageCodec Codec => image.Codec;

        public override IImage Clone() {

            return image.Clone();

        }
        public override Bitmap ToBitmap() {

            return image.ToBitmap();

        }

        public WebPImage(IImage image, IAnimationInfo animationInfo) {

            Animation = animationInfo;
            this.image = image;

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing) {

                image.Dispose();

            }

            base.Dispose(disposing);

        }

        // Private members

        private readonly IImage image;

    }

}

#endif