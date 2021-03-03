using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiPhase2RepositoryTests.TestUtilites
{
    public class TestDbConnections
    {
        public class LocalDB
        {
            public const string Database =
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True";

            public static string Master =
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
        }
    }
}
