namespace Gsemac.Core {

    public interface ICommandLineArgumentsBuilder {

        ICommandLineArgumentsBuilder AddArgument(string argumentValue);
        ICommandLineArgumentsBuilder AddArgument(string argumentName, string argumentValue);

        void Clear();

    }

}
