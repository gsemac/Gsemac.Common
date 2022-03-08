using System;
using System.Runtime.InteropServices;

namespace Gsemac.Drawing.Imaging.Native {

    [StructLayout(LayoutKind.Sequential)]
    internal struct WebPIterator {

        public int frame_num;
        /// <summary>
        /// Equivalent to <see cref="WebPFormatFeature.FrameCount"/>.
        /// </summary>
        public int num_frames;
        /// <summary>
        /// Offset relative to the canvas.
        /// </summary>
        public int x_offset;
        /// <summary>
        /// Offset relative to the canvas.
        /// </summary>
        public int y_offset;
        /// <summary>
        ///  Dimensions of this frame.
        /// </summary>
        public int width;
        /// <summary>
        ///  Dimensions of this frame.
        /// </summary>
        public int height;
        /// <summary>
        /// Display duration in milliseconds.
        /// </summary>
        public int duration;
        /// <summary>
        /// Dispose method for the frame.
        /// </summary>
        public WebPMuxAnimDispose dispose_method;
        /// <summary>
        ///  <see langword="true"/> if 'fragment' contains a full frame. partial images may still be decoded with the WebP incremental decoder.
        /// </summary>
        public int complete;
        /// <summary>
        /// The frame given by 'frame_num'. Note for historical reasons this is called a fragment.
        /// </summary>
        public WebPData fragment;
        /// <summary>
        /// <see langword="true"/> if the frame contains transparency.
        /// </summary>
        public int has_alpha;
        /// <summary>
        /// Blend operation for the frame.
        /// </summary>
        public WebPMuxAnimBlend blend_method;
        /// <summary>
        /// Padding for later use.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public readonly uint[] pad;
        /// <summary>
        /// For internal use only.
        /// </summary>
        public readonly IntPtr private_;

    }

}