using DL.DAL.Providers;
using DL.DAL.DBHelpers;

namespace DL.DAL
{
    public class DBOps
    {
        private IDBProvider _db;

        //Empty for Test Cases
        public DBOps()
        {
        }
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