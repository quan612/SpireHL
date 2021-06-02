using ShopzioModule.Models;
using ShopzioModule.PageObjects;
using ShopzioModule.WrapperFactories;
using SpireHL.Core.Models;
using System.Collections.Generic;
using System.Threading;

namespace ShopzioModule.Services
{
    public class ShopzioService
    {
        private List<SpireShopzioItem> _spireItems;
        private ShopzioCreateOrderUserOptions _userOptions;
        public ShopzioService(List<SpireShopzioItem> spireItems, ShopzioCreateOrderUserOptions userOptions)
        {
            _spireItems = spireItems;
            _userOptions = userOptions;
        }

        public string RunImport()
        {
            //BrowserFactory.InitBrowser("Firefox");
            BrowserFactory.InitBrowser("Chrome");
            BrowserFactory.LoadApplication("https://manage.repzio.com/");

            var homePage = LoginShopzio();
            var createOrder = homePage.GoToCreateOrderScreen();
            var orderNumber = createOrder.CreateNewOrder(_spireItems, _userOptions);
            Thread.Sleep(2000);
            BrowserFactory.CloseAllDrivers();
            return orderNumber;

        }

        private ShopzioHome LoginShopzio()
        {
            var loginPage = new ShopzioLogin(BrowserFactory.Driver);
            return loginPage.Login();
        }
    }
}
