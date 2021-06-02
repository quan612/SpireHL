using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace ShopzioModule.Extensions
{
    public static class WebElementExtensions
    {
        public static IWebElement WaitForVisibilityAndFindTheElement(this IWebDriver driver, By byLocator)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var element = wait.Until<IWebElement>((d) =>
                {
                    IWebElement ele = d.FindElement(byLocator);
                    if (ele.Displayed &&
                        ele.Enabled)
                    {
                        return ele;
                    }

                    return null;
                });

                if (element != null)
                {
                    return element;
                }
                else
                {
                    throw new Exception($"{byLocator} cannot be found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
