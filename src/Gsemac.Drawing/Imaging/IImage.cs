using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public interface IImage :
        IDisposable {

#if NETFRAMEWORK
        Image ToBitmap();
#endif

    }

}