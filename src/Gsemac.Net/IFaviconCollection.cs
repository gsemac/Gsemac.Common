using Gsemac.Drawing;
using System;
using System.Collections.Generic;

namespace Gsemac.Net {

    public interface IFaviconCollection :
        ICollection<IFavicon>,
        IDisposable {

        IImage DefaultIcon { get; }

        IFavicon GetFavicon(Uri uri);
        IFavicon GetFavicon(string name);

        IFavicon AddFromFile(string filePath);
        IFavicon AddFromHtmlDocument(string htmlDocument);
        IFavicon AddFromHtmlDocument(Uri uri, string htmlDocument);
        IFavicon AddFromUri(Uri uri);
        IFavicon AddFromUri(Uri uri, string name);

    }

}