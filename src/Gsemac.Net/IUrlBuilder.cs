namespace Gsemac.Net {

    public interface IUrlBuilder {

        IUrlBuilder WithScheme(string scheme);
        IUrlBuilder WithUserName(string username);
        IUrlBuilder WithPassword(string password);
        IUrlBuilder WithHostname(string hostname);
        IUrlBuilder WithPort(int port);
        IUrlBuilder WithPath(string path);
        IUrlBuilder WithFragment(string fragment);
        IUrlBuilder WithQueryParameter(string name, string value);

        IUrl Build();

    }

}