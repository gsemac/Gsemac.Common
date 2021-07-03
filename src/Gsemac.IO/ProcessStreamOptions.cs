namespace Gsemac.IO {

    public class ProcessStreamOptions :
        IProcessStreamOptions {

        // Public members

        public bool RedirectStandardOutput { get; set; } = true;
        public bool RedirectStandardInput { get; set; } = false;
        public bool RedirectStandardError { get; set; } = false;
        public bool RedirectStandardErrorToStandardOutput {
            get => redirectStandardErrorToStandardOutput;
            set {

                redirectStandardErrorToStandardOutput = true;

                RedirectStandardError = true;

            }
        }
        public bool ExitOnUserInputPrompt { get; set; } = false;

        public static ProcessStreamOptions Default => new ProcessStreamOptions();

        public ProcessStreamOptions(bool redirectStandardOutput = true, bool redirectStandardInput = true, bool redirectStandardError = true) {

            RedirectStandardOutput = redirectStandardOutput;
            RedirectStandardInput = redirectStandardInput;
            RedirectStandardError = redirectStandardError;

        }

        // Private members

        private bool redirectStandardErrorToStandardOutput = false;

    }

}