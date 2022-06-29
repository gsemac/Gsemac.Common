using System;

namespace Gsemac.Text.Ini {

    public class Ini :
        IniBase {

        // Public members

        public override IIniSection Global => global;
        public override IIniSectionCollection Sections => sections;

        public Ini() :
            this(IniOptions.Default) {
        }
        public Ini(IIniOptions options) :
            base(options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            global = new IniSection(options.KeyComparer);
            sections = new IniSectionCollection(options.KeyComparer);

        }

        // Private members

        private readonly IIniSection global;
        private readonly IIniSectionCollection sections;

    }

}