using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Net.Curl {

    internal static class CurlUtilities {

        // Public members

        public static string CABundlePath => caBundlePath.Value;
        public static string CurlExecutablePath => curlExecutablePath.Value;
        public static string LibCurlPath => libCurlPath.Value;

        public static void InitializeCurl() {

            lock (globalInitMutex) {

                if (isInitialized)
                    return;

                LibCurl.GlobalInit();

                isInitialized = true;

            }

        }
        public static void DeinitializeCurl() {

            lock (globalInitMutex) {

                if (isInitialized) {

                    LibCurl.GlobalCleanup();

                    isInitialized = false;

                }

            }

        }

        // Private members

        private static readonly Lazy<string> caBundlePath = new Lazy<string>(() => ResolveFile("curl-ca-bundle.crt"));
        private static readonly Lazy<string> curlExecutablePath = new Lazy<string>(() => ResolveFile("curl.exe"));
        private static readonly Lazy<string> libCurlPath = new Lazy<string>(() => ResolveFile(Environment.Is64BitProcess ? "libcurl-x64.dll" : "libcurl.dll"));

        private static readonly object globalInitMutex = new object();
        private static bool isInitialized = false;

        private static IEnumerable<IFileSystemAssemblyResolver> GetAssemblyResolvers() {

            IFileSystemAssemblyResolver assemblyResolver = new AnyCpuFileSystemAssemblyResolver() {
                AddExtension = false,
            };

            assemblyResolver.ProbingPaths.Add("lib");
            assemblyResolver.ProbingPaths.Add("bin");
            assemblyResolver.ProbingPaths.Add("plugins");

            return new IFileSystemAssemblyResolver[] {
                FileSystemAssemblyResolver.Default,
                assemblyResolver,
            };

        }
        private static string ResolveFile(string fileName) {

            return GetAssemblyResolvers().Select(resolver => resolver.GetAssemblyPath(fileName))
                .Where(path => File.Exists(path))
                .FirstOrDefault();

        }

    }

}