using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Gsemac.Net.WebDrivers {

    internal class WebDriverInfo :
        IWebDriverInfo {

        [JsonProperty("version"), JsonConverter(typeof(VersionConverter))]
        public Version Version { get; set; }
        [JsonProperty("executablePath")]
        public string ExecutablePath { get; set; }

    }

}