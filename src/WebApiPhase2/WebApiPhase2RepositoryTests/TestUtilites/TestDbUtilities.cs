using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class TestDbUtilities
    {
        private readonly string _testConnectionstring = 
            string.Concat(
                TestDbConnenction.LocalDb.LocalDbConnectionString,
                ";AttachDBFilename={0}.mdf");

        private string DatabaseName { get; set; }

        private string ConnectionString { get; set; }

        public TestDbUtilities(string databaseName)
        {
            this.DatabaseName = databaseName;
            this.ConnectionString = string.Format(_testConnectionstring,databaseName);
        }

        /// <summary>
        /// 確認DB是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsLocalDbExists()
        {
            using var conn = new SqlConnection(this.ConnectionString);
            return Database.Exists(conn);
        }

        /// <summary>
        /// 刪除LocalDb
        /// </summary>
        /// <param name="connectionString"></param>
        public void DeleteLocalDb(string connectionString = "")
        {
            var currentConnectionString = this.ConnectionString;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                currentConnectionString = connectionString;
            }

            if (!currentConnectionString.ToLower().Contains("localdb"))
            {
                return;
            }

            using var conn = new SqlConnection(currentConnectionString);
            if (Database.Exists(conn))
            {
                Database.Delete(conn);
            }
        }

        public void CreateDatabase()
        {
            this.DetachDatabase();

            var fileName = this.CleanupDatabase();
            using var conn = new SqlConnection(TestDbConnenction.LocalDb.Default);
            var commandText = new StringBuilder();
            commandText.AppendFormat(
                "CREATE DATABASE {0} ON (NAME = N'{0}', FILENAME = '{1}.mdf');",
                this.DatabaseName,
                fileName);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = commandText.ToString();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// CleanupDatabase
        /// </summary>
        /// <returns></returns>
        private string CleanupDatabase()
        {
            var fileName = this.DatabaseFilePath();
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

            return fileName;
        }

        /// <summary>
        /// DatabaseFilePath
        /// </summary>
        /// <returns></returns>
        private string DatabaseFilePath()
        {
            return Path.Combine(
                Path.GetDirectoryName(path: Assembly.GetExecutingAssembly().Location),
                this.DatabaseName);
        }

        /// <summary>
        /// DatachDatabase
        /// </summary>
        private void DetachDatabase()
        {
            using var conn = new SqlConnection(TestDbConnenction.LocalDb.Default);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"exec sp_detach_db '{this.DatabaseName}'";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Could not detach");
            }

        }
    }
}
