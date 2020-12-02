namespace Gsemac.Net.WebBrowsers {

    public interface ICookieDecryptor {

        bool CheckSignature(byte[] encryptedValue);
        byte[] DecryptCookie(byte[] encryptedValue);

    }

}