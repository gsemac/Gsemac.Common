using System.Net;

namespace Gsemac.Net.WebBrowsers {

    public interface IWebBrowserProfile {

        string Name { get; }
        bool IsDefault { get; }
        string DirectoryPath { get; }

        CookieContainer GetCookies();

    }

}