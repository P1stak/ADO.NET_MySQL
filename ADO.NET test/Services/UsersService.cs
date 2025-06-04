using System;
using ADO.NET_test.Models;
using MySql.Data.MySqlClient;

namespace ADO.NET_test.Services
{
    public class UsersService
    {
        public static bool Add(User user)
        {
            string connectionString = Constant.ConnectionString;
            string sqlQuery = @"INSERT INTO users 
                (full_name, details, join_date, avatar, is_active)
                VALUES 
                (@FullName, @Details, @JoinDate, @Avatar, @IsActive);";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Details", user.Details);
                        command.Parameters.AddWithValue("@JoinDate", user.JoinDate);
                        command.Parameters.AddWithValue("@Avatar", user.Avatar);
                        command.Parameters.AddWithValue("@IsActive", user.IsActive);

                        int execute = command.ExecuteNonQuery();
                        return execute > 0;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public static User Get(string fullName)
        {
            using (var connection = new MySqlConnection(Constant.ConnectionString))
            {
                connection.Open();

                const string sqlQuery = "SELECT * FROM users WHERE full_name = @FullName AND is_active = TRUE;";

                using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@FullName", fullName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                FullName = reader.IsDBNull(0) ? null : reader.GetString(0),
                                Details = reader.IsDBNull(1) ? null : reader.GetString(1),
                                JoinDate = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2),
                                Avatar = reader.IsDBNull(3) ? null : reader.GetString(3),
                                IsActive = reader.IsDBNull(4) ? true : reader.GetBoolean(4)

                            };
                        }

                    }
                }
            }
            return null;
        }
    }
}