using Azure;
using Dapper;
using DL.DAL.Providers;
using DL.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DL.DAL.DBHelpers
{
    
    public class CustomersLoan //:DALBase
    {

        private IDBProvider _db;
        public CustomersLoan()
        {

        }
        public CustomersLoan(IDBProvider dbProvider)// : base(dbProvider)
        {
            _db = dbProvider;
        }

        public virtual async Task<string> AddCustomersLoan(Customer customersLoan)
        {
            var parameters = new DynamicParameters();
            var spresult = string.Empty;
            string storedProcedure = string.Empty;
            parameters.Add("@CustomerSSN", customersLoan.CustomerSSN);
            parameters.Add("@FullName", customersLoan.FullName);
            parameters.Add("@LoanAmount", customersLoan.LoanAmount);
            parameters.Add("@EquityAmount", customersLoan.EquityAmount);
            parameters.Add("@SalaryAmount", customersLoan.SalaryAmount);
            parameters.Add("@spresult", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            storedProcedure = "AddCustomersLoan";
            //await RunWithRetyAsync(async (conn) =>
            //{
            using (var conn = _db.Connection)
            {
                conn.Open();
                await conn.ExecuteAsync(storedProcedure,
                                        parameters,
                                        commandType: CommandType.StoredProcedure);
                // read output value
                spresult = parameters.Get<string>("@spresult");
            }

            //});

            return spresult;
        }

        public virtual async Task<IEnumerable<Customer>> GetAllCustomersLoan()
        {
            var parameters = new DynamicParameters();
            var spresult = string.Empty;
            string storedProcedure = string.Empty;

            storedProcedure = "GetAllCustomersLoan";

            //return await RunWithRetyAsync(async (conn) =>
            //  {
            //  {
            using (var conn = _db.Connection)
            {
                conn.Open();

                var response = await conn.QueryAsync<Customer>(storedProcedure,
                       parameters,
                       commandType: CommandType.StoredProcedure);
                return response ?? new List<Customer>();
            }
            //});

        }

    }
}
