using System.Collections.Generic;
using System.IO;

namespace Gsemac.Net.GitHub {

    public interface IGitHubClient {

        IRepository GetRepository(string url);
        IEnumerable<IRelease> GetReleases(string url);
        IEnumerable<IFileNode> GetFiles(string url, SearchOption searchOption = SearchOption.TopDirectoryOnly);

    }

}