using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    public class ChromeCookieDecryptor :
        ICookieDecryptor {

        // Public members

        public ChromeCookieDecryptor() {

            decryptors.Add(new Aes256GcmChromeCookieDecryptor());
            decryptors.Add(new DPApiChromeCookieDecryptor());

        }

        public bool CheckSignature(byte[] encryptedValue) {

            return decryptors.Any(decryptor => decryptor.CheckSignature(encryptedValue));

        }
        public byte[] DecryptCookie(byte[] encryptedValue) {

            ICookieDecryptor cookieDecryptor = decryptors.Where(decryptor => decryptor.CheckSignature(encryptedValue)).FirstOrDefault();

            if (!(cookieDecryptor is null))
                return cookieDecryptor.DecryptCookie(encryptedValue);

            throw new FormatException("Encrypted value is not in the correct format.");

        }

        // Private members

        private readonly IList<ICookieDecryptor> decryptors = new List<ICookieDecryptor>();

    }

}