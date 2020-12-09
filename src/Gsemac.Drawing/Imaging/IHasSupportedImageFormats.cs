using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public interface IHasSupportedImageFormats {

        IEnumerable<IImageFormat> SupportedImageFormats { get; }

    }

}