using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DL.Shared.Models;
using DL.WebApp.Utility;
using DL.WebApp.Models;

namespace DL.WebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IOptionsSnapshot<AppConfig> _config;

        public CustomersController(IOptionsSnapshot<AppConfig> config)
        {
            _config = config;
        }

        public ActionResult Index()
        {
            try
            {
                Customer customerModel = new Customer();
                //var loggedInUser = this.HttpContext.GetLoggedInUser();
                //if (!loggedInUser.IsAuthorizedUser)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetAllCustomers(int? page, int? limit, string sortBy, string direction, string searchText, string StopCode, string Description, string LangCode, string Roles, string Process)
        {
            CustomerHttpHelper customerHttpHelper = new CustomerHttpHelper(_config);
            IEnumerable<Customer> query;
            int total;
            var records = customerHttpHelper.GetAllCustomer().Result;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = records.Where(q => q.CustomerSSN.ToString().Contains(searchText)
                                    || q.FullName.Contains(searchText)
                                    || q.LoanAmount.ToString().Contains(searchText)
                                    || q.EquityAmount.ToString().Contains(searchText)
                                    || q.SalaryAmount.ToString().Contains(searchText));
            }
            else
            {
                query = new List<Customer>();
            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    switch (sortBy.Trim().ToLower())
                    {
                        case "customerssn":
                            query = query.OrderBy(q => q.CustomerSSN);
                            break;

                        case "fullname":
                            query = query.OrderBy(q => q.FullName);
                            break;

                        case "equityamount":
                            query = query.OrderBy(q => q.EquityAmount);
                            break;
                        case "salaryamount":
                            query = query.OrderBy(q => q.SalaryAmount);
                            break;
                        case "loanamount":
                            query = query.OrderBy(q => q.LoanAmount);
                            break;

                    }
                }
                else
                {
                    switch (sortBy.Trim().ToLower())
                    {
                        case "customerssn":
                            query = query.OrderByDescending(q => q.CustomerSSN);
                            break;

                        case "fullname":
                            query = query.OrderByDescending(q => q.FullName);
                            break;

                        case "equityamount":
                            query = query.OrderByDescending(q => q.EquityAmount);
                            break;
                        case "salaryamount":
                            query = query.OrderByDescending(q => q.SalaryAmount);
                            break;
                        case "loanamount":
                            query = query.OrderByDescending(q => q.LoanAmount);
                            break;

                    }
                }
            }
            else
            {
                query = query.OrderBy(q => q.CustomerSSN);
            }

            total = query.Count();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                query = query.Skip(start).Take(limit.Value);
            }

            return this.Json(new { query, total });
        }
    }
}
