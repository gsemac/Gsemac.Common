namespace Gsemac.Net.WebBrowsers {

    public interface ICookiesReaderFactory {

        ICookiesReader Create(IWebBrowserInfo webBrowserInfo);

    }

}