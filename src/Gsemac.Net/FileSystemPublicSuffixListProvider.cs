using Gsemac.IO;
using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Net {

    public sealed class FileSystemPublicSuffixListProvider :
        PublicSuffixListProviderBase {

        // Public members

        public FileSystemPublicSuffixListProvider() { }
        public FileSystemPublicSuffixListProvider(IFileSystemAssemblyResolver resolver) {

            if (resolver is null)
                throw new ArgumentNullException(nameof(resolver));

            this.resolver = resolver;

        }

        public override IEnumerable<string> GetList() {

            IFileSystemAssemblyResolver resolver = this.resolver ??
                FileSystemAssemblyResolver.Default;

            if (resolver is null)
                return Enumerable.Empty<string>();

            // We need a resolver where "AddExtension" is false so we can look for the desired file name explicitly.
            // This is probably not the ideal mechanism for looking up the list, but it works for now. 

            resolver = new FileSystemAssemblyResolver(resolver.ProbingPaths) {
                AddExtension = false,
            };

            string filePath = resolver.GetAssemblyPath("public_suffix_list.dat");

            if (File.Exists(filePath) && FileUtilities.TryOpen(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, out FileStream fileStream)) {

                using (Stream stream = fileStream)
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8)) {

                    return ParseList(sr.ReadToEnd());

                }

            }

            // Return the default list if we weren't able to open a file.

            return base.GetList();

        }

        // Private members

        private readonly IFileSystemAssemblyResolver resolver;

    }

}