using System.IO;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverInfoCache {

        IWebDriverInfo GetWebDriverInfo(string webDriverFilePath);
        void AddWebDriverInfo(IWebDriverInfo webDriverInfo);

        void SaveTo(Stream stream);

    }

}