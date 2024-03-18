using Gsemac.Net.WebBrowsers.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    internal sealed class ChromiumCookieDecryptor :
        IWebBrowserCookieDecryptor {

        // Public members

        public ChromiumCookieDecryptor(string userDataDirectoryPath) {

            decryptors.Add(new ChromiumAes256GcmCookieDecryptor(userDataDirectoryPath));
            decryptors.Add(new ChromiumDpapiCookieDecryptor());

        }

        public byte[] DecryptCookie(byte[] encryptedBytes) {

            // It's okay if we have a zero-length byte array, which will be the case for cookies with empty values.

            if (encryptedBytes.Length <= 0)
                return new byte[0];

            if (TryDecryptCookie(encryptedBytes, out byte[] decryptedBytes))
                return decryptedBytes;

            throw new FormatException(ExceptionMessages.EncryptedDataIsMalformed);

        }
        public bool TryDecryptCookie(byte[] encryptedBytes, out byte[] decryptedBytes) {

            decryptedBytes = decryptors.Select(decryptor => decryptor.TryDecryptCookie(encryptedBytes, out byte[] result) ? result : null)
                .Where(result => result is object)
                .FirstOrDefault();

            return decryptedBytes is object;

        }

        // Private members

        private readonly IList<IWebBrowserCookieDecryptor> decryptors = new List<IWebBrowserCookieDecryptor>();

    }

}