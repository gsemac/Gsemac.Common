namespace Gsemac.IO.Logging {

    public interface ILogFileNameFormatter {

        string Name { get; }
        string FileExtension { get; }

        string GetFileName();

    }

}