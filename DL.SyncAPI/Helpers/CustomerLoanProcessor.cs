﻿using DL.DAL;
using DL.Shared.Models;
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
        /// Add CustomerLoan
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<string> ProcessCustomerLoanProcessor(Customer customer)
        {
            //var customersLoan = JsonConvert.DeserializeObject<CustomersLoan>(JsonConvert.SerializeObject(customers));

            return await _dbOps.CustomersLoan.AddCustomersLoan(customer);
        }

        /// <summary>
        /// Get All Records for Customer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Customer>> GetAllCustomersLoan()
        {
            var result = await _dbOps.CustomersLoan.GetAllCustomersLoan();
            return result;
        }
    }
}
