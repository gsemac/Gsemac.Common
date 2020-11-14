namespace Gsemac.Net.WebDrivers {

    /* This enum is identical to the one in the OpenQA.Selenium namespace.
     * However, by using a custom enum, the IWebDriverOptions class can be referenced without requiring WebDriver.dll to exist.
     * This is useful for scenarios where the lib may or may not be present.
     */

    public enum PageLoadStrategy {
        Default = 0,
        Normal = 1,
        Eager = 2,
        None = 3
    }

}