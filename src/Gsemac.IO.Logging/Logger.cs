namespace Gsemac.IO.Logging {

    public static class Logger {

        // Public members

        public static ILogger Default => new ConsoleLogger();
        public static ILogger Null => new NullLogger();

    }

}