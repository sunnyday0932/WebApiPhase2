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
        /// <param name="connectionString"></param>
        /// <param name="database"></param>
        public static void CreateDatabase(
            string connectionString,
            string database)
        {
            var IsExists = DatabaseExists(connectionString, database);
            if (IsExists)
            {
                return;
            }

            using var conn = new SqlConnection(connectionString);
            conn.Open();
            var sqlCommand = $"CREATE DATABASE [{database}]";
            conn.Execute(sqlCommand);
        }

        /// <summary>
        /// 確認DB是否存在
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        private static bool DatabaseExists(
            string connectionString,
            string database)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            var sqlCommand = new StringBuilder();
            sqlCommand.AppendLine($"if exists(select * from sys.databases where name = '{database}')");
            sqlCommand.AppendLine($"select 'true'");
            sqlCommand.AppendLine($"else ");
            sqlCommand.AppendLine($"select 'false'");

            var result = conn.QueryFirstOrDefault<string>(sqlCommand.ToString());
            return result.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// DestroyDatabase
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dataBasename"></param>
        internal static void DestroyDatabase(
            string connectionString,
            string dataBasename)
        {
            var queryCommand = $@"
                SELECT [physical_name] FROM [sys].[master_files]
                WHERE [database_id] = DB_ID('{dataBasename}')";

            var fileNames = ExecuteSqlQuery(
                connectionString,
                string.Format(queryCommand, dataBasename),
                row => (string) row["physical_name"]);

            var executeCommand = $@"
                ALTER DATABASE {dataBasename} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                EXEC sp_detach_db '{dataBasename}','true'";

            if (fileNames.Any())
            {
                ExecuteSqlCommand(connectionString, string.Format(executeCommand, dataBasename));
                fileNames.ForEach(File.Delete);
            }

            var fileName = DatabaseFilePath(dataBasename);

            try
            {
                var mdfPath = string.Concat(fileName, ".mdf");
                var ldfPath = string.Concat(fileName, "_log.ldf");

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
        /// 取得DB File path (for localDB)
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        private static string DatabaseFilePath(string dataBase)
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(directoryName, dataBase);
            return filePath;
        }
    }
}
