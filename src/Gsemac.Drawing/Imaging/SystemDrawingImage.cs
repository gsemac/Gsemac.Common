#if NETFRAMEWORK

using System.Drawing;

namespace Gsemac.Drawing {

    public class SystemDrawingImage :
        IImage {

        // Public members

        public SystemDrawingImage(Image image) {

            this.image = image;

        }

        public Image ToBitmap() {

            return image;

        }

        public void Dispose() {

            Dispose(disposing: true);

            System.GC.SuppressFinalize(this);

        }

        // Private members

        private readonly Image image;
        private bool disposedValue;

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    image.Dispose();

                }

                disposedValue = true;

            }
        }

    }

}

#endif