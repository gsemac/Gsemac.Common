namespace Gsemac.Core {

    public interface IProcessArgumentsBuilder<BuilderT> :
        ICommandLineArgumentsBuilder
        where BuilderT : class {

        new BuilderT WithArgument(string value);
        new BuilderT WithArgument(string name, string value);

    }

    public interface ICommandLineArgumentsBuilder {

        ICommandLineArgumentsBuilder WithArgument(string value);
        ICommandLineArgumentsBuilder WithArgument(string name, string value);

        void Clear();

    }

}