namespace Gsemac.Logging {

    public class NeverDeleteLogRetentionPolicy :
        ILogRetentionPolicy {

        public void ExecutePolicy(string directoryPath, string searchPattern = "*") {

            // Do nothing; all log files are preserved.

        }

    }

}