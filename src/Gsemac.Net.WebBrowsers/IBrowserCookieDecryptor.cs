namespace Gsemac.Net.WebBrowsers {

    internal interface IBrowserCookieDecryptor {

        byte[] DecryptCookie(byte[] encryptedBytes);
        bool TryDecryptCookie(byte[] encryptedBytes, out byte[] decryptedBytes);

    }

}