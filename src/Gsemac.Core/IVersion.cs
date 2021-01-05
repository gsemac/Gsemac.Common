using System;
using System.Collections.Generic;

namespace Gsemac.Core {

    public interface IVersion :
        IComparable,
        IComparable<IVersion>,
        IEnumerable<int> {

        bool IsPreRelease { get; }

        int Major { get; }
        int Minor { get; }

    }

}