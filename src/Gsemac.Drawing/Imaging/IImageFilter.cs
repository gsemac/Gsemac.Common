#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gsemac.Drawing.Imaging {

    public interface IImageFilter {

        Image Apply(Image sourceImage);

    }

}

#endif