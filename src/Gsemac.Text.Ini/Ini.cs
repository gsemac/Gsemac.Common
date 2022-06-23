using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Text.Ini {

    public class Ini :
        IIni {

        // Public members

        public IIniSection this[string sectionName] {
            get => Sections[sectionName];
        }

        public IIniSection Global => global;
        public IIniSectionCollection Sections => sections;

        public Ini() :
            this(IniOptions.Default) {
        }
        public Ini(IIniOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        public IEnumerator<IIniSection> GetEnumerator() {

            return sections.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            if (Global.Any()) {

                WriteSection(sb, Global);

            }


            foreach (IIniSection section in this) {

                sb.AppendLine();

                if (section == Global)
                    continue;

                WriteSection(sb, section);

            }

            return sb.ToString();

        }

        // Private members

        private readonly IIniSection global = new IniSection();
        private readonly IIniSectionCollection sections = new IniSectionCollection();
        private readonly IIniOptions options;

        private string GetKey(string sectionName) {

            if (string.IsNullOrEmpty(sectionName))
                return sectionName;

            return sectionName.ToLowerInvariant().Trim();

        }
        private void WriteSection(StringBuilder sb, IIniSection section) {

            if (section != Global && !string.IsNullOrEmpty(section.Name))
                sb.AppendLine($"[{IniUtilities.Escape(section.Name)}]");

            foreach (IIniProperty property in section) {

                if (options.AllowComments && !string.IsNullOrEmpty(options.CommentMarker) && !string.IsNullOrEmpty(property.Comment)) {

                    sb.Append(options.CommentMarker);
                    sb.Append(' ');
                    sb.AppendLine(property.Comment);

                }

                sb.AppendLine($"{IniUtilities.Escape(property.Name)}={IniUtilities.Escape(property.Value)}");

            }

        }

    }

}