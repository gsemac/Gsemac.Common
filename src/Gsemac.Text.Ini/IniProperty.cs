using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public class IniProperty :
        IIniProperty {

        // Public members

        public string Name { get; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string Value {
            get => GetValue();
            set => SetValue(value);
        }
        public ICollection<string> Values { get; } = new List<string>();

        public IniProperty(string name, string value) {

            Name = name;
            Value = value;

        }

        public override string ToString() {

            return Value;

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