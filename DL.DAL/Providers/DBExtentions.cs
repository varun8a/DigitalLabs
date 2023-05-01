using Microsoft.Data.SqlClient;
using System.Data;

namespace DL.DAL.Providers
{
    #region Base Schema
    public interface IDBProvider
    {
        IDbConnection Connection { get; }
    }
    public class DBProvider : IDBProvider
    {
        private readonly string _dbConnectionString;
        public DBProvider(string connectionString)
        {
            _dbConnectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_dbConnectionString);
            }
        }
    }
    #endregion
   
}