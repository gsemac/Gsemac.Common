namespace Gsemac.Net.WebDrivers {

    public class WebDriverUpdaterOptions :
        IWebDriverUpdaterOptions {

        public string WebDriverDirectoryPath { get; set; }

        public WebDriverUpdaterOptions() {
        }
        public WebDriverUpdaterOptions(string webDriverDirectory) {

            this.WebDriverDirectoryPath = webDriverDirectory;

        }

        public static WebDriverUpdaterOptions Default => new WebDriverUpdaterOptions();

    }

}