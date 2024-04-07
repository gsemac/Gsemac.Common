using Gsemac.Net.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Net.Http {

    public sealed class UrlEncodedFormData :
    IUrlEncodedFormData,
    IEnumerable<KeyValuePair<string, string>> {

        // Public members

        public string this[string key] {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public UrlEncodedFormData() { }
        public UrlEncodedFormData(string value) {

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            UrlEncodedFormData result = Parse(value);

            fields = result.fields;

        }

        public void Add(string key, string value) {

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(ExceptionMessages.UrlEncodedFormDataKeyCannotBeEmpty, nameof(key));

            fields.Add(new KeyValuePair<string, string>(key, value));

        }

        public byte[] ToBytes() {

            return encoding.GetBytes(ToString());

        }

        public override string ToString() {

            return string.Join("&", fields.Select(field => $"{Uri.EscapeDataString(field.Key)}={Uri.EscapeDataString(field.Value)}"));

        }

        public static UrlEncodedFormData Parse(string value) {

            if (!TryParse(value, out UrlEncodedFormData result))
                throw new FormatException("The form data was malformed.");

            return result;

        }
        public static bool TryParse(string value, out UrlEncodedFormData result) {

            result = new UrlEncodedFormData();

            if (value is null)
                return false;

            // An empty string will be considered value (empty) form data.

            if (string.IsNullOrWhiteSpace(value))
                return true;

            string[] keyValueStrs = value.Split('&');

            foreach (string keyValueStr in keyValueStrs) {

                string[] parts = keyValueStr.Split('=');
                string keyPart = parts[0];
                string valuePart = parts.Length > 1 ? parts[1] : string.Empty;

                result.Add(Uri.UnescapeDataString(keyPart), Uri.UnescapeDataString(valuePart));

            }

            return true;

        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {

            return fields.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        // Should UTF-8 be the default encoding?
        // According to the HTTP spec, the default encoding should be ISO-8859-1 (https://stackoverflow.com/a/708942/5383169).
        // However, it seems that web browsers will commonly default to UTF-& instead.

        private readonly Encoding encoding = Encoding.UTF8;
        private readonly List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();

        private int GetKeyIndex(string key) {

            if (string.IsNullOrWhiteSpace(key))
                return -1;

            // Note that parameter names are case-sensitive.

            return fields.FindIndex(pair => pair.Key.Equals(key, StringComparison.Ordinal));

        }
        private string GetValue(string key) {

            int index = GetKeyIndex(key);

            if (index < 0)
                return string.Empty;

            return fields[index].Value;

        }
        private void SetValue(string key, string value) {

            int index = GetKeyIndex(key);

            KeyValuePair<string, string> newPair = new KeyValuePair<string, string>(key, value ?? string.Empty);

            if (index < 0) {

                // The key doesn't exist, so we'll add a new one.

                fields.Add(newPair);

            }
            else {

                // The key already exists, so we'll replace it.

                fields.RemoveAt(index);
                fields.Insert(index, newPair);

            }

        }

    }

}