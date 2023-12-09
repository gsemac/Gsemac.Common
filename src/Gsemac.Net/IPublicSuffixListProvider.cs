using System;
using System.Collections.Generic;

namespace Gsemac.Net {

    public interface IPublicSuffixListProvider {

        TimeSpan TimeToLive { get; set; }
        bool FallbackEnabled { get; set; }

        IEnumerable<string> GetList();

    }

}