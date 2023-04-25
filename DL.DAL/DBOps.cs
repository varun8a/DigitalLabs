using DL.DAL.Providers;
using DL.DAL.DBHelpers;

namespace DL.DAL
{
    public class DBOps
    {
        private IDBProvider _db;

        public DBOps(IDBProvider dbProvider)
        {
            _db = dbProvider;
        }

        private CustomersLoan _customersLoan;

        public virtual CustomersLoan CustomersLoan
        {
            get
            {
                if (_customersLoan == null)
                    _customersLoan = new CustomersLoan(_db);
                return _customersLoan;
            }
        }
    }
}