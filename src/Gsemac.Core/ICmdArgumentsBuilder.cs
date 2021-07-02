namespace Gsemac.Core {

    public interface ICmdArgumentsBuilder<BuilderT> :
        ICmdArgumentsBuilder
        where BuilderT : class {

        new BuilderT WithArgument(string value);
        new BuilderT WithArgument(string name, string value);

    }

    public interface ICmdArgumentsBuilder {

        ICmdArgumentsBuilder WithArgument(string value);
        ICmdArgumentsBuilder WithArgument(string name, string value);

        void Clear();

    }

}