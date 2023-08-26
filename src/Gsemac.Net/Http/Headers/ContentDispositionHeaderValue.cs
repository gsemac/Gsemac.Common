using Gsemac.Net.Properties;
using Gsemac.Text;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http.Headers {

    public sealed class ContentDispositionHeaderValue {

        // Public members

        public ContentDispositionType Type { get; private set; }
        public string Name { get; private set; }
        public string FileName { get; private set; }

        public ContentDispositionHeaderValue(string value) {

            ContentDispositionHeaderValue header = Parse(value);

            Type = header.Type;
            Name = header.Name;
            FileName = header.FileName;

        }

        public static ContentDispositionHeaderValue Parse(string value) {

            if (TryParse(value, out ContentDispositionHeaderValue result))
                return result;
            else
                throw new FormatException(string.Format(ExceptionMessages.InvalidHttpContentDispositionHeaderWithString, value));


        }
        public static bool TryParse(string value, out ContentDispositionHeaderValue result) {

            result = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Match the content disposition type.

            // Parameters/directives are case-insensitive.
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Disposition#as_a_header_for_a_multipart_body

            Match typeAndParametersMatch = Regex.Match(value, @"^(?<type>attachment|inline|form-data)(?:;?\s*(?<parameters>\s*.+))?$", RegexOptions.IgnoreCase);

            if (!typeAndParametersMatch.Success || !TryParseContentDispositionType(typeAndParametersMatch.Groups["type"].Value, out ContentDispositionType contentDispositionType))
                return false;

            // Parse the remaining key-value pairs.

            string name = string.Empty;
            string fileName = string.Empty;
            string encodingName = string.Empty;

            foreach (Match parameterMatch in Regex.Matches(typeAndParametersMatch.Groups["parameters"].Value.Trim(), @"(?<name>.+?)\s*=\s*(?<value>.+?)(?:;|$)\s*")) {

                string parameterName = parameterMatch.Groups["name"].Value.Trim();
                string parameterValue = parameterMatch.Groups["value"].Value.Trim();

                // Strip outer quotes.

                if (parameterValue.StartsWith("\"") && parameterValue.EndsWith("\""))
                    parameterValue = parameterValue.Substring(1, parameterValue.Length - 2);

                switch (parameterName.ToLowerInvariant()) {

                    case "name":

                        name = parameterValue;

                        break;

                    case "filename":

                        // The "filename*" parameter takes precedence over "filename", so we'll only update the file name if it hasn't been set already.

                        if (string.IsNullOrWhiteSpace(fileName))
                            fileName = parameterValue;

                        break;

                    case "filename*":

                        // The "filename*" parameter will always overwrite the file name previously set by "filename".

                        string[] fileNameParts = parameterValue.Split(new[] { "''" }, StringSplitOptions.None);

                        if (fileNameParts.Length > 1) {

                            encodingName = fileNameParts[0];
                            fileName = Uri.UnescapeDataString(fileNameParts[1]);

                        }
                        else {

                            fileName = Uri.UnescapeDataString(fileNameParts[0]);

                        }

                        break;

                }

            }

            result = new ContentDispositionHeaderValue() {
                Type = contentDispositionType,
                Name = name,
                FileName = fileName,
                encodingName = encodingName,
            };

            return result is object;

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            switch (Type) {

                case ContentDispositionType.Inline:
                    sb.Append("inline");
                    break;

                case ContentDispositionType.Attachment:
                    sb.Append("attachment");
                    break;

                case ContentDispositionType.FormData:
                    sb.Append("form-data");
                    break;

            }

            if (!string.IsNullOrWhiteSpace(Name))
                sb.Append($"; name=\"{Name}\"");

            if (!string.IsNullOrWhiteSpace(FileName)) {

                if (StringUtilities.CanBeEncodedAs(FileName, Encoding.ASCII)) {

                    sb.Append($"; filename=\"{FileName}\"");

                }
                else {

                    sb.Append($"; filename*=\"{encodingName}''{Uri.EscapeDataString(FileName)}\"");

                }

            }

            return sb.ToString();

        }

        // Private members

        private string encodingName = string.Empty;

        private ContentDispositionHeaderValue() { }

        public static string ContentDispositiontypeToString(ContentDispositionType value) {

            switch (value) {

                case ContentDispositionType.Inline:
                    return "inline";

                case ContentDispositionType.Attachment:
                    return "attachment";

                case ContentDispositionType.FormData:
                    return "form-data";

                default:
                    return string.Empty;

            }

        }
        private static bool TryParseContentDispositionType(string value, out ContentDispositionType result) {

            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            switch (value.ToLowerInvariant().Trim()) {

                case "inline":
                    result = ContentDispositionType.Inline;
                    return true;

                case "attachment":
                    result = ContentDispositionType.Attachment;
                    return true;

                case "form-data":
                    result = ContentDispositionType.FormData;
                    return true;

                default:
                    return false;

            }

        }

    }

}