using Gsemac.Core;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverFactory :
        IFactory<IWebDriver>,
        IDisposable {

        event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;
        event DownloadFileCompletedEventHandler DownloadFileCompleted;

        IWebDriver Create(IWebBrowserInfo webBrowserInfo);

    }

}