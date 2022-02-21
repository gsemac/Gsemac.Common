using System;

namespace Gsemac.Drawing.Imaging {

    public interface IAnimationInfo {

        TimeSpan Delay { get; }
        int Iterations { get; }
        int FrameCount { get; }

    }

}