namespace Gsemac.Core.Extensions {

    public static class CmdArgumentsBuilderExtensions {

        public static ICmdArgumentsBuilder WithArgument(this ICmdArgumentsBuilder argumentsBuilder, string argument) {

            argumentsBuilder.AddArgument(argument);

            return argumentsBuilder;

        }
        public static ICmdArgumentsBuilder WithArgument(this ICmdArgumentsBuilder argumentsBuilder, string name, string value) {

            argumentsBuilder.AddArgument(name, value);

            return argumentsBuilder;

        }
        public static ICmdArgumentsBuilder WithArguments(this ICmdArgumentsBuilder argumentsBuilder, params string[] arguments) {

            foreach (string argument in arguments)
                argumentsBuilder.AddArgument(argument);

            return argumentsBuilder;

        }

    }

}