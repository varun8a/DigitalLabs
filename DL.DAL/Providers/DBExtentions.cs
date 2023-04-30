using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DL.DAL.Providers
{
    #region Base Schema
    public interface IDBProvider
    {
        IDbConnection Connection { get; }
    }
    public class DBProvider : IDBProvider
    {
        private string _dbConnectionString;
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
    //public class DALBase
    //{
    //    public const byte MAX_DB_RETRY = 10;
    //    public DALBase(IDBProvider dbProvider)
    //    {
    //        DB = dbProvider;
    //    }

    //    public IDBProvider DB { get; private set; }

    //    public async Task<R> RunWithRetyAsync<R>(Func<IDbConnection, Task<R>> task)
    //    {
    //        return await Utils.RunWithRetyAsync(async () =>
    //        {
    //            using (var conn = DB.Connection)
    //            {
    //                conn.Open();
    //                return await task(conn);
    //            }
    //        }, DoBreakRetryOnFirstDBException, MAX_DB_RETRY);
    //    }

    //    public async Task RunWithRetyAsync(Func<IDbConnection, Task> task)
    //    {
    //        await RunWithRetyAsync(async (conn) =>
    //        {
    //            await task(conn);
    //            return true;
    //        });
    //    }
    //    public static readonly List<int> BreakingSQLErrorCodes = new List<int>() {
    //        2627, 10055, 10066, 11011, 11012, 15224
    //    };
    //    private bool DoBreakRetryOnFirstDBException(Exception ex)
    //    {
    //        if (ex is SqlException)
    //        {
    //            var sqlEx = ex as SqlException;
    //            if (sqlEx != null
    //                && BreakingSQLErrorCodes.Contains(sqlEx.Number))
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //}
    
}