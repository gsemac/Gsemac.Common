namespace Gsemac.Net.WebBrowsers {

    public interface IChromeCookieDecryptor {

        byte[] DecryptCookie(byte[] encryptedValue);

    }

}