using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public interface IImageConversionOptions {

        IImageEncoderOptions EncoderOptions { get; }

#if NETFRAMEWORK
        ICollection<IImageFilter> Filters { get; }
#endif

    }

}