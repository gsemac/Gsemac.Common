using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

            global = new IniSection(options.KeyComparer);
            sections = new IniSectionCollection(options.KeyComparer);

        }

        public IEnumerator<IIniSection> GetEnumerator() {

            return sections.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            using (IIniWriter writer = new IniWriter(options)) {

                if (Global.Any()) {

                    WriteSection(writer, Global);

                }

                foreach (IIniSection section in this) {

                    if (section == Global)
                        continue;

                    WriteSection(writer, section);

                }

                return writer.ToString();

            }

        }

        // Private members

        private readonly IIniSection global;
        private readonly IIniSectionCollection sections;
        private readonly IIniOptions options;

        private void WriteSection(IIniWriter writer, IIniSection section) {

            if (section != Global && !string.IsNullOrEmpty(section.Name)) {

                writer.WriteSectionStart();
                writer.WriteSectionName(section.Name);
                writer.WriteSectionEnd();

            }

            foreach (IIniProperty property in section) {

                //if (options.AllowComments && !string.IsNullOrEmpty(options.CommentMarker) && !string.IsNullOrEmpty(property.Comment)) {

                //    sb.Append(options.CommentMarker);
                //    sb.Append(' ');
                //    sb.AppendLine(property.Comment);

                //}

                writer.WritePropertyName(property.Name);
                writer.WriteNameValueSeparator();
                writer.WritePropertyValue(property.Value);

            }

        }

    }

}