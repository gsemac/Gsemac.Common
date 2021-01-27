using System.Collections.Generic;

namespace Gsemac.Net.GitHub {

    public interface IGitHubClient {

        IRepository GetRepository(IRepositoryUrl repositoryUrl);
        IEnumerable<IRelease> GetReleases(IRepositoryUrl repositoryUrl);

    }

}