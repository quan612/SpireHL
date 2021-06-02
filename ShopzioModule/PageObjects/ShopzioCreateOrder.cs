using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ShopzioModule.Extensions;
using ShopzioModule.Models;
using SpireHL.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ShopzioModule.PageObjects
{
    public class ShopzioCreateOrder
    {
        private IWebDriver _driver;
        public ShopzioCreateOrder(IWebDriver driver)
        {
            _driver = driver;
        }

        private By _customerInput => By.Id("findcustomer");
        private By _customerName => By.Id("ui-id-2");
        private By _nextBtn => By.XPath("//button[@value='Next']");
        private By _placeByRep => By.Id("placedByDropdownTogglerBtn");
        private By _placeByRepValue => By.XPath("//div[@onclick='selectRepNum(0)']");
        private By _writtenByRepSelect => By.Id("WrittenForRepNumber");
        private By _orderTypeSelect => By.Id("OrderStatus");
        private By _doNotShipBefore => By.Id("DoNotShipBefore");
        private By _submitBtn => By.XPath("//button[@value='Submit']");

        public string CreateNewOrder(List<SpireShopzioItem> spireItems, ShopzioCreateOrderUserOptions userOptions)
        {
            EnterCustomerNameId(userOptions);
            SubmitNewOrder();
            AddToOrder(spireItems, userOptions);
            return GetOrderNumber();
        }

        private void EnterCustomerNameId(ShopzioCreateOrderUserOptions userOptions)
        {
            _driver.WaitForVisibilityAndFindTheElement(_customerInput)
               .SendKeys(userOptions.CustomerName);

            _driver.WaitForVisibilityAndFindTheElement(_customerName).Click();
        }

        private void SubmitNewOrder()
        {

            // add special wait here
            Thread.Sleep(5000);

            _driver.WaitForVisibilityAndFindTheElement(_nextBtn)
               .Click();

            _driver.WaitForVisibilityAndFindTheElement(_nextBtn)
               .Click();

            // Place by Rep
            _driver.WaitForVisibilityAndFindTheElement(_placeByRep)
               .Click();

            _driver.WaitForVisibilityAndFindTheElement(_placeByRepValue)
               .Click();

            // Written For Rep#
            var writtenRepEle = _driver.WaitForVisibilityAndFindTheElement(_writtenByRepSelect);
            var repToSelect = new SelectElement(writtenRepEle);
            repToSelect.SelectByText("Office HLG");

            // Order type
            var orderTypeEle = _driver.WaitForVisibilityAndFindTheElement(_orderTypeSelect);
            var orderTypeToSelect = new SelectElement(orderTypeEle);
            orderTypeToSelect.SelectByText("Test");

            // Do Not Ship Before
            var shippingDate = DateTime.Now.ToString("yyyy-MM-dd");
            _driver.WaitForVisibilityAndFindTheElement(_doNotShipBefore).SendKeys(shippingDate);

            // submit
            _driver.WaitForVisibilityAndFindTheElement(_submitBtn).Click();

        }

        private void AddToOrder(List<SpireShopzioItem> spireItems, ShopzioCreateOrderUserOptions userOptions)
        {
            By _addNewItemBtn = By.Id("btnAddNewRow");
            _driver.WaitForVisibilityAndFindTheElement(_addNewItemBtn).Click();

            // loop here
            foreach (var item in spireItems)
            {
                Thread.Sleep(5000);

                // item id
                var _itemId = By.Id("ItemID");
                _driver.WaitForVisibilityAndFindTheElement(_itemId)
                   .SendKeys(item.PartNo);

                var _itemIdDropdown = By.Id("ui-id-2");
                _driver.WaitForVisibilityAndFindTheElement(_itemIdDropdown).Click();

                // quantity
                if (!string.IsNullOrEmpty(item.ShopzioOrderQty.ToString()) && userOptions.IsQuantityOverride)
                {
                    var _quantityTextBox = By.Id("Qty");
                    _driver.WaitForVisibilityAndFindTheElement(_quantityTextBox).Clear();
                    _driver.WaitForVisibilityAndFindTheElement(_quantityTextBox)
                        .SendKeys(item.ShopzioOrderQty.ToString());
                }

                // price override
                if (userOptions.IsPriceOverride)
                {
                    var _priceOverrideTextbox = By.Id("PriceOverride");
                    if (Decimal.TryParse(item.Price1.ToString(), out decimal overridePrice))
                    {
                        _driver.WaitForVisibilityAndFindTheElement(_priceOverrideTextbox).Clear();
                        _driver.WaitForVisibilityAndFindTheElement(_priceOverrideTextbox).SendKeys(overridePrice.ToString());
                    }
                    // if null or emtpy -> false -> change to 0
                    else
                    {
                        _driver.WaitForVisibilityAndFindTheElement(_priceOverrideTextbox).Clear();
                        _driver.WaitForVisibilityAndFindTheElement(_priceOverrideTextbox).SendKeys("0");
                    }
                }

                // click on add item
                var _addSingleItemBtn = By.Id("save");
                _driver.WaitForVisibilityAndFindTheElement(_addSingleItemBtn).Click();
            }

            // close add item modal
            var _closeModal = By.XPath("//button[@class='close']");
            _driver.WaitForVisibilityAndFindTheElement(_closeModal).Click();
        }

        private string GetOrderNumber()
        {
            var liOrder = By.CssSelector("ul[class='breadcrumb breadcrumb-top'] > li:nth-child(3)");
            return _driver.WaitForVisibilityAndFindTheElement(liOrder).Text;
        }
    }
}
