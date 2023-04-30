using DL.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DL.Web.Utility
{
    public static class CustomerHttpHelper
    {
        /// <summary>
        /// Get Customers List using API call
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Customer>> GetAllCustomer()
        {
            IEnumerable<Customer> response;
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(10000);
                    var authData = string.Format("{0}:{1}", AppSetting.UserName, AppSetting.Password);
                    var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                    var responseFromApi =  client.GetAsync(AppSetting.URLGetCustomers).Result;
                    string datafromApi = await responseFromApi.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<IEnumerable<Customer>>(datafromApi);
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Customer using API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<string> CreateCustomer(string request)
        {
            String response;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.BaseAddress = new Uri(AppSetting.UrlCreateCustomer);
                    client.Timeout = TimeSpan.FromMilliseconds(10000);
                    var authData = string.Format("{0}:{1}", AppSetting.UserName, AppSetting.Password);
                    var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                    var stringContent = new StringContent(request, Encoding.UTF8, "application/json");
                    var responseFromApi = client.PostAsync(string.Empty, stringContent).Result;
                    string datafromApi = await responseFromApi.Content.ReadAsStringAsync();
                    response = datafromApi;
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}

