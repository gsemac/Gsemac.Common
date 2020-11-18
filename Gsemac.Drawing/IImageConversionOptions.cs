using System.Collections.Generic;

namespace Gsemac.Drawing {

    public interface IImageConversionOptions {

        float Quality { get; set; }

#if NETFRAMEWORK
        ICollection<IImageFilter> Filters { get; }
#endif

    }

}