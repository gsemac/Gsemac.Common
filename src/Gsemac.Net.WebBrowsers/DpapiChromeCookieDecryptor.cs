using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Gsemac.Net.WebBrowsers {

    public class DpapiChromeCookieDecryptor :
        ICookieDecryptor {

        // Public members

        public bool CheckSignature(byte[] encryptedValue) {

            return encryptedValue.Take(signatureBytes.Length).SequenceEqual(signatureBytes);

        }
        public byte[] DecryptCookie(byte[] encryptedValue) {

            using (MemoryStream stream = new MemoryStream(encryptedValue))
            using (BinaryReader reader = new BinaryReader(stream)) {

                if (!reader.ReadBytes(signatureBytes.Length).SequenceEqual(signatureBytes))
                    throw new FormatException("Encrypted value is not in the correct format.");

                return ProtectedData.Unprotect(encryptedValue, null, DataProtectionScope.CurrentUser);

            }

        }

        // Private members

        private readonly byte[] signatureBytes = new byte[] { 0x01, 0x00, 0x00, 0x00, 0xD0, 0x8C, 0x9D, 0xDF, 0x01, 0x15, 0xD1, 0x11, 0x8C, 0x7A, 0x00, 0xC0, 0x4F, 0xC2, 0x97, 0xEB }; // DPAPI signature

    }

}