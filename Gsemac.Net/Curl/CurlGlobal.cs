﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Net.Curl {

    [Flags]
    public enum CurlGlobal {
        All = Ssl | Win32,
        Ssl = 1,
        Win32 = 2,
        Nothing = 0,
        Default = All,
        AckEintr = 4
    }

}