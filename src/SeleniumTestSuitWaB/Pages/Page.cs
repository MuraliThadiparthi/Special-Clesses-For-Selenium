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
    public abstract class Page
    {
        //HardCode
        //Запишем название всех элементов
        //TODO
        //Использовать FindElement в конмтрукторе и выдавать ассерт Page если не найдено
        protected IWebDriver driver;

        protected string _Name = "";
        public string Name
        {
            get { return _Name; }
        }

        protected string _Address = "";
        public string Address
        {
            get { return _Address; }
        }

        protected string _Title = "";
        public string Title
        {
            get { return _Title; }
        }
        
        private const string MessageDialog = "div.message";

        protected Page(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected void Error(string addr)
        {
            Assert.Fail("Не удалось пропарсерить страницу " + addr);
        }

        protected void CheckPage()
        {
            if (driver.Url != _Address)
                ATest.GoToPage(_Address, driver);
            if (driver.Url != _Address)
                Assert.Fail("Не возможно перейти по указанному адресу (возможно, переадрессация).");
            if (driver.Title != _Title)
            {
                Assert.Fail("Не удалось войти на страницу " + _Title + "\n" +
                    "Текущая страница " + driver.Title);
            }
        }

        protected void InitFields<T>(T tmp)
        {
            Assert.Fail("Запущен метод абстрактного базового класса.");
        }

        protected virtual void ClearFields()
        {
            Assert.Fail("Запущен метод абстрактного базового класса.");
        }

        protected virtual void CheckParentPage()
        {
            Assert.Fail("Запущен метод абстрактного базового класса.");
        }

        protected virtual void CheckSave()
        {
            //Прроверка что данные сохранены
            ATest.WaitForElementTextPresent(By.CssSelector(MessageDialog), driver, ATest.WebTimeStep);
            string text = driver.FindElement(By.CssSelector(MessageDialog)).Text;
            if (text != "Данные успешно сохранены")
                Assert.Fail("Данные о " + this._Name + " не сохранены. " +
                            "Не выдано сообщение об успешном сохранении\n" +
                            "Получены данные: " +
                            text);
        }

        protected virtual void CheckErrors()
        {
            Assert.Fail("Запущен метод абстрактного базового класса.");
        }

        public virtual void LoadPage(bool ischeck = true)
        {
            if (driver == null)
                Assert.Fail("Не возможно загрузить конструктор. Драйвер не инициализирован.");
            if (driver.Url != this._Address)
                ATest.GoToPage(_Address, driver);
            if(ischeck == true)
                CheckPage();
        }

        public void ExitPage(string url)
        {
            ATest.GoToPage(url, driver);
        }
    }
}
