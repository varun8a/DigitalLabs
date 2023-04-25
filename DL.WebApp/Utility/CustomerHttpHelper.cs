using System.Net.Http.Headers;
using System.Text;
using System;
using Newtonsoft.Json;
using DL.Shared.Models;
using DL.Shared.Helpers;

namespace DL.WebApp.Utility
{
    public static class CustomerHttpHelper
    {
        public static async Task<IEnumerable<Customer>> GetAllCustomer()
        {
            string datafromApi = string.Empty;
            IEnumerable<Customer> response;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.BaseAddress = new Uri(AppSetting.URLGetCustomers);
                    client.Timeout = TimeSpan.FromMilliseconds(20);
                    //var authData = string.Format("{0}:{1}", AppSetting.MeapUserNamePH, AppSetting.MeapUserPasswordPH);
                    //var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                    //var stringContent = new StringContent(request, Encoding.UTF8, Constants.ApplicationJson);
                    //var responseFromPH = await client.PostAsync(string.Empty, stringContent);
                    var responseFromApi = await client.GetAsync("");

                    datafromApi = await responseFromApi.Content.ReadAsStringAsync();


                    response = JsonConvert.DeserializeObject<IEnumerable<Customer>>(datafromApi);


                    return response;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> CreateCustomer(string request)
        {
            string datafromApi = string.Empty;
            String response;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.BaseAddress = new Uri(AppSetting.UrlCreateCustomer);
                    client.Timeout = TimeSpan.FromMilliseconds(20);
                    //var authData = string.Format("{0}:{1}", AppSetting.MeapUserNamePH, AppSetting.MeapUserPasswordPH);
                    //var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                    var stringContent = new StringContent(request, Encoding.UTF8, Constants.ApplicationJson);
                    var responseFromApi = await client.PostAsync(string.Empty, stringContent);
                    datafromApi = await responseFromApi.Content.ReadAsStringAsync();
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

