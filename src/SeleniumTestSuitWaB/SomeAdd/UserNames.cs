#region C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
#endregion

#region UnitTest
using NUnit.Framework;
#endregion

#region XMLCode
using System.Xml;
#endregion

namespace SeleniumTestSuitWaB
{
    /// <summary>
    /// Класс для хранения аттирибутов пользователя (имя, отчество, фамилия)
    /// </summary>
    public class User
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name = "";

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        public string SurName = "";

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string MiddleName = "";

        /// <summary>
        /// EMail пользователя
        /// </summary>
        public string EMail = "";

        /// <summary>
        /// EMail Password пользователя
        /// </summary>
        public string PasswordEmail = "";

        /// <summary>
        /// Фирма Password пользователя
        /// </summary>
        public string Firm;

        /// <summary>
        /// Пароль от почты
        /// </summary>
        public string RealPassword = "";
    }

    /// <summary>
    /// Класс для хранения сприска возможных пользователй
    /// </summary>
    public class UserNames
    {
        /// <summary>
        /// Список пользователей
        /// </summary>
        public List<User> users = new List<User>();

        /// <summary>
        /// Атрибут для идентификации Имени в XML
        /// </summary>
        private readonly string uname = "Name";

        /// <summary>
        /// Атрибут для идентификации email в XML
        /// </summary>
        private readonly string email = "EMail";

        /// <summary>
        /// Атрибут для идентификации Отчества в XML
        /// </summary>
        private readonly string middlename = "MiddleName";

        /// <summary>
        /// Атрибут для идентификации Фамилии в XML
        /// </summary>
        private readonly string surname = "SurName";

        /// <summary>
        /// Атрибут для идентификации password email в XML
        /// </summary>
        private readonly string pswde = "EMailPassword";

        /// <summary>
        /// Атрибут для идентификации начала данных в XML
        /// </summary>
        private readonly string fileheader = "/Users_Table/User";

        /// <summary>
        /// Атрибут для установки пути к XML-файлу
        /// </summary>
        private string xmlfilename = @"../../usernamesfile.xml";

        /// <summary>
        /// Атрибут для идентификации Фирмы в XML
        /// </summary>
        private string firm = "Firm";

        /// <summary>
        /// Атрибут для идентификации пароля к почте
        /// </summary>
        private string realpassword = "RealPassword";

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UserNames(string filename = "")
        {
            ReadFile(filename);
        }

        /// <summary>
        /// Метод для парсинга XML файла с данными и заполнения списка пользователей
        /// </summary>
        public void ReadFile(string localXmlfilename = "")
        {   
            try
            {
                XmlDocument rdr = new XmlDocument();
                if(localXmlfilename != "")
                    rdr.Load(localXmlfilename);
                else
                    rdr.Load(xmlfilename); // Загрузка XML

                XmlNodeList xnList = rdr.SelectNodes(fileheader);
                foreach (XmlNode xmluser in xnList)
                {
                    User user = new User();

                    user.Name = xmluser[uname].InnerText;
                    user.MiddleName = xmluser[middlename].InnerText;
                    user.SurName = xmluser[surname].InnerText;
                    user.EMail = xmluser[email].InnerText;
                    user.PasswordEmail = xmluser[pswde].InnerText;
                    user.Firm = xmluser[firm].InnerText;
                    user.RealPassword = xmluser[realpassword].InnerText;

                    users.Add(user);
                }
            }
            catch (Exception err)
            {
                Assert.Fail("Проблемы с парсингом файла имен пользоватлей.\n" + err.Message);
            }
        }
    }
}
