using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Core {

    public abstract class ProcessArgumentsBuilderBase<TBuilder> :
        IProcessArgumentsBuilder<TBuilder> where TBuilder : class {

        // Public members

        public TBuilder WithArgument(string value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!string.IsNullOrWhiteSpace(value))
                arguments.Add(EscapeArgumentValue(value));

            return this as TBuilder;

        }
        public TBuilder WithArgument(string name, string value) {

            name = name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (!name.StartsWith("-")) {

                if (name.Length == 1)
                    name = $"-{name}";
                else
                    name = $"--{name}";

            }

            arguments.Add($"{EscapeArgumentValue(name)} {EscapeArgumentValue(value)}");

            return this as TBuilder;

        }

        public void Clear() {

            arguments.Clear();

        }

        public override string ToString() {

            return string.Join(" ", arguments);

        }

        ICommandLineArgumentsBuilder ICommandLineArgumentsBuilder.WithArgument(string value) {

            WithArgument(value);

            return this;

        }
        ICommandLineArgumentsBuilder ICommandLineArgumentsBuilder.WithArgument(string name, string value) {

            WithArgument(name, value);

            return this;

        }

        // Protected members

        protected ProcessArgumentsBuilderBase() {

            // Make sure that can actually cast the current object to BuilderT.

            if (this as TBuilder is null)
                throw new InvalidCastException(string.Format(Properties.ExceptionMessages.CannotCastTypeToTypeWithTypeAndType, GetType(), typeof(TBuilder)));

        }

        // Private members

        private readonly List<string> arguments = new List<string>();

        private bool IsReservedCharacter(char c) {

            return "^&|\\\"".Any(reservedChar => reservedChar.Equals(c));

        }
        private string EscapeArgumentValue(string value) {

            bool containsQuotes = value.Contains("\"");
            bool requiresQuotes = value.Any(c => char.IsWhiteSpace(c) || IsReservedCharacter(c));

            if (containsQuotes)
                value = Regex.Replace(value, @"[""]", "\"\"");

            if (requiresQuotes)
                value = $"\"{value}\"";

            return value;

        }

    }

}