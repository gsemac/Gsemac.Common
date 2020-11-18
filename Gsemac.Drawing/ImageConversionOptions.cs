using System.Collections.Generic;

namespace Gsemac.Drawing {

    public class ImageConversionOptions :
        IImageConversionOptions {

        public float Quality { get; set; } = 1.0f;

#if NETFRAMEWORK
        public ICollection<IImageFilter> Filters { get; } = new List<IImageFilter>();
#endif

    }

}