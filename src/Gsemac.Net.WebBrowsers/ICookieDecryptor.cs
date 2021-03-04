namespace Gsemac.Net.WebBrowsers {

    public interface ICookieDecryptor {

        byte[] DecryptCookie(byte[] encryptedBytes);
        bool TryDecryptCookie(byte[] encryptedBytes, out byte[] decryptedBytes);

    }

}