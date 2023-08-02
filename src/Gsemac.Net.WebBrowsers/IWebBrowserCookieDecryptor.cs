namespace Gsemac.Net.WebBrowsers {

    internal interface IWebBrowserCookieDecryptor {

        byte[] DecryptCookie(byte[] encryptedBytes);
        bool TryDecryptCookie(byte[] encryptedBytes, out byte[] decryptedBytes);

    }

}