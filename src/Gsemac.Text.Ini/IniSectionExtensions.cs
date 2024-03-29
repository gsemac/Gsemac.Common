﻿using System;

namespace Gsemac.Text.Ini {

    public static class IniSectionExtensions {

        // Public members

        public static bool ContainsKey(this IIniSection section, string propertyName) {

            if (section is null)
                throw new ArgumentNullException(nameof(section));

            return section.Properties.ContainsKey(propertyName);

        }

        public static void Merge(this IIniSection section, IIniSection other) {

            if (section is null)
                throw new ArgumentNullException(nameof(section));

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            foreach (IIniProperty property in other.Properties)
                section.Properties.Add(property);

        }

    }

}