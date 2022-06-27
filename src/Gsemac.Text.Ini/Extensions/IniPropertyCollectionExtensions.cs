using System;

namespace Gsemac.Text.Ini.Extensions {

    public static class IniPropertyCollectionExtensions {

        // Public members

        public static string GetValue(this IIniPropertyCollection collection, string name) {

            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            return collection.Get(name)?.Value ?? String.Empty;

        }
        public static void SetValue(this IIniPropertyCollection collection, string name, string value) {

            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            collection.Set(name, value);

        }

    }

}