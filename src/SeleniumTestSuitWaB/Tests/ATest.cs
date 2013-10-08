#region C#
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
//using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
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
using System.Drawing.Imaging;
#endregion
#endregion

#region XMLCode
using System.Xml;
#endregion

#region Code
using SeleniumTestSuitWaB.Pages;
using SeleniumTestSuitWaB.Users;
#endregion

//TODO
//Можно main и Login страницы сделать глобальными, что бы не плодить объекты

namespace SeleniumTestSuitWaB.Tests
{
    public abstract class ATest
    {
        #region Data
        #region Selenium
        public enum DriverType { Chrome, FF, IE, Android, Safari, Opera, None};

        /*public class DR
        {

        }*/

        /// <summary>
        /// Типы тестов
        /// </summary>
        public enum TestStatus
        {
            /// <summary>
            /// The test was inconclusive
            /// </summary>
            Inconclusive = 0,

            /// <summary>
            /// The test has skipped
            /// </summary>
            Skipped = 1,

            /// <summary>
            /// The test succeeded
            /// </summary>
            Passed = 2,

            /// <summary>
            /// The test failed
            /// </summary>
            Failed = 3
        }

        /// <summary>
        ///  Указание на ошибку
        /// </summary>
        protected bool acceptNextAlert = false;

        /// <summary>
        ///  Драйвер браузера для теста
        /// </summary>
        protected IWebDriver Driver = null;
        
        protected StringBuilder verificationErrors = null;

        /// <summary>
        /// Время ожидания для каждого елемента
        /// </summary>
        protected IWait<IWebDriver> wait = null;

        /// <summary>
        /// Максимальный временной шаг для ожиждания  браузера
        /// </summary> 
        protected const double MaximalWebTimeStep = 15.0;

        /// <summary>
        /// Минимальный временной шаг для ожиждания  браузера
        /// </summary> 
        protected const double MinimalWebTimeStep = 5.0;

        /// <summary>
        /// Временной шаг для ожиждания  браузера
        /// </summary> 
        public static double WebTimeStep = MinimalWebTimeStep;
        #endregion

        #region Site
        /// <summary>
        ///  Адресс начальнойстраницы сайта
        /// </summary>
        protected string baseURL = "";

        /// <summary>
        /// Данные о пользователе
        /// </summary>
        protected UserData globaluser = UserData.MyInstance;

        /// <summary>
        /// Статический класс (не Одиночка)
        /// </summary>
        static public class SitePages
        {
            public static HashSet<string> WithSomeElementsPages = new HashSet<string>()
                {
                    ""
                };

            public static HashSet<Tuple<string, string>> TuplePages = new HashSet<Tuple<string, string>>()
            {
                new Tuple<string, string>("", "")
            };
        }
        #endregion
        #endregion

        #region InitDestroy
        /// <summary>
        ///  Метод для Первоначальной инициализации браузера для всех тестов
        /// </summary>
        /// <returns>Пустое значение</returns>
        [TestFixtureSetUp]
        virtual public void ClassInitialize()
        {
            //Иногда нужно не разлогинивтаься,
            //а запускать тесты вподряд под одной сессией
            
            //CloseDriver();
            NewDriver(DriverType.Chrome);
            //NewChromeDriver();
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WebTimeStep));

            TestCleanUp();

            //InitPages();
            MainPage mp = new MainPage(Driver);
            LoginPage lp = new LoginPage(Driver);

            baseURL = lp.Address;
            verificationErrors = new StringBuilder();
            acceptNextAlert = true;

            globaluser.SetData("", "");
        }

        /// <summary>
        ///  Метод для завершения работы браузера для всех тестов
        /// </summary>
        /// <returns>Пустое значение</returns>
        [TestFixtureTearDown]
        virtual public void ClassClean()
        {
            Driver.Close();
            Driver.Quit();
            Driver = null;
            wait = null;
            baseURL = "";
            globaluser.SetData();
            verificationErrors = null;
            acceptNextAlert = false;
            TestCleanUp();
            //WebTimeStep = MinimalWebTimeStep;
            //DestroyPages();
        }

        /// <summary>
        /// Уничтожить драйвер
        /// </summary>
        private void CloseDriver()
        {
            if (Driver != null)
            {
                Driver.Close();
                Driver.Quit();
                Driver = null;
            }
        }

        protected void NewDriver(DriverType dt)
        {
            //Нет проверки try catch finaly что бы избежать рекурсии 
            //при вызове браузер и начального метода инициализации
            CloseDriver();
            switch (dt)
            {
                case DriverType.Chrome:
                    Driver = new ChromeDriver();
                    break;
                case DriverType.IE:
                    Driver = new InternetExplorerDriver();
                    break;
                default:
                    Driver = new ChromeDriver();
                    break;
            }
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(WebTimeStep));
        }

        /// <summary>
        /// Создание IE драйвера
        /// </summary>
        protected void NewIEDriver()
        {
            //Нет проверки try catch finaly что бы избежать рекурсии 
            //при вызове браузер и начального метода инициализации
            CloseDriver();
            Driver = new InternetExplorerDriver();
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(WebTimeStep));
        }

        /// <summary>
        /// Создание нового драйвера для Chrome
        /// </summary>
        protected void NewChromeDriver()
        {
            //Нет проверки try catch finaly что бы избежать рекурсии 
            //при вызове браузер и начального метода инициализации
            CloseDriver();
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(WebTimeStep));
        }

        protected void NewFFDriver()
        {
            //Нет проверки try catch finaly что бы избежать рекурсии 
            //при вызове браузер и начального метода инициализации   
            CloseDriver();
            Driver = new FirefoxDriver();
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(WebTimeStep));
        }

        protected void NewOptimizeFFDriver()
        {
            //Нет проверки try catch finaly что бы избежать рекурсии 
            //при вызове браузер и начального метода инициализации
            var profile = new FirefoxProfile();
            profile.EnableNativeEvents = false;

            CloseDriver();
            Driver = new FirefoxDriver(profile);
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(WebTimeStep));
        }

        protected void NewAndroidDriver()
        {
            //Нет проверки try catch finaly что бы избежать рекурсии 
            //при вызове браузер и начального метода инициализации
            CloseDriver();
            Driver = new AndroidDriver();
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(WebTimeStep));
        }

        [SetUp]
        public void TestOpen()
        {
            //GoToMainPage();
            //LogIn(globaluser);
            TestCleanUp();
        }

        [TearDown]
        public void TestCleanUp()
        {
            //Driver.Manage().Cookies.DeleteAllCookies();
            MakeDefaultWait();
            //LogOut(globaluser.UserFullName);
        }
        #endregion

        #region SupportSelenium
        /// <summary>
        ///  Метод для проверки запущен ли драйвер
        /// </summary>
        /// <returns>Пустое значение</returns>
        protected void TryWebDriver()
        {
            if (Driver == null)
                Assert.Fail("Веб-драйвер не запущен.");
        }

        /// <summary>
        ///  Метод для проверки существует ли елемент на старнице
        /// </summary>
        /// <param name="by">Елемент который нужно найти</param>
        /// <returns>Возвращает логическое значение True (нашел) или False (не нашел)</returns>
        public static bool IsElementPresent(By by, IWebDriver Driver)
        {
            try
            {
                Driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        protected bool IsElementPresent(By by)
        {
            return IsElementPresent(by, Driver);
        }

        public static bool IsElementPresent(IWebElement el)
        {
            try
            {
                string str = el.Text;
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Проверка на присутствие alert
        /// </summary>
        /// <returns>True - alert есть, False - alert нет</returns>
        protected bool IsAlertPresent()
        {
            try
            {
                //IAlert Alert = null;
                Driver.SwitchTo().Alert().Accept();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        protected string getTextByJavascript(IWebElement element)
        {
            string script = "var element = arguments[0];"
                + "return element.textContent;";
            return (String)((IJavaScriptExecutor)Driver).ExecuteScript(script, element);
        }

        protected void RunJSScript(string script)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript(script);
        }

        protected string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = Driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }

        public static void WaitForElementPresent(By by, 
                                                 IWebDriver Driver, 
                                                 double MinimalWebTimeStep)
        {
            for (int second = 0; ; ++second)
            {
                if (second >= MinimalWebTimeStep) 
                    Assert.Fail("Ожидание элемента прекращено по тайм-ауту.\n" + 
                                "Страница " + Driver.Title + "n" + 
                                "Элемент " + by.ToString());
                try
                {
                    Driver.FindElement(by);
                    break;
                }
                catch(Exception)
                {
                    ;
                }
                Thread.Sleep(1000);
            }
        }

        protected void WaitForElementPresent(By by)
        {
            WaitForElementPresent(by, Driver, MinimalWebTimeStep);
        }

        public static void WaitForElementTextPresent(By by,
                                                     IWebDriver Driver,
                                                     double MinimalWebTimeStep,
                                                     string NotText = "")
        {
            for (int second = 0; ; ++second)
            {
                if (second >= MinimalWebTimeStep)
                    Assert.Fail("Ожидание текста элемента прекращено по тайм-ауту.\n" +
                                "Страница " + Driver.Title + "n" +
                                "Элемент " + by.ToString());
                try
                {
                    if(Driver.FindElement(by).Text != NotText)
                        break;
                }
                catch(Exception)
                {
                    ;
                }
                Thread.Sleep(1000);
            }
        }

        protected void WaitForElementTextPresent(By by, string NotText = "")
        {
            WaitForElementTextPresent(by, Driver, MinimalWebTimeStep, NotText);
        }

        private bool TryFindElement(By by, out IWebElement element)
        {
            /// How to use
            /*IWebElement NewElement = null;
            if (TryFindElement(By.CssSelector("div.logintextbox"), out element)
            {
                bool visible = IsElementVisible(element);
                if  (visible)
                {
                    // do something
                }
            }*/
              
            try
            {
                element = Driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                element = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Видим ли элемент
        /// </summary>
        /// <param name="element">Элемент для проверки</param>
        /// <returns>True - если видим и активен, False - если не видим или не активен</returns>
        public static bool IsElementVisible(IWebElement element)
        {
            return element.Displayed && element.Enabled;
        }

        /// <summary>
        /// Нажать клавишу с помощью JS
        /// </summary>
        /// <param name="command">Конкретный элемент для нажатия</param>
        public static void JSClickButton(string command, IWebDriver Driver)
        {
            string js = "$('" + command + "').trigger('mouseenter').trigger('click');";
            ((IJavaScriptExecutor)Driver).ExecuteScript(js);
        }

        protected void JSClickButton(string command)
        {
            JSClickButton(command, Driver);
        }

        public static void JSClick(string element, IWebDriver Driver)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
            executor.ExecuteScript("arguments[0].click();", element);
            /*Actions builder = new Actions(driver);
            builder.MoveToElement(GridCountPageElementsButton).Click(GridCountPageElementsButton);
            builder.Perform();*/
        }

        /// <summary>
        /// Есть ли ошибки в виде красного подсвечивания поля
        /// </summary>
        /// <param name="id">Элемент для проверки</param>
        /// <returns>True - есть ошибки
        /// False - нет ошибок</returns>
        public static bool IsColorError(IWebElement color)
        {
            string str = color.GetAttribute("style");
            List<string> list = new List<string>()
                                        {"red",
                                         "RED",
                                         "#DE3914"
                                        };
            int error = -1;

            foreach (var s in list)
            {
                error = str.IndexOf(s);
                if (error >= 0)
                    break;
            }

            if (error < 0)
                return false;
            else
                return true;

            /*((IJavaScriptExecutor)Driver).ExecuteScript(
                "window.getComputedStyle(
                window.document.getElementById('p256-input')
                ).getPropertyValue('outline');"
                ));*/

            return true;
        }

        /// <summary>
        /// POST запрос
        /// </summary>
        /// <param name="Url">Адрес сервиса</param>
        /// <param name="Data">Параметры для отправки</param>
        /// <returns>Ответ сервера</returns>
        private static string POST(string Url, string Data)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            req.Method = "POST";
            req.Timeout = 100000;
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] sentData = Encoding.GetEncoding(1251).GetBytes(Data);
            req.ContentLength = sentData.Length;
            System.IO.Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            System.Net.WebResponse res = req.GetResponse();
            System.IO.Stream ReceiveStream = res.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);
            //Кодировка указывается в зависимости от кодировки ответа сервера
            Char[] read = new Char[256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0)
            {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }
            return Out;
        }

        protected bool IsAlertPresentMax()
        {
            bool presentFlag = false;

            try
            {
                // Check the presence of alert
                IAlert alert = Driver.SwitchTo().Alert();
                // Alert present; set the flag
                presentFlag = true;
                // if present consume the alert
                alert.Accept();
            }
            catch (NoAlertPresentException ex)
            {
                // Alert not present
                ex.Message.ToString();
            }

            return presentFlag;
        }

        /// <summary>
        /// Сделать скриншот страницы
        /// </summary>
        protected void MakeScreenShot()
        {
            string strName = "ScreenShot " + 
                DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() +
                "(" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + ")";

            Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
            string screenShot = ss.AsBase64EncodedString;
            byte[] screenShotAsByteArray = ss.AsByteArray;
            ss.SaveAsFile(strName, ImageFormat.Png);
        }

        /// <summary>
        /// Написать письмо
        /// </summary>
        protected void CreateEmailLetter(string host, 
            string login, string password, string to = "testwab-str@mail.ru", 
            int port = 25)
        {
            //Авторизация на SMTP сервере
            SmtpClient Smtp = new SmtpClient(host, port);
            Smtp.Credentials = new NetworkCredential(login, password);
            //Smtp.EnableSsl = false;

            //Формирование письма
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress(login);
            Message.To.Add(new MailAddress(to));
            Message.Subject = "Заголовок";
            Message.Body = "Test";

            /*//Прикрепляем файл
            string file = "C:\\file.zip";
            Attachment attach = new Attachment(file, MediaTypeNames.Application.Octet);
            // Добавляем информацию для файла
            ContentDisposition disposition = attach.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);

            Message.Attachments.Add(attach);*/

            Smtp.Send(Message);//отправка
        }

        /// <summary>
        /// Загрузка/Импорт Excel файла
        /// </summary>
        /// <param name="id">Id элемента в который загружается файл</param>
        /// <param name="file">Имя файла для импорта</param>
        protected void DoImport(By by, string file)
        {
            //Driver.FindElement(by).Clear();
            Driver.FindElement(by).SendKeys(@file);
        }

        /// <summary>
        /// Выделить имя файла из пути
        /// </summary>
        /// <param name="file">Путь с именем файла</param>
        protected string DoNameOfFileFromAddress(string file)
        {
            const string strV = @".*/([^/]+$)";

            //make path without name of file
            string filename = Regex.Match(file, strV).Groups[1].Value;
            //ReplaceText(ref filename, file, "");

            return filename;
        }

        protected void GetFileName(string name)
        {
            Path.GetFileName(name);
        }

        /// <summary>
        /// Заменить текст
        /// </summary>
        /// <param name="input">Строка в котрую поместить новое значение</param>
        /// <param name="pattern">В чем заменить</param>
        /// <param name="replacement">На что заменить</param>
        protected void ReplaceText(ref string input, string pattern, string replacement)
        {
            try
            {
                Regex rgx = new Regex(pattern);
                input = rgx.Replace(input, replacement);
            }
            catch (Exception err)
            {
                Assert.Fail("Не возможно произвести замену в файле.\n" + err.Message);
            }
        }

        private void MakeMinimalWait()
        {
            WebTimeStep = MinimalWebTimeStep;
        }

        private void MakeMaximalWait()
        {
            WebTimeStep = MaximalWebTimeStep;
        }

        private void MakeDefaultWait()
        {
            MakeMinimalWait();
        }

        /// <summary>
        /// Подождать X секунд
        /// </summary>
        /// <param name="Time">Количестов секунд</param>
        public static void WaitSomeTime(double Time = MinimalWebTimeStep)
        {
            Thread.Sleep((int)(Time * 1000));
        }

        protected string FindMetaTag(string name)
        {
            string result = "";
            try
            {
                string XPathName =  "//meta[@name='" + name + "']";
                result = Driver.FindElement((By.XPath(XPathName))).GetAttribute("content");
            }
            catch (Exception err)
            {
                result = "";
                Assert.Fail("Не удалось найти указанный мэта-тег на странице " + Driver.Title);
            }
            return result;
        }

        public static bool FindAny<T>(IEnumerable<T> TSpace, T match) where T : IEqualityComparer<T>
        {
            var matchFound = false;
            Parallel.ForEach(TSpace,
            (curValue, loopstate) =>
            {
                if (curValue.Equals(match))
                {
                    matchFound = true;
                    loopstate.Stop();
                }
            });
            return matchFound;
        }
        #endregion

        #region SupportSite
        /// <summary>
        ///  Метод для входа на сайт под указанным логином и паролем
        /// </summary>
        /// <param name="LogIn">Логин пользователя</param>
        /// <param name="Password">Пароль пользователя</param>
        /// <returns>Пустое значение</returns>
        protected void LogIn(UserData user) //string LogIn, string Password
        {
            GoToMainPage();

            if (IsIn(user.UserFullName))
                Assert.Fail("Вы уже залогинены."); //LogOut(user.UserFullName);

            LoginPage lp = new LoginPage(Driver);
            MainPage mp = new MainPage(Driver);
            string name = "";
            string firm = "";
            
            /*Assert.IsTrue(IsElementPresent(By.Id(lp.LoginEdit)), "Не найдено меню для входа.");
            Driver.FindElement(By.Id(lp.LoginEdit)).Clear();
            Driver.FindElement(By.Id(lp.LoginEdit)).SendKeys(user.UserFullName);*/
            try
            {
                lp.CreatePage(user.UserFullName, user.UserPassword);
            }
            catch (Exception e)
            {
                Assert.Fail("Не найдена форма входа.\n" + e.Message);
            }

            Assert.AreEqual(mp.Title, Driver.Title, "Не удалось войти на сайт.");
            try
            {
                if (IsElementPresent(By.CssSelector(mp.MessageDialog)))
                    Driver.FindElement(By.CssSelector(mp.MessageDialog)).Click();
            }
            catch (Exception)
            {
                ;
            }
            Assert.IsTrue(IsElementPresent(By.LinkText(user.UserFullName)), "Не совпало имя пользователя после входа.");
            Assert.AreEqual(firm.ToUpper(), Driver.FindElement(By.CssSelector(mp.CompanyName)).Text,
                "Название компании не найдено или не совпадает с указанным.");
        }

        /// <summary>
        ///  Метод для проверки того, залогинены ли мы на данный момент под данным пользователем
        ///  - Если передавать дефолтный параметр то метод просто проверит залогинены ли мы
        ///  - Если передавать не дефолтный параметр то метод проверит залогинены ли мы 
        ///  на данный момент под данным пользователем
        /// </summary>
        /// <param name="LogIn">Полное имя пользователя (Логин) = "" (по умолчанию)</param>
        /// <returns>Возвращае True если залогинены, возвращает False если не залогинены 
        /// (с учтом конкретного пользователя и любого пользователя)</returns>
        protected bool IsIn(string LogIn = "")
        {
            //GoToMainPage();
            LoginPage lp = new LoginPage(Driver);
            MainPage mp = new MainPage(Driver);
            if (Driver.Title == lp.Title)
                return false;
            if (!IsElementPresent(By.ClassName(mp.UserButton)))
                return false;

            if (LogIn == "")
                return true;
            else
            {
                if (IsElementPresent(By.LinkText(LogIn)))
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        ///  Метод для выхода из сайта
        ///  Есть возможность указывать конкретного опльзователя для выхода 
        ///  либо прост овыйти независимо от пользователя
        /// </summary>
        /// <param name="LogIn">Логин пользователя (не обязательный парметр, значени по умолчанию = "")</param>
        /// <returns>Пустое значение</returns>
        protected void LogOut(string LogIn = "")
        {
            MainPage mp = new MainPage(Driver);
            LoginPage lp = new LoginPage(Driver);

            if (IsIn(LogIn))
            {
                //TODO: сделать выход по нажатию на кнопку
                //GoToMainPage();
                Assert.IsTrue(IsElementPresent(By.ClassName(mp.UserButton)), "Не найдено меню для выхода.");

                mp.ExitPage();
                Assert.AreEqual(lp.Title, Driver.Title, "Не удалось выйти из сайта.");
            }
            else
                Assert.Fail("Не возможно выйти из аккаунта. Возможно Вы не залогинены.");
        }

        /// <summary>
        ///  Метод для пеехода к конкретному url
        /// </summary>
        /// <param name="url">Адресс страницы на которую нужно перейти</param>
        /// <returns>Пустое значение</returns>
        public static void GoToPage(string url, IWebDriver Driver)
        {
            Driver.Navigate().GoToUrl(url);
        }

        protected void GoToPage(string url)
        {
            GoToPage(url, Driver);
        }

        /// <summary>
        ///  Метод для пеехода к главной странице сайта url
        ///  Нет разницы между страницей Логина и Заглавной страницей
        /// </summary>
        /// <returns>Пустое значение</returns>
        protected void GoToMainPage()
        {
            GoToPage(baseURL);
            if (Driver.Title != "Вход в систему" && Driver.Title != "Стартовая страница")
                Assert.Fail("Не удалось зайти на главную страницу.");
        }

        /// <summary>
        /// Проверить, что пользователь администратор
        /// </summary>
        protected bool IsAdmin()
        {
            return IsElementPresent(By.LinkText(""));
        }
        #endregion
    }
}
