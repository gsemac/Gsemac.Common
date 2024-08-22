using System;

namespace Gsemac.Net {

    public interface IPublicSuffixListProvider {

        TimeSpan TimeToLive { get; set; }
        bool FallbackEnabled { get; set; }

        IPublicSuffixList GetList();

    }

}