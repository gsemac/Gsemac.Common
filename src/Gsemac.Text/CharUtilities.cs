namespace Gsemac.Text {

    public static class CharUtilities {

        public static bool IsNewLine(char c) {

            return c == '\r' || c == '\n';

        }
        public static bool IsTerminalPunctuation(char value) {

            switch (value) {

                case '.':
                case '!':
                case '?':
                    return true;

                default:
                    return false;

            }

        }

    }

}