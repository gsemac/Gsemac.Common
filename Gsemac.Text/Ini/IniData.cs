using Gsemac.Collections;
using Gsemac.Core;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gsemac.Text.Ini {

    public class IniData :
        IIniData {

        // Public members

        public IIniSection this[string key] {
            get => GetSection(key) ?? new IniSection(string.Empty);
            set => AddSection(value);
        }

        public void AddSection(IIniSection section) {

            sections[section.Name.ToLowerInvariant()] = section;

        }
        public IIniSection GetSection(string name) {

            if (sections.TryGetValue(name.ToLowerInvariant(), out IIniSection section))
                return section;

            return null;

        }
        public bool RemoveSection(string name) {

            return sections.Remove(name.ToLowerInvariant());

        }

        public IEnumerator<IIniSection> GetEnumerator() {

            return sections.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            return base.ToString();

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

        private readonly IDictionary<string, IIniSection> sections = new OrderedDictionary<string, IIniSection>();

    }

}