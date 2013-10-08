
#region C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

#region Seelnium
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Android;
#endregion

#region XMLCode
using System.Xml;
#endregion

#region UnitTest
using NUnit.Framework;
#endregion

namespace SeleniumTestSuitWaB.Users
{
    //TODO
    //Обдумать вариант использования одного класса для пользовтелей:
    //Пользователь для логина
    //Пользователь для заполнения данных формы
    /// <summary>
    /// Класс для задания аттрибутов пользователя системой (логин, пароль)
    /// Реализован как паттерн Одиночка
    /// </summary>
    public sealed class UserData
    {
        /// <summary>
        /// Аттрибут для возвращения всегда однного и того же объекта
        /// </summary>
        static readonly UserData myInstance = new UserData();

        /// <summary>
        /// Закрытый статический конструктор 
        /// для обеспечения существоания всегда только одного представителя класса
        /// </summary>
        static UserData()
        {
            ;
        }

        /// <summary>
        /// Закрытый конструктор 
        /// для обеспечения существоания всегда только одного представителя класса
        /// Производит создание и начальную инициализацию объекта
        /// </summary>
        UserData()
        {
            SetData();
        }

        /// <summary>
        /// Метод для возврата всегда только одного прдетсавителя класса
        /// </summary>
        public static UserData MyInstance
        {
            get
            {
                return myInstance;
            }
        }

        /// <summary>
        /// Метод для утсановки логина и пароля
        /// </summary>
        public void SetData(string login = "", string password = "", 
            string realpswd = "", string email = "")
        {
            userFullName = login;
            userPassword = password;
            userRealPassword = realpswd;
            userEmail = email;
        }

        /// <summary>
        /// Атрибут для задания логина пользователя (полный логин)
        /// </summary>
        private string userFullName;

        /// <summary>
        /// Метод для доступа к логину пользователя извне
        /// </summary>
        public string UserFullName
        {
            get { return userFullName; }
            set { userFullName = value; }
        }

        /// <summary>
        /// Аттрибут для задания пароля пользователя
        /// </summary>
        private string userPassword;

        /// <summary>
        /// Метод для доступа к паролю пользователя внешними сущностями
        /// </summary>
        public string UserPassword
        {
            get { return userPassword; }
            set { userPassword = value; }
        }

        /// <summary>
        /// Атрибут для задания пароля к почте
        /// </summary>
        private string userRealPassword;

        /// <summary>
        /// Метод для доступа к паролю от почты
        /// </summary>
        public string UserRealPassword
        {
            get { return userRealPassword; }
            set { userRealPassword = value; }
        }

        /// <summary>
        /// Атрибут для почты
        /// </summary>
        private string userEmail;

        /// <summary>
        /// Метод для доступа к почте
        /// </summary>
        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        public void ReadUserData(string file)
        {
            try
            {
                XmlDocument rdr = new XmlDocument();
                string fileheader = "/User";

                if (file == "")
                    Assert.Fail("Вы ввели пустое имя файла для загрузки имени пользователя.");
                try
                {
                    rdr.Load(@file);
                }
                catch (Exception e)
                {
                    Assert.Fail("Не удается загрузить файл  с именем пользователя\n" + e.Message);
                }

                XmlNode xmlData = rdr.SelectSingleNode(fileheader); //OR rdr.SelectNodes для несколкьих
                string name = xmlData["Login"].InnerText;
                string password = xmlData["Password"].InnerText;

                SetData(name, password);
            }
            catch (Exception e)
            {
                Assert.Fail("Проблемы с парсингом файла имени пользователя.\n" + e.Message);
            }
        }
    };
}
