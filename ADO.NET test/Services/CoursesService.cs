using ADO.NET_test.Models;
using MySql.Data.MySqlClient;
using System.Reflection.PortableExecutable;

namespace ADO.NET_test.Services
{
    public class CoursesService
    {
        public static int GetTotalCount()
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            const string sqlQuery = "SELECT COUNT(*) FROM courses";

            using var command = new MySqlCommand(sqlQuery, connection);
            object result = command.ExecuteScalar();

            if (result != null) return Convert.ToInt32(result);
            else return 0;

        }

        public static List<Course> Get(string fullName)
        {
            var courses = new List<Course>();

            try
            {
                using (var connection = new MySqlConnection(Constant.ConnectionString))
                {
                    connection.Open();

                    const string sqlQuery = @"
                    SELECT c.title, c.summary, c.photo 
                    FROM courses c
                    INNER JOIN user_courses uc ON c.id = uc.course_id
                    INNER JOIN users u ON u.id = uc.user_id
                    WHERE u.full_name = @FullName AND u.is_active = 1
                    ORDER BY uc.last_viewed DESC;";

                    using (var command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add(new MySqlParameter("@FullName", fullName));

                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                courses.Add(new Course
                                {
                                    Title = reader.IsDBNull(0) ? null : reader.GetString(0),
                                    Summary = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    Photo = reader.IsDBNull(2) ? null : reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка при получении курсов: {ex.Message}");
                Console.ResetColor();
            }

            return courses;
        }
    }
}

