using Gsemac.Collections;
using Gsemac.Core;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gsemac.Text.Ini {

    public class IniData :
        IEnumerable<IniSection> {

        // Public members

        public IniSection this[string key] {
            get => GetSection(key) ?? new IniSection(string.Empty);
            set => AddSection(value);
        }

        public void AddSection(IniSection section) {

            sections[section.Name.ToLowerInvariant()] = section;

        }
        public IniSection GetSection(string name) {

            if (sections.TryGetValue(name.ToLowerInvariant(), out IniSection section))
                return section;

            return null;

        }
        public bool RemoveSection(string name) {

            return sections.Remove(name.ToLowerInvariant());

        }

        public IEnumerator<IniSection> GetEnumerator() {

            return sections.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public static string Escape(string input) {

            string result = Regex.Replace(input, @"[\\'""\x00\a\b\t\r\n;#=:]",
                m => $@"\{m.Value}", RegexOptions.IgnoreCase);

            return result;

        }
        public static string Unescape(string input) {

            string result = Regex.Replace(input, @"\\(?:x[0-9a-z]{2,4}|.)",
                m => StringUtilities.Unescape(m.Value, UnescapeOptions.UnescapeEscapeSequences), RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return result;

        }

        // Private members

        private readonly IDictionary<string, IniSection> sections = new OrderedDictionary<string, IniSection>();

    }

}