using System;
using System.Collections.Generic;

namespace Gsemac.Net.GitHub {

    public interface IRelease {

        string Url { get; }
        string Tag { get; }
        string Title { get; }
        string Description { get; }
        DateTimeOffset Published { get; }

        IEnumerable<IReleaseAsset> Assets { get; }

    }

}