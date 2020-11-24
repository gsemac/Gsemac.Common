namespace Gsemac.IO.Logging {

    public interface ILogRetentionPolicy {

        void ExecutePolicy(string directoryPath, string searchPattern = "*");

    }

}