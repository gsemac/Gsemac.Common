using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.GitHub {

    internal class Release :
        IRelease {

        public string Url { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreationTime { get; set; }

        public IEnumerable<IReleaseAsset> Assets { get; set; } = Enumerable.Empty<IReleaseAsset>();

    }

}