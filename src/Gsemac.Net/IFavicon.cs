using Gsemac.Drawing.Imaging;
using System;

namespace Gsemac.Net {

    public interface IFavicon :
        IDisposable {

        Uri Uri { get; }
        string Name { get; }
        IImage Icon { get; }

    }

}
