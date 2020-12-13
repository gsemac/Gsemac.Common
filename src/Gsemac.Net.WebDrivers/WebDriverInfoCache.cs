using Gsemac.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverInfoCache :
        IWebDriverInfoCache {

        // Public members

        public IWebDriverInfo GetWebDriverInfo(string webDriverFilePath) {

            if (File.Exists(webDriverFilePath)) {

                // Look up the web driver by file hash.

                string md5Hash = FileUtilities.CalculateMD5Hash(webDriverFilePath).ToLowerInvariant();

                if (fileHashDict.TryGetValue(md5Hash, out IWebDriverInfo webDriverInfo))
                    return webDriverInfo;

            }

            // If we get here, the web driver was not in the cache.

            return null;

        }
        public void AddWebDriverInfo(IWebDriverInfo webDriverInfo) {

            if (string.IsNullOrWhiteSpace(webDriverInfo.Md5Hash))
                throw new ArgumentException("The MD5 hash was empty.", nameof(webDriverInfo));

            fileHashDict[webDriverInfo.Md5Hash.ToLowerInvariant()] = webDriverInfo;

        }

        public void SaveTo(Stream stream) {

            using (MemoryStream tempStream = new MemoryStream(Encoding.UTF8.GetBytes(ToJson())))
                tempStream.CopyTo(stream);

        }

        // Private members

        private readonly IDictionary<string, IWebDriverInfo> fileHashDict = new Dictionary<string, IWebDriverInfo>();

        private string ToJson() {

            return JsonConvert.SerializeObject(fileHashDict.Values, Formatting.Indented);

        }

    }

}