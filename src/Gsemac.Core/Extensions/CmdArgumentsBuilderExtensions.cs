using System;

namespace Gsemac.Core.Extensions {

    public static class CmdArgumentsBuilderExtensions {

        public static TBuilder WithArguments<TBuilder>(this ICommandLineArgumentsBuilder<TBuilder> argumentsBuilder, params string[] arguments)
            where TBuilder : class {

            foreach (string argument in arguments)
                argumentsBuilder.WithArgument(argument);

            if (argumentsBuilder as TBuilder is null)
                throw new InvalidCastException(string.Format(Properties.ExceptionMessages.CannotCastTypeToTypeWithTypeAndType, argumentsBuilder.GetType(), typeof(TBuilder)));

            return argumentsBuilder as TBuilder;

        }

    }

}