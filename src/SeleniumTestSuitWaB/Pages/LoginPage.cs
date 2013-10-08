#region C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

#region UnitTest
using NUnit.Framework;
#endregion

#region Selenium
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.PageObjects;
#region Selenium Browsers
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Android;
#endregion
#endregion

#region Code
using SeleniumTestSuitWaB.Tests;
#endregion

namespace SeleniumTestSuitWaB.Pages
{
    public class LoginPage : Page
    {
        //public string PasswordEdit = "Password"; 
        [FindsBy(How = How.Id, Using = "")]
        [CacheLookup]
        private IWebElement PasswordEdit;

        [FindsBy(How = How.Id, Using = "")]
        [CacheLookup]
        private IWebElement LoginEdit;

        [FindsBy(How = How.Id, Using = "")]
        [CacheLookup]
        private IWebElement EnterButton;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            _Address = "";
            _Name = "Страница Логина";
            _Title = "Вход в систему";
            PageFactory.InitElements(driver, this);
        }

        //Fluent interfase
        //Не Page, так как Page - не итерфейс, а абстрактный класс
        public LoginPage CreatePage(string UserFullName, string UserPassword)
        {
            LoadPage();
            CheckPage();

            LoginEdit.Clear();
            LoginEdit.SendKeys(UserFullName);
            PasswordEdit.Clear();
            PasswordEdit.SendKeys(UserPassword);
            EnterButton.Click();

            return this;
        }

        protected override void CheckParentPage()
        {
            var mp = new MainPage(driver);
            if (driver.Url != mp.Address)
            {
                Assert.Fail("Не выполнен вход на сайт.");
            }
        }

        protected override void CheckErrors()
        {
            //Проврека на клиентскую валидацию
            if ((driver.Url == Address) && (driver.Title == Title))
            {
                if (ATest.IsColorError(LoginEdit))
                {
                    Assert.Fail("Не заполнено поле логина.");
                }
                else if (ATest.IsColorError(PasswordEdit))
                {
                    Assert.Fail("Не заполнено поле пароля.");
                }
            }
        }
    }
}
