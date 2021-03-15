using System;

namespace Gsemac.Net.GitHub {

    public interface IFileNode {

        string Url { get; }
        string CommitHash { get; }
        string CommitMessage { get; }
        bool IsDirectory { get; }
        DateTimeOffset LastModified { get; }
        string Name { get; }
        string Path { get; }

        string RawUrl { get; }

    }

}