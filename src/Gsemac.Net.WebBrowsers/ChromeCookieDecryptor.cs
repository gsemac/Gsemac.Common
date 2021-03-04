using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    public class ChromeCookieDecryptor :
        ICookieDecryptor {

        // Public members

        public ChromeCookieDecryptor() {

            decryptors.Add(new Aes256GcmChromeCookieDecryptor());
            decryptors.Add(new DpapiChromeCookieDecryptor());

        }

        public byte[] DecryptCookie(byte[] encryptedBytes) {

            if (TryDecryptCookie(encryptedBytes, out byte[] decryptedBytes))
                return decryptedBytes;

            throw new FormatException("Encrypted value is not in the correct format.");

        }
        public bool TryDecryptCookie(byte[] encryptedBytes, out byte[] decryptedBytes) {

            decryptedBytes = decryptors.Select(decryptor => decryptor.TryDecryptCookie(encryptedBytes, out byte[] result) ? result : null)
                .Where(result => result is object)
                .FirstOrDefault();

            return decryptedBytes is object;

        }

        // Private members

        private readonly IList<ICookieDecryptor> decryptors = new List<ICookieDecryptor>();

    }

}