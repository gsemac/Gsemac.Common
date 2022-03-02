using System;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public interface IWebPDemuxer :
        IDisposable {

        IWebPFrame GetFrame(int frameIndex);
        IEnumerable<IWebPFrame> GetFrames();

        int GetI(WebPFormatFeature feature);

    }

}