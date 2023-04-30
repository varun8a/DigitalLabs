using System.Configuration;
using System;
//using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace DL.WebApp.Utility
{
    public class AppSetting
    {
        public static string UrlCreateCustomer
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UrlCreateCustomer"];
            }
        }

        public static string URLGetCustomers
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["URLGetCustomers"];
            }
        }

        
    }
}
