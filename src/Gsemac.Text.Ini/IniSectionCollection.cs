using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Text.Ini {

    public class IniSectionCollection :
        IIniSectionCollection {

        // Public members

        public int Count => sections.Count;
        public bool IsReadOnly => sections.IsReadOnly;

        public IIniSection this[string sectionName] {
            get => GetOrDefault(sectionName);
        }

        public void Add(string sectionName) {

            Add(new IniSection(sectionName));

        }
        public void Add(IIniSection item) {

            if (item is null)
                throw new ArgumentNullException(nameof(item));

            sections.Add(item.Name, new IniSectionInfo(item));

        }

        public void Clear() {

            sections.Clear();

        }

        public bool Contains(string sectionName) {

            return sections.TryGetValue(sectionName, out IniSectionInfo info) &&
                !info.IsTransient;

        }
        public bool Contains(IIniSection item) {

            if (item is null)
                return false;

            return sections.TryGetValue(item.Name, out IniSectionInfo info) &&
                info.Section.Equals(item);

        }

        public void CopyTo(IIniSection[] array, int arrayIndex) {

            this.ToList().CopyTo(array, arrayIndex);

        }

        public IIniSection Get(string sectionName) {

            if (sections.TryGetValue(sectionName, out IniSectionInfo info) && !info.IsTransient)
                return info.Section;

            return null;

        }

        public bool Remove(string sectionName) {

            return sections.Remove(sectionName);

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

            return sections.Values
                .Where(info => !info.IsTransient)
                .Select(info => info.Section)
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

        private readonly IDictionary<string, IniSectionInfo> sections = new Dictionary<string, IniSectionInfo>();

        private IIniSection GetOrDefault(string sectionName) {

            if (sections.TryGetValue(sectionName, out IniSectionInfo info))
                return info.Section;

            info = new IniSectionInfo(new IniSection(sectionName), isTransient: true);

            sections.Add(sectionName, info);

            return info.Section;

        }

    }

}