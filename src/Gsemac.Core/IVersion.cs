using System;

namespace Gsemac.Core {

    public interface IVersion :
        IComparable,
        IComparable<IVersion> {

        bool IsPreRelease { get; }

        int Major { get; }
        int Minor { get; }

    }

}