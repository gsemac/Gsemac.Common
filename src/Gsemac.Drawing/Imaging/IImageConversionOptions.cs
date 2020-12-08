using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public interface IImageConversionOptions {

        IImageEncoderOptions EncoderOptions { get; }
        ICollection<IImageFilter> Filters { get; }

    }

}