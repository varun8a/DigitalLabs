using DL.DAL.DBHelpers;
using DL.DAL.Providers;

namespace DL.DAL
{
    public class DBOps 
    {
        private readonly IDBProvider _db;

        public DBOps(IDBProvider dbProvider)
        {
            _db = dbProvider;
        }

        private Customers _customersLoan;

        public virtual Customers CustomersLoan
        {
            get
            {
                if (_customersLoan == null)
                    _customersLoan = new Customers(_db);
                return _customersLoan;
            }
        }
    }
}