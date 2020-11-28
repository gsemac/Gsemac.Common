namespace Gsemac.Core {

    public interface ICommandLineArgumentsBuilder {

        void AddArgument(string argumentValue);
        void AddArgument(string argumentName, string argumentValue);
        void Clear();

    }

}
