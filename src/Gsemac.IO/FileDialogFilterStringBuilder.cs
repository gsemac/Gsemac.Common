using Gsemac.IO.FileFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsemac.IO {

    public sealed class FileDialogFilterStringBuilder :
        IFileDialogFilterStringBuilder {

        // Public members

        public IFileDialogFilterStringBuilder WithFileFormat(IFileFormat format) {

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            if (format.Equals(new AnyFileFormat())) {

                showAllFilesOption = true;

            }
            else {

                return WithFileFormat(format.Name, format.Extensions);

            }

            return this;

        }

        public IFileDialogFilterStringBuilder WithFileFormat(string name, string extension) {

            if (string.IsNullOrWhiteSpace(name))
                return this;

            return WithFileFormat(name, new[] { extension });

        }
        public IFileDialogFilterStringBuilder WithFileFormat(string name, IEnumerable<string> extensions) {

            if (extensions is null)
                throw new ArgumentNullException(nameof(extensions));

            if (string.IsNullOrWhiteSpace(name))
                return this;

            string formattedName = FormatFileFormatName(name);
            string formattedFileExtensions = string.Join(";", extensions.Select(ext => FormatFileFormatExtension(ext)));

            formats.Add($"{formattedName} ({formattedFileExtensions})|({formattedFileExtensions})");

            return this;


        }

        public IFileDialogFilterStringBuilder WithAllFilesOption() {

            showAllFilesOption = true;

            return this;

        }

        public string Build() {

            StringBuilder sb = new StringBuilder();

            sb.Append(string.Join("|", formats.Distinct()));

            if (showAllFilesOption)
                sb.Append("|All Files (*.*)|*.*");

            return sb.ToString();

        }

        public override string ToString() {

            return Build();

        }

        // Private members

        private readonly List<string> formats = new List<string>();
        private bool showAllFilesOption = false;

        private string FormatFileFormatName(string name) {

            // File dialog filters, in practice, tend to use title case for the file format names.
            // The name should be plural.

            if (string.IsNullOrWhiteSpace(name))
                return name;

            if (!name.EndsWith("s", StringComparison.OrdinalIgnoreCase))
                name += "s";

            return name;


        }
        private string FormatFileFormatExtension(string extension) {

            // File extensions should be lowercase and begin with a wildcard character.

            if (string.IsNullOrWhiteSpace(extension))
                return extension;

            extension = extension.TrimStart('*', '.')
                .ToLowerInvariant();

            return $"*.{extension}";

        }

    }

}