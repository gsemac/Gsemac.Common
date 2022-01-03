namespace Gsemac.Net.JavaScript {

    public interface IJSWindow {

        IJSConsole Console { get; }

        string Atob(string encodedData);
        string Btoa(string stringToEncode);

    }

}