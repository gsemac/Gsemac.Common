using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Net.WebDrivers {
    
    public interface IWebDriverInfoCache {

        IWebDriverInfo GetWebDriverInfo(string webDriverFilePath);

    }

}
