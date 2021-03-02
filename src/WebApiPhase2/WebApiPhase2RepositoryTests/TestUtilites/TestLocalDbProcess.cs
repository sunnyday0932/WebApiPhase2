using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class TestLocalDbProcess
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
            DatabaseCommands.CreateDatabase(connectionString,database);
        }

        /// <summary>
        /// 銷毀DB
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="database"></param>
        public static void DestroyDatabase(
            string connectionString,
            string database)
        {
            DatabaseCommands.DestroyDatabase(connectionString,database);
        }
    }
}
