using DL.DAL;
using DL.Shared.Models;
using DL.SyncAPI;
using DL.SyncAPI.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Text;

namespace DL.SyncApi.Test
{
    public class CustomersTest
    {
        private Mock<DBOps> _dBOps;
        private TelemetryClient _telemetry;
        private IOptionsSnapshot<AppConfig> _config;

        [SetUp]
        public void Setup()
        {
            _dBOps = new Mock<DBOps>(MockBehavior.Strict);
            _telemetry = new TelemetryClient();
        }

        [Test]
        public void Test_Valid_Request()
        {
            var function = new CustomerLoan(_dBOps.Object, _telemetry, _config);
            var content = new StringContent("", Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Content = content
            };
            var actionResult = function.AddCustomerLoan(request).Result;
            var createdResult = actionResult as BadRequestObjectResult;
            Assert.IsNotNull(createdResult.Value);
            Assert.That(createdResult.Value, Is.EqualTo("Bad Request"));
        }

        [Test]
        public void Test_Valid_SSN()
        {
            var function = new CustomerLoan(_dBOps.Object, _telemetry, _config);

            var customer = new Customer()
            {
                CustomerSSN = 31212121,
                FullName = "Varun"
            };

            var json = JsonConvert.SerializeObject(customer);
            //construct content to send
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Content = content
            };
            var actionResult = function.AddCustomerLoan(request).Result;
            var createdResult = actionResult as BadRequestObjectResult;
            Assert.IsNotNull(createdResult.Value);
            Assert.That(createdResult.Value, Is.EqualTo("CustomerSSN should be of 11 digit."));
        }

        [Test]
        public void Test_Valid_FullName()
        {
            var function = new CustomerLoan(_dBOps.Object, _telemetry, _config);

            var customer = new Customer
            {
                CustomerSSN = 31078622121,
                FullName = ""
            };

            var json = JsonConvert.SerializeObject(customer);
            //construct content to send
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Content = content
            };
            var actionResult = function.AddCustomerLoan(request).Result;
            var createdResult = actionResult as BadRequestObjectResult;
            Assert.IsNotNull(createdResult.Value);
            Assert.That(createdResult.Value, Is.EqualTo("Customer Name is Required."));
        }


    }
}