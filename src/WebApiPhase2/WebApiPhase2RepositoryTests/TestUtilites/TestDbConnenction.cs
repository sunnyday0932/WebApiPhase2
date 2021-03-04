﻿namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class TestDbConnenction
    {
        public class LocalDb
        {
            public const string LocalDbConnectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True";

            public static string Default =
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
        }
    }
}
