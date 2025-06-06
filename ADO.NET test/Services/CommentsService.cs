using ADO.NET_test.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_test.Services
{
    public class CommentsService
    {
        /// <summary>
        /// Получение всех комментариев к курсу
        /// </summary>
        /// <param name="id">id курса</param>
        /// <returns>Список комментариев</returns>
        public static List<Comment> Get(int id)
        {
            var comments = new List<Comment>();

            try
            {
                using var connection = new MySqlConnection(Constant.ConnectionString);
                connection.Open();

                const string sqlQuery = @"SELECT c.id, c.text, c.time
                                        FROM comments AS c
                                        JOIN steps AS s ON c.step_id = s.id
                                        JOIN unit_lessons AS ul ON s.id = ul.lesson_id
                                        JOIN lessons AS l ON ul.lesson_id = l.id
                                        JOIN units AS u ON ul.unit_id = u.id
                                        JOIN courses AS cr ON u.course_id = cr.id
                                        WHERE reply_comment_id IS NULL AND cr.id = @id
                                        ORDER BY c.time DESC;";

                using var command = new MySqlCommand(sqlQuery, connection);
                {
                    command.Parameters.Add(new MySqlParameter("@id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comments.Add(new Comment
                            {
                                Id = reader.GetInt32(0),
                                Text = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Time = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка при получении коментария: {ex.Message}");
                Console.ResetColor();
            }

            return comments;
        }


        /// <summary>
        /// Удаление комментария пользователя
        /// </summary>
        /// <param name="id">id комментария</param>
        /// <returns>Удалось ли удалить комментарий</returns>
        public static bool Delete(int id)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            MySqlTransaction transaction = connection.BeginTransaction();

            try
            {
                const string sqlQuery = $@"DELETE FROM course_reviews WHERE comment_id = @id;";

                using var command = new MySqlCommand(sqlQuery, connection, transaction);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

                command.CommandText = $@"DELETE FROM comments WHERE reply_comment_id = @id;";
                command.ExecuteNonQuery();

                command.CommandText = $@"DELETE FROM comments WHERE id = @id;";
                command.ExecuteNonQuery();

                transaction.Commit();

                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}
