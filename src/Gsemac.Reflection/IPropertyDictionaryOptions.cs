using System;

namespace Gsemac.Reflection {

    public interface IPropertyDictionaryOptions {

        bool SkipReadOnlyProperties { get; }
        bool IncludeNestedProperties { get; }
        StringComparer StringComparer { get; }

    }

}