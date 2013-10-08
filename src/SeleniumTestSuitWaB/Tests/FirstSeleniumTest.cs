#region C#
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
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
using SeleniumTestSuitWaB.Pages;
using SeleniumTestSuitWaB.Users;
#endregion

/*Создать класс для юзеров не синглетон
 * Создать класс для переменных  константнызх
 */

/*Заюзать патерн для разделения логики и реализации
 * PageObject
 * PageFactory
 */

//TODO
//Один тип ользователей
//Всегда работат ьсо страницей через pAGE
//Считывать имена пользователей в тестах из файла

namespace SeleniumTestSuitWaB.Tests
{
    #region TestClasses
    [TestFixture, Category("Regression Tests")]
    public sealed class FirstSeleniumTest : ATest
    {
        #region Tests
        /// <summary>
        ///  Тестовый метод.
        ///  Проверяет корректный логин и логаут сайта.
        /// </summary>
        /// <returns>Пустое значение</returns>
        [Test]
        public void LogInLogOut([Values("", "")] string login, 
            [Values("")] string password)
        {
            TryWebDriver();
            
            globaluser.SetData(login, password);
            LogIn(globaluser);
            LogOut(globaluser.UserFullName);
        }
        #endregion
    }
    #endregion
}
