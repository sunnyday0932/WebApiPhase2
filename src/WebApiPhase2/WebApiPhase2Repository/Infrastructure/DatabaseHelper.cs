using System.Data;
using System.Data.SqlClient;

namespace WebApiPhase2Repository.Infrastructure
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// 建立連線
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            var conn = new SqlConnection(this._connectionString);

            return conn;
        }
    }
}