namespace Gsemac.IO.Logging {

    public class PreserveLogRetentionPolicy :
        ILogRetentionPolicy {

        public void ExecutePolicy(string directoryPath, string searchPattern) {

            // Do nothing; all log files are preserved.

        }

    }

}