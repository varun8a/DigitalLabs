using DL.Shared.Models;
using System.Net.Http.Headers;
using System.Text;

namespace DL.Shared.Helpers
{
    public static class Helper
    {
        /// <summary>
        /// Validate Customer Model
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool ValidateModel(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentException("Bad Request");
            }
            if (customer.CustomerSSN.ToString().Length != 11)
            {
                throw new ArgumentException("CustomerSSN should be of 11 digit.");
            }
            if (string.IsNullOrEmpty(customer.FullName))
            {
                throw new ArgumentException("Customer Name is Required.");
            }
            return true;
        }

        /// <summary>
        /// Validate Basic Auth
        /// </summary>
        /// <param name="header"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidateToken(AuthenticationHeaderValue header, string userName, string password)
        {
            //Checking the header
            if (header != null && header.Parameter != null && header.Scheme.Equals("Basic"))
            {
                //Decoding Base64
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(header.Parameter));
                //Splitting Username:Password
                int seperatorIndex = usernamePassword.IndexOf(':');
                // Extracting the individual username and password
                var username = usernamePassword.Substring(0, seperatorIndex);
                var pass = usernamePassword.Substring(seperatorIndex + 1);
                //Validating the credentials
                if (username.Equals(userName) && pass.Equals(password)) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
    }
}
