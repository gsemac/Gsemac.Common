namespace Gsemac.IO.Logging {

    public interface ILoggerFactory {

        ILogger Create();
        ILogger Create(string name);

    }

}