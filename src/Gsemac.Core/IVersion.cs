using System;
using System.Collections.Generic;

namespace Gsemac.Core {

    public interface IVersion :
        IComparable,
        IComparable<IVersion> {

        bool IsPreRelease { get; }
        IEnumerable<int> RevisionNumbers { get; }

        int Major { get; }
        int Minor { get; }

    }

}