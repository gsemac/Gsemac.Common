using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.Net.Http {

    public class UrlEncodedFormDataBuilder {

        // Public members

        public UrlEncodedFormDataBuilder() {
        }
        public UrlEncodedFormDataBuilder(Encoding encoding) {

            this.encoding = encoding;

        }

        public UrlEncodedFormDataBuilder WithField(string key, string value) {

            fields.Add(new KeyValuePair<string, string>(key, value));

            return this;

        }

        public byte[] Build() {

            return encoding.GetBytes(ToString());


        }

        public override string ToString() {

            return string.Join("&", fields.Select(field => $"{field.Key}={Uri.EscapeDataString(field.Value)}"));

        }

        // Private members

        // Should UTF-8 be the default encoding?
        // According to the HTTP spec, the default encoding should be ISO-8859-1 (https://stackoverflow.com/a/708942/5383169).
        // However, it seems that web browsers will commonly default to UTF-& instead.
        private readonly Encoding encoding = Encoding.UTF8;
        private readonly List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();

    }

}