using System;

namespace Gsemac.Reflection {

    public sealed class PropertyDictionaryOptions :
        IPropertyDictionaryOptions {

        // Public members

        public bool SkipReadOnlyProperties { get; set; } = false;
        public bool IncludeNestedProperties { get; set; } = false;
        public StringComparer StringComparer { get; } = StringComparer.Ordinal;

        public static PropertyDictionaryOptions Default => new PropertyDictionaryOptions();

    }

}