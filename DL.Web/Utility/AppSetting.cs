using System.Configuration;
using System;


namespace DL.Web.Utility
{
    public class AppSetting
    {
        public static string UrlCreateCustomer
        {
            get
            {
                return ConfigurationManager.AppSettings["UrlCreateCustomer"];
            }
        }

        public static string URLGetCustomers
        {
            get
            {
                return ConfigurationManager.AppSettings["URLGetCustomers"];
            }
        }

        public static string UserName
        {
            get
            {
                return ConfigurationManager.AppSettings["UserName"];
            }
        }

        public static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["Password"];
            }
        }


    }
}
