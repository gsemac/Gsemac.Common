using System;

namespace Gsemac.Drawing.Imaging {

    public class AnimationInfo :
        IAnimationInfo {

        // Public members

        public TimeSpan Delay { get; }
        public int Iterations { get; }
        public int FrameCount { get; }

        public static AnimationInfo Static => new AnimationInfo(1, 0, TimeSpan.Zero);

        public AnimationInfo(int frameCount, int iterations, TimeSpan delay) {

            FrameCount = frameCount;
            Iterations = iterations;
            Delay = delay;

        }

    }

}