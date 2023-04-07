﻿using System.Collections.Generic;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    public interface IBrowserProfile {

        string Name { get; }
        bool IsDefaultProfile { get; }
        string DirectoryPath { get; }

        IEnumerable<Cookie> GetCookies();

    }

}