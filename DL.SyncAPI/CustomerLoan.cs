using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DL.DAL;
using DL.DAL.DBHelpers;
using DL.Shared.Models;
using DL.SyncAPI.Helpers;
using DL.SyncAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace DL.SyncAPI
{
    public class CustomerLoan
    {
        private readonly ILogger<CustomerLoan> _logger;
        private readonly DBOps _dbOps;
        public CustomerLoan(ILogger<CustomerLoan> log, DBOps dbops)
        {
            _logger = log;
            _dbOps = dbops;
        }

        [FunctionName("AddCustomerLoan")]
        [OpenApiOperation(operationId: nameof(CustomersLoan), tags: new[] { "Customers" }, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", typeof(Customer), Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]

        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CustomersLoan")]
        HttpRequestMessage req,
        ILogger log)
        {
            var response = new ContentResult { ContentType = "application/json", StatusCode = 200 };

            try
            {
                string body = req.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Customer>(body);
                CustomerLoanProcessor customerLoanProcessor = new CustomerLoanProcessor(_dbOps);
                response.Content = await customerLoanProcessor.ProcessCustomerLoanProcessor(result, log);

                return new OkObjectResult(response);
            }
            catch (Exception e)
            {
                log.LogInformation(e.ToString());
                return new BadRequestObjectResult("It went wrong");
            }
        }

        [FunctionName("GetAllCustomersLoan")]
        [OpenApiOperation(operationId: nameof(CustomersLoan), tags: new[] { "Customers" }, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        ////[OpenApiRequestBody(contentType: "application/json", typeof(Customers), Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Customer>))]

        public async Task<IEnumerable<Customer>> GetAllCustomersLoan(
       [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetAllCustomersLoan")] HttpRequestMessage req,
       ILogger log)
        {
            try
            {
                CustomerLoanProcessor customerLoanProcessor = new(_dbOps);
                return await customerLoanProcessor.GetAllCustomersLoan(log);
            }
            catch (Exception e)
            {
                log.LogInformation(e.ToString());
                throw;
            }
        }
    }



}


