using Dapper;
using System;
using System.Data.SqlClient;

namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class TableCommands
    {
        /// <summary>
        /// Drop Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static void DropTable(
            string connectionString,
            string tableName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "請確認輸入的連線字串");
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "請確認輸入的Table");
            }

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            var sqlCommand = $@"IF OBJECT_ID('dbo.{tableName}','U') IS NOT NULL
                                DROP TABLE dbo.{tableName}";

            conn.Execute(sqlCommand);
        }

        /// <summary>
        /// Truncate Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static void TruncateTable(
            string connectionString, 
            string tableName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "請確認輸入的連線字串");
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "請確認輸入的Table Name");
            }

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            var sqlCommand = $@"IF OBJECT_ID('dbo.{tableName}','U') IS NOT NULL
                                TRUNCATE TABLE dbo.{tableName};";
            conn.Execute(sqlCommand);
        }

        /// <summary>
        /// CreateTable
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlCommand"></param>
        public static void CreateTable(
            string connectionString,
            string sqlCommand)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "請確認輸入的連線字串");
            }

            if (string.IsNullOrWhiteSpace(sqlCommand))
            {
                throw new ArgumentNullException(nameof(sqlCommand), "請確認輸入的sqlCommand");
            }

            using var conn = TestDbConnenction.GetSqlConnection(connectionString);
            conn.Open();
            conn.Execute(sqlCommand);
        }

        /// <summary>
        /// Execute SQL command
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        public static void Execute(
            string connectionString,
            string commandText)
        {
            using var conn = TestDbConnenction.GetSqlConnection(connectionString);
            conn.Open();
            var command = conn.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
    }
}
