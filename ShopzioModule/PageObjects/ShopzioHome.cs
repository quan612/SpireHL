using OpenQA.Selenium;
using ShopzioModule.Extensions;

namespace ShopzioModule.PageObjects
{
    public class ShopzioHome
    {
        private IWebDriver _driver;
        public ShopzioHome(IWebDriver driver)
        {
            _driver = driver;
        }

        private By _orderTab => By.XPath("//a[@title='Orders']");

        private By _createOrderTab => By.XPath("//a[@href='/orders/create']");

        public ShopzioCreateOrder GoToCreateOrderScreen()
        {
            _driver.WaitForVisibilityAndFindTheElement(_orderTab)
               .Click();

            _driver.WaitForVisibilityAndFindTheElement(_createOrderTab)
               .Click();

            return new ShopzioCreateOrder(_driver);
        }
    }
}
