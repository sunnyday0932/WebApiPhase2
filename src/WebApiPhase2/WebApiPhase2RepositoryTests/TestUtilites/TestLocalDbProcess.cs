namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class TestLocalDbProcess
    {
        /// <summary>
        /// CreateDatabase
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public static void CreateDatabase(
            string connectionString,
            string databaseName)
        {
            DatabaseCommands.CreateDatabase(connectionString,databaseName);
        }

        /// <summary>
        /// DestroyDatabase
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public static void DestroyDatabase(
            string connectionString,
            string databaseName)
        {
            DatabaseCommands.DestroyDatabase(connectionString,databaseName);
        }
    }
}
