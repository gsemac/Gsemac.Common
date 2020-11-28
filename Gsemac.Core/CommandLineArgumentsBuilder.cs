using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Core {

    public class CommandLineArgumentsBuilder :
        ICommandLineArgumentsBuilder {

        // Public members

        public void AddArgument(string argumentValue) {

            arguments.Add(EscapeArgumentValue(argumentValue));

        }
        public void AddArgument(string argumentName, string argumentValue) {

            if (!argumentName.StartsWith("-")) {

                if (argumentName.Length == 1)
                    argumentName = $"-{argumentName}";
                else
                    argumentName = $"--{argumentName}";

            }

            arguments.Add($"{EscapeArgumentValue(argumentName)} {EscapeArgumentValue(argumentValue)}");

        }
        public void Clear() {

            arguments.Clear();

        }

        public override string ToString() {

            return string.Join(" ", arguments);

        }

        // Private members

        private readonly List<string> arguments = new List<string>();

        private string EscapeArgumentValue(string argumentValue) {

            bool containsQuotes = argumentValue.Contains("\"");
            bool containsWhitespace = argumentValue.Any(c => char.IsWhiteSpace(c));

            if (containsQuotes)
                argumentValue = Regex.Replace(argumentValue, @"[""]", "\"\"");

            if (containsQuotes || containsWhitespace)
                argumentValue = $"\"{argumentValue}\"";

            return argumentValue;

        }

    }

}