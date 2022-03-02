using Gsemac.Drawing.Imaging.Native;
using System;

namespace Gsemac.Drawing.Imaging {

    public class WebPFrame :
        IWebPFrame {

        // Public members

        /// <summary>
        /// The index of this frame.
        /// </summary>
        public int Index => iterator.frame_num - 1; // WebP frame indices are 1-based
        /// <summary>
        /// The horizontal offset relative to the canvas.
        /// </summary>
        public int XOffset => iterator.x_offset;
        /// <summary>
        /// The vertical offset relative to the canvas.
        /// </summary>
        public int YOffset => iterator.y_offset;
        /// <summary>
        /// The width of this frame.
        /// </summary>
        public int Width => iterator.width;
        /// <summary>
        /// The height of this frame.
        /// </summary>
        public int Height => iterator.height;
        /// <summary>
        /// The display duration of this frame.
        /// </summary>
        public TimeSpan Duration => TimeSpan.FromMilliseconds(iterator.duration);
        /// <summary>
        /// Returns <see cref="true"/> if the frame contains transparency.
        /// </summary>
        public bool HasAlpha { get; }

        // Internal members

        internal WebPFrame(WebPIterator iterator) {

            this.iterator = iterator;

        }

        // Private members

        private WebPIterator iterator;

    }

}