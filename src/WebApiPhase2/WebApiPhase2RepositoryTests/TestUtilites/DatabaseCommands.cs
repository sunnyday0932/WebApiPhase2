using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class DatabaseCommands
    {
        /// <summary>
        /// 建立DB
        /// </summary>
        /// <param name="connetionString"></param>
        /// <param name="dataBase"></param>
        public static void CreateDatabase(
            string connetionString,
            string dataBase)
        {
            var isExist = DatabaseExists(connetionString,dataBase);
            if (isExist)
            {
                return;
            }

            using var conn = new SqlConnection(connetionString);
            conn.Open();
            var sqlCommand = $@"CREATE DATABASE [{dataBase}];";
            conn.Execute(sqlCommand);
        }

        /// <summary>
        /// 檢查DB是否存在
        /// </summary>
        /// <param name="connetionString"></param>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        private static bool DatabaseExists(
            string connetionString,
            string dataBase)
        {
            using var conn = new SqlConnection(connetionString);
            conn.Open();
            var sqlCommand = $@"if exists(select * from sys.databases where name = '{dataBase}')
                                     select 'true'
                                     else
                                     select 'false'";

            var result = conn.QueryFirstOrDefault<string>(sqlCommand);
            return result.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 銷毀DB(for localDB)
        /// </summary>
        /// <param name="connetionString"></param>
        /// <param name="dataBase"></param>
        internal static void DestroyDatabase(
            string connetionString,
            string dataBase)
        {
            var queryCommand = $@"
                SELECT [physical_name] FROM [sys].[master_files]
                WHERE [database_id] = DB_ID('{dataBase}')";

            var fileNames = ExecuteSqlQuery(
                connetionString,
                string.Format(queryCommand,dataBase),
                row => (string) row["physical_name"]);

            var executeCommand = $@"
                ALTER DATABASE {dataBase} SET SINGLE_USER WITH ROLLBACK IMMEDIATE ;
                EXEC sp_detach_db '{dataBase}','true'";

            if (fileNames.Any())
            {
                ExecuteSqlCommand(connetionString, string.Format(executeCommand, dataBase));
                fileNames.ForEach(File.Delete);
            }

            var fileName = DatabaseFilePath(dataBase);

            try
            {
                var mdfPath = string.Concat(fileName, ".mdf");
                var ldfPath = string.Concat(fileName, ".ldf");

                var ismdfExist = File.Exists(mdfPath);
                var isldfExist = File.Exists(ldfPath);

                if (ismdfExist)
                {
                    File.Delete(mdfPath);
                };

                if (isldfExist)
                {
                    File.Delete(ldfPath);
                };
            }
            catch
            {
                Console.WriteLine("Could not delete the files (open in Visual Studio?)");
            }

        }

        /// <summary>
        /// 取得DB File path (for localDB)
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        private static string DatabaseFilePath(string dataBase)
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(directoryName,dataBase);
            return filePath;
        }
        /// <summary>
        /// 執行SQL Command(for localDB)
        /// </summary>
        /// <param name="connetionString"></param>
        /// <param name="commandText"></param>
        internal static void ExecuteSqlCommand(
            string connetionString,
            string commandText)
        {
            using var conn = new SqlConnection(connetionString);
            conn.Open();
            using var command = conn.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 執行SQL Query (for localDB)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connetionString"></param>
        /// <param name="queryText"></param>
        /// <param name="read"></param>
        /// <returns></returns>
        private static List<T> ExecuteSqlQuery<T>(
            string connetionString,
            string queryText,
            Func<SqlDataReader, T> read)
        {
            var result = new List<T>();
            using var conn = new SqlConnection(connetionString);
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = queryText;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(read(reader));
            }

            return result;
        }

        /// <summary>
        /// Drop Table Qeury
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static string DropTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "請確認輸入的Table Name");
            }

            var sqlCommand = $@"IF OBJECT_ID('dbo.{tableName}','U') IS NOT NULL
                                DROP TABLE dbo.{tableName}";

            return sqlCommand;
        }

        /// <summary>
        /// Trncate Table Query
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static string TruncateTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "請確認輸入的Table Name");
            }

            var sqlCommand = $@"IF OBJECT_ID('dbo.{tableName}','U') IS NOT NULL
                                TRUNCATE TABLE dbo.{tableName};";

            return sqlCommand;
        }
    }
}
