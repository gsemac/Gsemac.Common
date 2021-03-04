using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IO;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    internal class Aes256GcmChromeCookieDecryptor :
        ICookieDecryptor {

        // Public members


        public byte[] DecryptCookie(byte[] encryptedBytes) {

            // Based on the answer given here: https://stackoverflow.com/a/60423699 (Topaco)

            using (MemoryStream stream = new MemoryStream(encryptedBytes))
            using (BinaryReader reader = new BinaryReader(stream)) {

                if (!reader.ReadBytes(signatureBytes.Length).SequenceEqual(signatureBytes))
                    throw new FormatException("Encrypted value is not in the correct format.");

                byte[] nonce = reader.ReadBytes(12);
                byte[] ciphertext = reader.ReadBytes(encryptedBytes.Length - (signatureBytes.Length + nonce.Length));
                byte[] decryptionKey = GetDecryptionKey();

                return AesGcmDecrypt(ciphertext, decryptionKey, nonce);

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

        private readonly byte[] signatureBytes = new byte[] { 0x76, 0x31, 0x30 }; // ASCII encoding of "v10" 
        private byte[] decryptionKey;

        private bool CheckSignature(byte[] encryptedBytes) {

            return encryptedBytes.Take(signatureBytes.Length).SequenceEqual(signatureBytes);

        }

        private string GetLocalStatePath() {

            string localStatePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Google\Chrome\User Data\Local State");

            if (File.Exists(localStatePath))
                return localStatePath;

            throw new FileNotFoundException("Could not determine local state path.", localStatePath);


        }
        private byte[] GetDecryptionKey() {

            if (decryptionKey is null) {

                // Read encrypted decryption key from Local State.

                string localStatePath = GetLocalStatePath();
                JObject localState = JObject.Parse(File.ReadAllText(localStatePath));

                string encryptedKey = localState["os_crypt"]["encrypted_key"].ToString();
                byte[] encryptedKeyBytes = Convert.FromBase64String(encryptedKey);

                // ASCII encoding of "DPAPI"

                byte[] dpapiBytes = new byte[] { 0x44, 0x50, 0x41, 0x50, 0x49 };

                if (!encryptedKeyBytes.Take(dpapiBytes.Length).SequenceEqual(dpapiBytes))
                    throw new FormatException("Encrypted key is not in the correct format.");

                encryptedKeyBytes = encryptedKeyBytes.Skip(dpapiBytes.Length).ToArray();

                // Decrypt the encrypted key.

                decryptionKey = System.Security.Cryptography.ProtectedData.Unprotect(encryptedKeyBytes, null, System.Security.Cryptography.DataProtectionScope.LocalMachine);

            }

            return decryptionKey;

        }
        private byte[] AesGcmDecrypt(byte[] ciphertext, byte[] decryptionKey, byte[] nonce) {

            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            AeadParameters parameters = new AeadParameters(new KeyParameter(decryptionKey), 128, nonce);

            cipher.Init(false, parameters);

            byte[] decryptedBytes = new byte[cipher.GetOutputSize(ciphertext.Length)];

            int length = cipher.ProcessBytes(ciphertext, 0, ciphertext.Length, decryptedBytes, 0);

            cipher.DoFinal(decryptedBytes, length);

            return decryptedBytes;

        }

    }

}