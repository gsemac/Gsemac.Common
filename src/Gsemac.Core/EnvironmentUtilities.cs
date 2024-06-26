﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gsemac.Core {

    public static class EnvironmentUtilities {

        // Public members

        public static void AddEnvironmentPath(string path) {

            AddEnvironmentPaths(new[] { path });

        }
        public static void AddEnvironmentPaths(IEnumerable<string> paths) {

            // https://stackoverflow.com/a/2864714 (Chris Schmich)

            string[] oldPathValue = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };
            string newPathValue = string.Join(Path.PathSeparator.ToString(), oldPathValue.Concat(paths));

            Environment.SetEnvironmentVariable("PATH", newPathValue);

        }
        public static IEnumerable<string> GetEnvironmentPaths() {

            return Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? Enumerable.Empty<string>();

        }

        public static IVersion GetClrVersion() {

            return new MSVersion(Environment.Version);

        }
        public static IVersion GetFrameworkVersion() {

            return Version.Parse(FileVersionInfo.GetVersionInfo(typeof(int).Assembly.Location).ProductVersion);

        }
        public static IPlatformInfo GetPlatformInfo() {

            // This method was adapted from the answer given here: https://stackoverflow.com/a/38795621/5383169 (jariq)

            string windir = Environment.GetEnvironmentVariable("windir");

            if (!string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir)) {

                // Detected Windows

                return new PlatformInfo(PlatformId.Windows);

            }
            else if (File.Exists(@"/proc/sys/kernel/ostype")) {

                string osType = File.ReadAllText(@"/proc/sys/kernel/ostype");

                if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase)) {

                    // Detected Linux or Android

                    return new PlatformInfo(PlatformId.Linux);

                }

            }
            else if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist")) {

                // Detected MacOS or iOS

                return new PlatformInfo(PlatformId.MacOS);

            }

            return new PlatformInfo(PlatformId.Unknown);

        }

        public static bool IsUsingDarkMode() {

            return IsUsingDarkMode(systemWide: false);

        }
        public static bool IsUsingDarkMode(bool systemWide) {

            try {

                // The value of the registry key is 1 if we're using light mode; otherwise, we're using dark mode.
                // https://stackoverflow.com/a/72172845/5383169

                int lightThemeValue = (int)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", systemWide ? "SystemUsesLightTheme" : "AppsUseLightTheme", -1);

                if (lightThemeValue == -1 || lightThemeValue == 1)
                    return false;

                return true;

            }
            catch {

                return false;

            }

        }

    }

}