namespace Gsemac.Core {

    public sealed class NameValuePair :
        INameValuePair {

        // Public members

        public string Name { get; }
        public string Value { get; }

        public NameValuePair(string name, string value) {

            Name = name;
            Value = value;

        }

    }

}