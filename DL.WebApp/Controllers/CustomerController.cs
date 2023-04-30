using DL.WebApp.Models;
using DL.WebApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DL.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IOptionsSnapshot<AppConfig> _config;

        public CustomerController(IOptionsSnapshot<AppConfig> config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            CustomerHttpHelper customerHttpHelper = new CustomerHttpHelper(_config);
            return View(customerHttpHelper.GetAllCustomer().Result);
        }
    }
}
