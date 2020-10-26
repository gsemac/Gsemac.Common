using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Net.WebBrowsers {

    public static class WebBrowserUtilities {

        public static IEnumerable<string> GetInstalledWebBrowserExecutablePaths() {

            // A better way of detecting installed web browsers might be looking up their associated keys in the registry.

            IEnumerable<string> driveDirectoryPaths = System.IO.DriveInfo.GetDrives().Select(info => info.RootDirectory.FullName);

            IEnumerable<string> programFilesDirectoryPaths = new string[]{
                @"Program Files",
                @"Program Files (x86)"
            }.SelectMany(path => driveDirectoryPaths.Select(drivePath => System.IO.Path.Combine(drivePath, path)));

            IEnumerable<string> webBrowserExecutablePaths = new string[]{
                @"Google\Chrome\Application\chrome.exe",
                @"Mozilla Firefox\firefox.exe"
            }.SelectMany(path => programFilesDirectoryPaths.Select(programFilesDirectoryPath => System.IO.Path.Combine(programFilesDirectoryPath, path)));

            return webBrowserExecutablePaths.Where(path => System.IO.File.Exists(path));

        }

    }

}