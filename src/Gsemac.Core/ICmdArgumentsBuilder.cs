namespace Gsemac.Core {

    public interface ICmdArgumentsBuilder {

        void AddArgument(string value);
        void AddArgument(string name, string value);

        void Clear();

    }

}