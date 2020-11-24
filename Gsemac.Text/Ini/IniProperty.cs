namespace Gsemac.Text.Ini {

    public class IniProperty {

        // Public members

        public string Name { get; }
        public string Value { get; }

        public IniProperty(string name, string value) {

            Name = name;
            Value = value;

        }

    }

}