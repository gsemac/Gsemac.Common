using Gsemac.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Text.Ini {

    public class IniSectionCollection :
        IIniSectionCollection {

        // Public members

        public int Count => GetNonTransientSections().Count();
        public bool IsReadOnly => sections.IsReadOnly;

        public IIniSection this[string sectionName] {
            get => GetOrDefault(sectionName);
        }

        public IniSectionCollection() :
            this(EqualityComparer<string>.Default) {
        }
        public IniSectionCollection(IEqualityComparer<string> keyComparer) {

            if (keyComparer is null)
                throw new ArgumentNullException(nameof(keyComparer));

            sections = new Dictionary<string, IniSectionInfo>(keyComparer);
            this.keyComparer = keyComparer;

        }

        public void Add(string name) {

            Add(new IniSection(name, keyComparer));

        }
        public void Add(IIniSection item) {

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            // If we try to add a section that already exists, just merge the properties.

            if (sections.TryGetValue(item.Name, out IniSectionInfo existingSection)) {

                existingSection.Section.Properties.AddRange(item.Properties);

            }
            else {

                sections.Add(item.Name, new IniSectionInfo(item));

            }

        }

        public void Clear() {

            sections.Clear();

        }

        public bool Contains(string name) {

            return sections.TryGetValue(name, out IniSectionInfo info) &&
                !info.IsTransient;

        }
        public bool Contains(IIniSection item) {

            if (item is null)
                return false;

            return sections.TryGetValue(item.Name, out IniSectionInfo info) &&
                !info.IsTransient &&
                info.Section.Equals(item);

        }

        public void CopyTo(IIniSection[] array, int arrayIndex) {

            this.ToList().CopyTo(array, arrayIndex);

        }

        public IIniSection Get(string name) {

            if (sections.TryGetValue(name, out IniSectionInfo info) && !info.IsTransient)
                return info.Section;

            return null;

        }

        public bool Remove(string name) {

            return sections.Remove(name);

        }
        public bool Remove(IIniSection item) {

            if (item is null)
                return false;

            // If we have an identical section to the one given, remove it.

            if (Contains(item))
                return sections.Remove(item.Name);

            return false;

        }

        public IEnumerator<IIniSection> GetEnumerator() {

            return GetNonTransientSections()
                .GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private class IniSectionInfo {

            // Public members

            public IIniSection Section { get; }
            public bool IsTransient => isTransient && !Section.Any();

            public IniSectionInfo(IIniSection section) :
                this(section, isTransient: false) {
            }
            public IniSectionInfo(IIniSection section, bool isTransient) {

                Section = section;

                this.isTransient = isTransient;

            }

            // Private members

            private readonly bool isTransient;

        }

        private readonly IDictionary<string, IniSectionInfo> sections;
        private readonly IEqualityComparer<string> keyComparer;

        private IIniSection GetOrDefault(string sectionName) {

            if (sections.TryGetValue(sectionName, out IniSectionInfo info))
                return info.Section;

            info = new IniSectionInfo(new IniSection(sectionName), isTransient: true);

            sections.Add(sectionName, info);

            return info.Section;

        }
        private IEnumerable<IIniSection> GetNonTransientSections() {

            return sections.Values
              .Where(info => !info.IsTransient)
              .Select(info => info.Section);

        }

    }

}