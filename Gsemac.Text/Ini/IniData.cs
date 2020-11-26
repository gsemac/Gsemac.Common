using Gsemac.Collections;
using Gsemac.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Text.Ini {

    public class IniData :
        IIniData {

        // Public members

        public IIniSection this[string key] {
            get => GetSection(key) ?? new IniSection(key, this);
            set => AddSection(value);
        }

        public IIniSection DefaultSection => this[string.Empty];

        public IniData() { }
        public IniData(IIniOptions options) {

            this.options = options;

        }

        public void AddSection(IIniSection section) {

            sections[GetKey(section.Name)] = section;

        }
        public IIniSection GetSection(string name) {

            if (sections.TryGetValue(GetKey(name), out IIniSection section))
                return section;

            return null;

        }
        public bool RemoveSection(string name) {

            return sections.Remove(GetKey(name));

        }

        public void Save(string filePath) {

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                Save(fs);

        }
        public void Save(Stream stream) {

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(ToString())))
                ms.CopyTo(stream);

        }

        public IEnumerator<IIniSection> GetEnumerator() {

            return sections.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            if (DefaultSection.Any())
                WriteSection(sb, DefaultSection);

            foreach (IIniSection section in this) {

                if (section == DefaultSection)
                    continue;

                WriteSection(sb, section);

            }

            return sb.ToString();

        }

        public static string Escape(string input) {

            string result = Regex.Replace(input, @"[\\'""\x00\a\b\t\r\n;#=:]",
                m => {

                    switch (m.Value) {

                        case "\0":
                            return @"\0";

                        case "\a":
                            return @"\a";

                        case "\b":
                            return @"\b";

                        case "\t":
                            return @"\t";

                        case "\r":
                            return @"\r";

                        case "\n":
                            return @"\n";

                        default:
                            return $@"\{m.Value}";

                    }

                }, RegexOptions.IgnoreCase);

            return result;

        }
        public static string Unescape(string input) {

            return StringUtilities.Unescape(input, UnescapeOptions.UnescapeEscapeSequences);

        }

        public static IniData Parse(string iniString) {

            return Parse(iniString, new IniOptions());

        }
        public static IniData Parse(string iniString, IIniOptions options) {

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(iniString)))
                return FromStream(ms);

        }
        public static IniData FromFile(string filePath) {

            return FromFile(filePath, new IniOptions());

        }
        public static IniData FromFile(string filePath, IIniOptions options) {

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
                return FromStream(fs);

        }
        public static IniData FromStream(Stream stream) {

            return FromStream(stream, new IniOptions());

        }
        public static IniData FromStream(Stream stream, IIniOptions options) {

            IniData result = new IniData(options);

            IIniSection lastSection = null;
            IIniProperty lastProperty = null;

            using (IIniLexer lexer = new IniLexer(stream)) {

                while (lexer.ReadNextToken(out IIniLexerToken token)) {

                    switch (token.Type) {

                        case IniLexerTokenType.SectionName:

                            lastSection = new IniSection(token.Value);

                            result.AddSection(lastSection);

                            break;

                        case IniLexerTokenType.PropertyName:

                            lastProperty = new IniProperty(token.Value, string.Empty);

                            (lastSection ?? result.DefaultSection).AddProperty(lastProperty);

                            break;

                        case IniLexerTokenType.PropertyValue:

                            if (!(lastProperty is null))
                                lastProperty.Value = token.Value;

                            break;

                    }

                }

            }

            return result;

        }

        // Private members

        private readonly IDictionary<string, IIniSection> sections = new OrderedDictionary<string, IIniSection>();
        private readonly IIniOptions options = new IniOptions();

        private string GetKey(string sectionName) {

            if (string.IsNullOrEmpty(sectionName))
                return sectionName;

            return sectionName.ToLowerInvariant().Trim();

        }
        private void WriteSection(StringBuilder sb, IIniSection section) {

            if (section != DefaultSection && !string.IsNullOrEmpty(section.Name))
                sb.AppendLine($"[{Escape(section.Name)}]");

            foreach (IIniProperty property in section)
                sb.AppendLine($"{Escape(property.Name)}={Escape(property.Value)}");

            sb.AppendLine();

        }

    }

}