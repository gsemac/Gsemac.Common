namespace Gsemac.Logging {

    public class KeepAllLogRetentionPolicy :
        ILogRetentionPolicy {

        public void ExecutePolicy(string directoryPath, string searchPattern = "*") {

            // Do nothing; all log files are preserved.

        }

    }

}