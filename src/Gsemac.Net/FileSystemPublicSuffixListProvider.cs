using Gsemac.IO;
using Gsemac.IO.Logging;
using Gsemac.Reflection;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Net {

    public sealed class FileSystemPublicSuffixListProvider :
        PublicSuffixListProviderBase {

        // Public members

        public FileSystemPublicSuffixListProvider() :
            this(Logger.Null) {
        }
        public FileSystemPublicSuffixListProvider(ILogger logger) :
            this(GetDefaultResolver(), logger) {
        }
        public FileSystemPublicSuffixListProvider(IFileSystemAssemblyResolver resolver) :
            this(resolver, Logger.Null) {
        }
        public FileSystemPublicSuffixListProvider(IFileSystemAssemblyResolver resolver, ILogger logger) {

            if (resolver is null)
                throw new ArgumentNullException(nameof(resolver));

            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.resolver = resolver;
            this.logger = new NamedLogger(logger, nameof(FileSystemPublicSuffixListProvider));

        }

        public override IPublicSuffixList GetList() {

            lock (mutex) {

                bool isCacheInvalid = cache is null ||
                    (TimeToLive != default && (cacheLastUpdated == default || DateTimeOffset.Now > cacheLastUpdated + TimeToLive));

                if (isCacheInvalid) {

                    cache = GetListInternal();
                    cacheLastUpdated = DateTimeOffset.Now;

                }

                return cache;

            }

        }

        // Private members

        private readonly IFileSystemAssemblyResolver resolver;
        private readonly ILogger logger;
        private readonly object mutex = new object();
        private IPublicSuffixList cache;
        private DateTimeOffset cacheLastUpdated;

        private IPublicSuffixList GetListInternal() {

            IFileSystemAssemblyResolver resolver = this.resolver ??
                GetDefaultResolver();

            if (resolver is null)
                return new PublicSuffixList(Enumerable.Empty<string>());

            // We need a resolver where "AddExtension" is false so we can look for the desired file name explicitly.
            // This is probably not the ideal mechanism for looking up the list, but it works for now. 

            resolver = new FileSystemAssemblyResolver(resolver.ProbingPaths) {
                AddExtension = false,
            };

            string fileName = "public_suffix_list.dat";
            string filePath = resolver.GetAssemblyPath(fileName);

            if (File.Exists(filePath) && FileUtilities.TryOpen(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, out FileStream fileStream)) {

                using (Stream stream = fileStream)
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8)) {

                    return new PublicSuffixList(ParseList(sr.ReadToEnd()));

                }

            }

            if (!FallbackEnabled)
                throw new FileNotFoundException($"Could not find {fileName}.", fileName);

            logger.Warning($"Could not find {fileName}, using internal list");

            // Return the default list if we weren't able to open a file.

            return base.GetList();

        }

        private static IFileSystemAssemblyResolver GetDefaultResolver() {

            return FileSystemAssemblyResolver.Default;

        }

    }

}