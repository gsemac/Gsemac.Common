using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.IO {

    public class MimeType :
        IMimeType {

        // Public members

        public string Type { get; private set; }
        public string Subtype { get; private set; }
        public IDictionary<string, string> Parameters { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public MimeType(string mimeType) {

            if (string.IsNullOrEmpty(mimeType))
                throw new ArgumentNullException(nameof(mimeType));

            mimeType = mimeType.ToLowerInvariant();

            string[] parts = mimeType.Split(';');

            if (!parts.Any())
                throw new ArgumentException(Properties.ExceptionMessages.MalformedMimeType, nameof(mimeType));

            ParseTypeAndSubtype(parts[0]);

            if (parts.Count() > 1)
                foreach (string parameter in parts.Skip(1).Where(part => !string.IsNullOrWhiteSpace(part)))
                    ParseParameter(parameter);

        }
        public MimeType(string type, string subtype) {

            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrEmpty(subtype))
                throw new ArgumentNullException(nameof(subtype));

            this.Type = type.Trim();
            this.Subtype = subtype.Trim();

        }

        public override bool Equals(object obj) {

            if (obj is null)
                return false;

            return ReferenceEquals(this, obj) ||
                (obj is IMimeType mimeType && ToString().Equals(mimeType.ToString(), StringComparison.OrdinalIgnoreCase));

        }
        public override int GetHashCode() {

            return ToString().GetHashCode();

        }

        public override string ToString() {

            return ToString(withParameters: true);

        }
        public string ToString(bool withParameters) {

            StringBuilder sb = new StringBuilder();

            sb.Append(Type);
            sb.Append('/');
            sb.Append(Subtype);

            if (withParameters && Parameters is object) {

                foreach (var parameter in Parameters.Where(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value))) {

                    sb.Append(';');
                    sb.Append(parameter.Key.Trim());
                    sb.Append('=');
                    sb.Append(parameter.Value.Trim());

                }

            }

            return sb.ToString().ToLowerInvariant();

        }

        public static MimeType Parse(string mimeType) {

            return new MimeType(mimeType);

        }
        public static bool TryParse(string mimeType, out MimeType result) {

            try {

                result = Parse(mimeType);

                return true;

            }
            catch (ArgumentException) {

                result = null;

                return false;

            }

        }

        // Private members

        private void ParseTypeAndSubtype(string typeAndSubtype) {

            string[] parts = typeAndSubtype.Split('/');

            if (parts.Count() != 2 || parts.Any(part => string.IsNullOrWhiteSpace(part)))
                throw new ArgumentException(Properties.ExceptionMessages.MalformedMimeType, nameof(typeAndSubtype));

            this.Type = parts[0].Trim();
            this.Subtype = parts[1].Trim();

        }
        private void ParseParameter(string parameter) {

            string[] parts = parameter.Split('=');

            if (parts.Count() != 2 || parts.Any(part => string.IsNullOrWhiteSpace(part)))
                throw new ArgumentException(Properties.ExceptionMessages.MalformedMimeType, nameof(parameter));

            Parameters[parts[0].Trim()] = parts[1].Trim();

        }

    }

}