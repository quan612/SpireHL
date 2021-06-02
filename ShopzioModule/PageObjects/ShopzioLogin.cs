using OpenQA.Selenium;
using ShopzioModule.Extensions;

namespace ShopzioModule.PageObjects
{
    public class ShopzioLogin
    {
        private IWebDriver _driver;

        private By _userName => By.Id("login-email");

        private By _password => By.Id("login-password");

        private By _loginBtn => By.Name("button");

        public ShopzioLogin(IWebDriver driver)
        {
            _driver = driver;
        }

        public ShopzioHome Login()
        {
            _driver.WaitForVisibilityAndFindTheElement(_userName)
                .SendKeys("HiLine Gift");

            _driver.WaitForVisibilityAndFindTheElement(_password)
                .SendKeys("37348128");

            _driver.WaitForVisibilityAndFindTheElement(_loginBtn)
                .Click();

            return new ShopzioHome(_driver);
        }
    }
}