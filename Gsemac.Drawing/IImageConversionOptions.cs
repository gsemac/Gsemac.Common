using System.Collections.Generic;

namespace Gsemac.Drawing {

    public interface IImageConversionOptions {

        IImageEncoderOptions EncoderOptions { get; }

#if NETFRAMEWORK
        ICollection<IImageFilter> Filters { get; }
#endif

    }

}