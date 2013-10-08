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



namespace SeleniumTestSuitWaB.Pages
{
    public class MainPage : Page
    {
        private string _UserButton = "";
        public string UserButton
        {
            get { return _UserButton; }
        }

        private string _LogOutAddress = ""; //Или сделать статическим, 
                                            //что бы не создавать каждый раз объект ради выхода из сайта
        public string LogOutAddress
        {
            get { return _LogOutAddress; }
        }
        
        private string _MessageDialog = "";
        public string MessageDialog
        {
            get { return _MessageDialog; }
        }

        private string _CompanyName = "";
        public string CompanyName
        {
            get { return _CompanyName; }
        }

        public MainPage(IWebDriver driver) : base(driver)
        {
            _LogOutAddress = "" + "";
            _Address = "";
            _Name = "Домашняя старница сайта";
            _Title = "Стартовая страница";

            _UserButton = "";
            _MessageDialog = "";
            _CompanyName = "";
        }

        public void ExitPage()
        {
            base.ExitPage(_LogOutAddress);
        }
    }
}
