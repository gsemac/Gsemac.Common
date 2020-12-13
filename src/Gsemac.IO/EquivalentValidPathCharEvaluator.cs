using Gsemac.Text;
using System;

namespace Gsemac.IO {

    public class EquivalentValidPathCharEvaluator :
         ICharReplacementEvaluator {

        // Public members

        public string GetReplacement(char inputChar) {

            switch (inputChar) {

                case '\0':
                case '\u0001':
                case '\u0002':
                case '\u0003':
                case '\u0004':
                case '\u0005':
                case '\u0006':
                case '\a':
                case '\b':
                case '\n':
                case '\v':
                case '\f':
                case '\r':
                case '\u000e':
                case '\u000f':
                case '\u0010':
                case '\u0011':
                case '\u0012':
                case '\u0013':
                case '\u0014':
                case '\u0015':
                case '\u0016':
                case '\u0017':
                case '\u0018':
                case '\u0019':
                case '\u001a':
                case '\u001b':
                case '\u001c':
                case '\u001d':
                case '\u001e':
                case '\u001f':
                    return string.Empty;

                case '\t':
                    return " ";

                case '"':

                    inQuotes = !inQuotes;

                    return !inQuotes ? "”" : "“";

                case '*':
                    return "＊";

                case '/':
                    return "⁄";

                case ':':
                    return "∶";

                case '<':
                    return "＜";

                case '>':
                    return "＞";

                case '?':
                    return "？";

                case '\\':
                    return "＼";

                case '|':
                    return "｜";

                default:
                    return inputChar.ToString();

            }

        }

        // Private members

        private bool inQuotes = false;

    }

}