namespace Gsemac.Net.WebDrivers {

    public class WebDriverUpdaterOptions :
        IWebDriverUpdaterOptions {

        public string WebDriverDirectory { get; set; }

        public WebDriverUpdaterOptions() {
        }
        public WebDriverUpdaterOptions(string webDriverDirectory) {

            this.WebDriverDirectory = webDriverDirectory;

        }

        public static WebDriverUpdaterOptions Default => new WebDriverUpdaterOptions();

    }

}