using Gsemac.IO.Logging;
using Gsemac.Net.WebBrowsers;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverUpdater :
        ILogEventSource {

        event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;
        event DownloadFileCompletedEventHandler DownloadFileCompleted;

        IWebDriverInfo Update(IWebBrowserInfo webBrowserInfo);

    }

}