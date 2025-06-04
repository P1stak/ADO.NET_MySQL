using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_test.Services
{
    public class CoursesService
    {
        public static int GetTotalCount()
        {
            var connectionString = Constant.ConnectionString;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            const string sqlQuery = "SELECT COUNT(*) FROM courses";

            using var command = new MySqlCommand(sqlQuery, connection);
            object result = command.ExecuteScalar();

            if (result != null) return Convert.ToInt32(result);
            else return 0;

        }
    }
}
