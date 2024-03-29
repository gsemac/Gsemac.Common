﻿using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public class IniProperty :
        IIniProperty {

        // Public members

        public string Comment { get; set; } = string.Empty;
        public string Name { get; } = string.Empty;
        public string Value {
            get => GetValue();
            set => SetValue(value);
        }
        public ICollection<string> Values { get; } = new List<string>();

        public IniProperty(string name) :
            this(name, null) {
        }
        public IniProperty(string name, string value) {

            Name = name;

            if (!string.IsNullOrEmpty(value))
                Value = value;

        }

        public override string ToString() {

            using (IIniWriter iniWriter = new IniWriter()) {

                iniWriter.WritePropertyName(Name);
                iniWriter.WriteNameValueSeparator();
                iniWriter.WritePropertyValue(Value);

                return iniWriter.ToString();

            }

        }

        // Private members

        private string GetValue() {

            return string.Join(", ", Values);

        }
        private void SetValue(string value) {

            Values.Clear();

            Values.Add(value);

        }

    }

}