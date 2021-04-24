namespace Gsemac.Core {

    public sealed class NameValuePair {

        public string Name { get; }
        public string Value { get; }

        public NameValuePair(string name, string value) {

            this.Name = name;
            this.Value = value;

        }

    }

}