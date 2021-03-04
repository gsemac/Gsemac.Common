using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Gsemac.Net.WebBrowsers {

    internal class DpapiChromeCookieDecryptor :
        ICookieDecryptor {

        // Public members

        public byte[] DecryptCookie(byte[] encryptedBytes) {

            using (MemoryStream stream = new MemoryStream(encryptedBytes)) {

                if (!CheckSignature(signatureBytes))
                    throw new FormatException("Encrypted value is not in the correct format.");

                return ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);

            }

        }
        public bool TryDecryptCookie(byte[] encryptedBytes, out byte[] decryptedBytes) {

            decryptedBytes = null;

            if (!CheckSignature(encryptedBytes))
                return false;

            decryptedBytes = DecryptCookie(encryptedBytes);

            return true;

        }

        // Private members

        private readonly byte[] signatureBytes = new byte[] { 0x01, 0x00, 0x00, 0x00, 0xD0, 0x8C, 0x9D, 0xDF, 0x01, 0x15, 0xD1, 0x11, 0x8C, 0x7A, 0x00, 0xC0, 0x4F, 0xC2, 0x97, 0xEB }; // DPAPI signature

        private bool CheckSignature(byte[] encryptedBytes) {

            return encryptedBytes.Take(signatureBytes.Length).SequenceEqual(signatureBytes);

        }

    }

}