using DL.Web.Models;
using DL.Web.Utility;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DL.WebApp.Controllers
{
    public class CustomersController : Controller
    {
        public TelemetryClient _telemetry = new TelemetryClient();

        /// <summary>
        /// Index Page for List
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                _telemetry.TrackEvent("Customer");
                Customer customerModel = new Customer();
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex,
                           new Dictionary<string, string> { { "Customers Index", ex.Message }, { "Customers Index", ex.InnerException.ToString() } },
                           new Dictionary<string, double> {
                       {"time",  DateTime.Now.ToOADate()},
                          });
                return RedirectToAction("Index", "Error");
            }
            return View();
        }
        /// <summary>
        /// Get Customers list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <param name="searchText"></param>
        /// <param name="CustomerSSN"></param>
        /// <param name="FullName"></param>
        /// <param name="LoanAmount"></param>
        /// <param name="EquityAmount"></param>
        /// <param name="SalaryAmount"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllCustomers(int? page, int? limit, string sortBy, string direction, string searchText, string CustomerSSN, string FullName, string LoanAmount, string EquityAmount, string SalaryAmount)
        {
            try
            {
                _telemetry.TrackEvent("GetAllCustomers");
                List<Customer> records;
                int total;
                var query = CustomerHttpHelper.GetAllCustomer().Result;

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(q => q.CustomerSSN.ToString().Contains(searchText)
                                        || q.FullName.Contains(searchText)
                                        || q.LoanAmount.ToString().Contains(searchText)
                                        || q.EquityAmount.ToString().Contains(searchText)
                                        || q.SalaryAmount.ToString().Contains(searchText));
                }

                if (!string.IsNullOrWhiteSpace(CustomerSSN))
                {
                    query = query.Where(q => q.CustomerSSN.ToString().Contains(CustomerSSN));
                }
                if (!string.IsNullOrWhiteSpace(FullName))
                {
                    query = query.Where(q => q.FullName.Contains(FullName));
                }
                if (!string.IsNullOrWhiteSpace(EquityAmount))
                {
                    query = query.Where(q => q.EquityAmount.ToString().Contains(EquityAmount));
                }
                if (!string.IsNullOrWhiteSpace(SalaryAmount))
                {
                    query = query.Where(q => q.EquityAmount.ToString().Contains(EquityAmount));
                }
                if (!string.IsNullOrWhiteSpace(LoanAmount))
                {
                    query = query.Where(q => q.LoanAmount.ToString().Contains(LoanAmount));
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
                    records = query.Skip(start).Take(limit.Value).ToList();
                }
                else
                {
                    records = query.ToList();
                }
                _telemetry.TrackEvent("Customer", new Dictionary<string, string> { { "Response", JsonConvert.SerializeObject(records) } });
                return this.Json(new { records, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex,
                           new Dictionary<string, string> { { "Customers GetAllCustomers", ex.Message }, { "Customers GetAllCustomers", ex.InnerException.ToString() } },
                           new Dictionary<string, double> {
                       {"time",  DateTime.Now.ToOADate()},
                          });
                return Json(new { result = false, exMessage = ex.GetBaseException().ToString() });

            }
        }

        /// <summary>
        /// Create New Customer
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddNewCustomer(Customer record)
        {
            try
            {
                var request = JsonConvert.SerializeObject(record);
                _telemetry.TrackEvent("Customers: Add New Customer", new Dictionary<string, string> { { "Request", JsonConvert.SerializeObject(request) } });
                string response = CustomerHttpHelper.CreateCustomer(request).Result;
                _telemetry.TrackEvent("Customers: Add New Customer", new Dictionary<string, string> { { "Response", response } });
                if (response.Contains(record.CustomerSSN.ToString()))
                {
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false });
                }
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex,
                           new Dictionary<string, string> { { "Customers: Add New Customer", ex.Message }, { "Customers GetAllCustomers", ex.InnerException.ToString() } },
                           new Dictionary<string, double> {
                       {"time",  DateTime.Now.ToOADate()},
                          });
                return Json(new { result = false });
            }
        }
    }
}
