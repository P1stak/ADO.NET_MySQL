using ADO.NET_test.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

namespace ADO.NET_test.Services
{
    public class CoursesService
    {
        /// <summary>
        /// Получение списка курсов пользователя
        /// </summary>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>Список курсов</returns>
        public List<Course> Get(string fullName)
        {
            var courses = new List<Course>();
            try
            {
                using var connection = new MySqlConnection(Constant.ConnectionString);
                connection.Open();

                const string sqlQuery = @"SELECT title, summary, photo, courses.id
                      FROM user_courses
                      JOIN courses ON user_courses.course_id = courses.id
                      JOIN users ON users.id = user_courses.user_id
                      WHERE users.full_name = @fullName AND users.is_active = 1
                      ORDER BY user_courses.last_viewed DESC;";

                using var command = new MySqlCommand(sqlQuery, connection);
                var fullNameParam = new MySqlParameter("@fullName", fullName);
                command.Parameters.Add(fullNameParam);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var course = new Course
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title"),
                        Summary = reader.IsDBNull("summary") ? null : reader.GetString("summary"),
                        Photo = reader.IsDBNull("photo") ? null : reader.GetString("photo")
                    };
                    courses.Add(course);
                }

                return courses;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения списка курсов: {ex.Message}");
                return new List<Course>();
                
            }
        }


        /// <summary>
        /// Получение общего количества курсов
        /// </summary>
        public int GetTotalCount()
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            const string sqlQuery = "SELECT COUNT(*) FROM courses";

            using var command = new MySqlCommand(sqlQuery, connection);
            object result = command.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : 0;

        }
    }
}

