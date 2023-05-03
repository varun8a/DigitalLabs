using DL.DAL;
using DL.DAL.DBHelpers;
using DL.Shared.Helpers;
using DL.Shared.Models;
using DL.SyncAPI.Helpers;
using DL.SyncAPI.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
namespace DL.SyncAPI
{
    public class CustomerLoan
    {
        private DBOps _dbOps;
        private readonly TelemetryClient _telemetry;
        private readonly AppConfig _config;
        public CustomerLoan(DBOps dbops, TelemetryClient telemetry, IOptionsSnapshot<AppConfig> config)
        {
            _dbOps = dbops;
            _telemetry = telemetry;
            _config = config?.Value ?? null;
        }
        [FunctionName("AddCustomerLoan")]
        [OpenApiOperation(operationId: nameof(Customers), tags: new[] { "Customers" }, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiRequestBody(contentType: "application/json", typeof(Customer), Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> AddCustomerLoan(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CustomersLoan")]
        HttpRequestMessage req)
        {
            try
            {
                if (req != null && req.Headers != null && req.Content != null)
                {
                    string body = req.Content.ReadAsStringAsync().Result;
                    var headers = req.Headers.Authorization;
                    _telemetry.TrackEvent("Create Customer", new Dictionary<string, string> { { "Request", body } });
                    var response = new ContentResult { ContentType = "application/json", StatusCode = 200 };
                    var customer = JsonConvert.DeserializeObject<Customer>(body);
                    //config null for Test Request Header Exception
                    if (_config == null || Helper.ValidateToken(headers, _config.UserName, _config.Password))
                    {
                        if (Helper.ValidateModel(customer))
                        {
                            CustomerLoanProcessor customerLoanProcessor = new CustomerLoanProcessor(_dbOps);
                            response.Content = await customerLoanProcessor.ProcessCustomerLoanProcessor(customer);
                            _telemetry.TrackEvent("Create Customer", new Dictionary<string, string> { { "Response", response.Content } });
                        }
                        return new OkObjectResult(response);
                    }
                    else
                    {
                        return new UnauthorizedResult();
                    }
                }
                else
                {
                    return new BadRequestObjectResult("Bad Request");
                }
            }
            catch (ArgumentException ex)
            {
                _telemetry.TrackException(ex, new Dictionary<string, string> { { "Argument Exception in Create", ex.Message } });
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex, new Dictionary<string, string> { { "Exception in Create", ex.Message } });
                return new BadRequestObjectResult("It went wrong");
            }
        }
        /// <summary>
        /// GetAllCustomers with Basic Auth
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [FunctionName("GetAllCustomersLoan")]
        [OpenApiOperation(operationId: nameof(Customers), tags: new[] { "Customers" }, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Customer>))]
        public async Task<IEnumerable<Customer>> GetAllCustomersLoan(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetAllCustomersLoan")] HttpRequestMessage req)
        {
            try
            {
                _telemetry.TrackEvent("Get All Customers " + _config.UserName, new Dictionary<string, string> { { "User", _config.UserName } });
                var headers = req.Headers.Authorization;
                if (_config == null || Helper.ValidateToken(headers, _config.UserName, _config.Password))
                {
                    CustomerLoanProcessor customerLoanProcessor = new(_dbOps);
                    var response = await customerLoanProcessor.GetAllCustomersLoan();
                    _telemetry.TrackEvent("Get All Customers", new Dictionary<string, string> { { "Response", JsonConvert.SerializeObject(response) } });
                    return response;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                _telemetry.TrackException(ex, new Dictionary<string, string> { { "Exception in Get All Customer", ex.Message } });
                throw;
            }
        }
    }
}
