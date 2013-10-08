using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SeleniumTestSuitWaB
{
    public class Bank : IData
    {
        public string Name = "";
        public string Bic = "";
        public string City = "";
        public string Mail = "";
        public string Phone = "";

        private static uint _Count = 0;
        public static uint Count
        {
            get
            {
                return _Count;
            }
        }

        public Bank(string Name, string Bic, string City, string Mail, string Phone)
        {
            SetData(Name, Bic, City, Mail, Phone);

            ++_Count;
        }

        public void SetData(string Name, string Bic, string City, string Mail, string Phone)
        {
            this.Name = Name;
            this.Bic = Bic;
            this.City = City;
            this.Mail = Mail;
            this.Phone = Phone;
        }

        protected void MakeDefault()
        {
            SetData("Банк " + _Count.ToString(), "МФО-БИК " + _Count.ToString(), "Киев ",
                    "Почтовый адрес: ", "111-11-11");
        }

        ~Bank()
        {
            //_Count всегда больше или равно 0 (uint)
            /*if(_Count != 0)
                --_Count;*/
        }
    }
}
