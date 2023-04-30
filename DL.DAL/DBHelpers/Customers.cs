using Dapper;
using DL.DAL.Providers;
using DL.Shared.Models;
using System.Data;

namespace DL.DAL.DBHelpers
{

    public class Customers
    {

        private readonly IDBProvider _db;

        public Customers(IDBProvider dbProvider)// : base(dbProvider)
        {
            _db = dbProvider;
        }
        /// <summary>
        /// AddCustomersLoan
        /// </summary>
        /// <param name="customersLoan"></param>
        /// <returns></returns>
        public virtual async Task<string> AddCustomersLoan(Customer customersLoan)
        {
            var parameters = new DynamicParameters();
            var spresult = string.Empty;
            parameters.Add("@CustomerSSN", customersLoan.CustomerSSN);
            parameters.Add("@FullName", customersLoan.FullName);
            parameters.Add("@LoanAmount", customersLoan.LoanAmount);
            parameters.Add("@EquityAmount", customersLoan.EquityAmount);
            parameters.Add("@SalaryAmount", customersLoan.SalaryAmount);
            parameters.Add("@spresult", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            string storedProcedure = "AddCustomersLoan";
            using (var conn = _db.Connection)
            {
                conn.Open();
                await conn.ExecuteAsync(storedProcedure,
                                        parameters,
                                        commandType: CommandType.StoredProcedure);
                // read output value
                spresult = parameters.Get<string>("@spresult");
            }

            return spresult;
        }

        /// <summary>
        /// GetAllCustomers records for Loan
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Customer>> GetAllCustomersLoan()
        {
            string storedProcedure = "GetAllCustomersLoan";
            using (var conn = _db.Connection)
            {
                conn.Open();

                var response = await conn.QueryAsync<Customer>(storedProcedure,
                       new DynamicParameters(),
                       commandType: CommandType.StoredProcedure);
                return response ?? new List<Customer>();
            }
        }
    }
}
