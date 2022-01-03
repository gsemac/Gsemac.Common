namespace Gsemac.Net.JavaScript {

    public interface IJSConsole {

        void Log(params object[] objects);
        void Log(string message, params object[] objects);

    }

}