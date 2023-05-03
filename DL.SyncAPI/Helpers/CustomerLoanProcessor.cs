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
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbOps"></param>
        public CustomerLoanProcessor(DBOps dbOps)
        {
            _dbOps = dbOps;
        }
        /// <summary>
        /// Add Customer Loan
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<string> ProcessCustomerLoanProcessor(Customer customer)
        {
            return await _dbOps.CustomersLoan.AddCustomersLoan(customer);
        }

        /// <summary>
        /// Get All Records for Customers
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Customer>> GetAllCustomersLoan()
        {
            var result = await _dbOps.CustomersLoan.GetAllCustomersLoan();
            return result;
        }
    }
}
