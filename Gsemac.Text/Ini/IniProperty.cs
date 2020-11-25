namespace Gsemac.Text.Ini {

    public class IniProperty :
        IIniProperty {

        // Public members

        public string Name { get; }
        public string Value { get; set; }

        public IniProperty(string name, string value) {

            Name = name;
            Value = value;

        }

    }

}