using DL.DAL;
using DL.Shared.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace DL.SyncAPI.Helpers
{
    public class CustomerLoanProcessor
    {
        private readonly DBOps _dbOps;

        public CustomerLoanProcessor(DBOps dbOps)
        {
            _dbOps = dbOps;
        }
        public async Task<string> ProcessCustomerLoanProcessor(Customer customer, ILogger log)
        {
            //var customersLoan = JsonConvert.DeserializeObject<CustomersLoan>(JsonConvert.SerializeObject(customers));

            return await _dbOps.CustomersLoan.AddCustomersLoan(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersLoan(ILogger log)
        {
            var result = await _dbOps.CustomersLoan.GetAllCustomersLoan();
            return result;
            //return JsonConvert.DeserializeObject<IEnumerable<Customers>>(JsonConvert.SerializeObject(result));
        }
    }
}
