using System;

namespace Gsemac.Drawing.Imaging {

    public interface IWebPFrame {

        TimeSpan Duration { get; }
        bool HasAlpha { get; }
        int Height { get; }
        int Index { get; }
        int Width { get; }
        int XOffset { get; }
        int YOffset { get; }

    }

}