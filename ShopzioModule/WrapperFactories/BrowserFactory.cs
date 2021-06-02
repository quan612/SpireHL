using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;

namespace ShopzioModule.WrapperFactories
{
    public class BrowserFactory
    {
        private static readonly IDictionary<string, IWebDriver> Drivers = new Dictionary<string, IWebDriver>();
        private static IWebDriver driver;

        public static IWebDriver Driver
        {
            get
            {
                if (driver == null)
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method InitBrowser.");
                return driver;
            }
            private set
            {
                driver = value;
            }
        }

        public static void InitBrowser(string browserName)
        {
            switch (browserName)
            {
                case "Firefox":
                    driver = new FirefoxDriver();
                    Drivers.Add("Firefox", Driver);
                    break;

                case "Chrome":
                    ChromeDriverService service = ChromeDriverService.CreateDefaultService("c:\\");
                    service.HideCommandPromptWindow = true;

                    var optionsChrome = new ChromeOptions();
                    optionsChrome.AddArgument("start-maximized");

                    driver = new ChromeDriver(service, optionsChrome);

                    Drivers.Add("Chrome", Driver);
                    break;
            }
        }

        public static void LoadApplication(string url)
        {
            Driver.Url = url;
        }

        public static void CloseAllDrivers()
        {
            foreach (var key in Drivers.Keys)
            {
                Drivers[key].Close();
                Drivers[key].Quit();
            }
            Drivers.Clear();
        }
    }
}
