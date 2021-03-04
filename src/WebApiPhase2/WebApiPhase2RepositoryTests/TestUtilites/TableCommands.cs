using System;

namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class TableCommands
    {
        /// <summary>
        /// Drop Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string DropTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "請確認輸入的Table");
            }

            var sqlCommand = $@"IF OBJECT_ID('dbo.{tableName}','U') IS NOT NULL
                                DROP TABLE dbo.{tableName}";

            return sqlCommand;
        }

        /// <summary>
        /// Truncate Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string TruncateTable(string tableName)
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
