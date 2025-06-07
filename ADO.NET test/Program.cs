using ADO.NET_test.Models;
using ADO.NET_test.Services;
using MySql.Data.MySqlClient;
using System.Data;

public class Program
{

    /// <summary>
    /// Обработка начального меню
    /// </summary>
    public static void Main()
    {
        DisplayMainMenu();

        while (true)
        {
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    User user = PerformLoginUser();
                    if (!string.IsNullOrEmpty(user?.FullName))
                    {
                        UserMenu(user);
                    }
                    break;
                case "2":
                    User newUser = PerformRegistrationUser();
                    if (!string.IsNullOrEmpty(newUser?.FullName))
                    {
                        UserMenu(newUser);
                    }
                    break;
                case "3":
                    PerformDeleteUser();
                    DisplayMainMenu();
                    break;
                case "4":
                    DisplayUserRating();
                    DisplayMainMenu();
                    break;
                case "5":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("До свидания!\n");
                    Console.ResetColor();
                    return;
                default:
                    PrintWrongInputMessage();
                    break;
            }
        }
    }

    /// <summary>
    /// Отображение главного меню приложения.
    /// </summary>
    public static void DisplayMainMenu()
    {
        using var connect = new MySqlConnection(Constant.ConnectionString);

        var totalCoursesCount = CoursesService.GetTotalCount();
        var totalUsersCount = UsersService.GetTotalCount();
        Console.ForegroundColor = ConsoleColor.Cyan;



        Console.WriteLine(@$"
                            ***********************************************************
                            *********** Добро пожаловать в БД {connect.Database.ToString()}! ************
                            ***********************************************************
                            ************ Количество курсов на платформе: {totalCoursesCount} ***********
                            ******* Количество пользователей на платформе: {totalUsersCount} *********
                            ***********************************************************

                            Выберите действие (введите число и нажмите Enter):

                            1. Войти
                            2. Зарегистрироваться
                            3. Удалить пользователя из БД
                            4. Посмотреть рейтинг пользователя
                            5. Закрыть приложение

                            ***********************************************************

");
        Console.ResetColor();
    }

    /// <summary>
    /// Вывод сообщения об ошибке при неверном выборе.
    /// </summary>
    public static void PrintWrongInputMessage()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Неверный выбор. Попробуйте снова.");
        Console.ResetColor();
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <returns>Возвращает объект пользователя, если регистрация успешна, иначе пустой объект.</returns>
    public static User PerformRegistrationUser()
    {
        var userName = "";
        while (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Введите имя и фамилию через пробел и нажмите Enter:");
            userName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Имя не может быть пустым!");
                Console.ResetColor();
                continue;
            }

            if (userName.Split(' ').Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Пожалуйста, введите и имя, и фамилию!");
                Console.ResetColor();
                userName = "";
            }
        }

        var newUser = new User
        {
            FullName = userName
        };

        try
        {
            bool isAdditionSuccessful = UsersService.Add(newUser);

            if (isAdditionSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Пользователь '{newUser.FullName}' успешно добавлен.\n");
                Console.ResetColor();
                return newUser;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Произошла ошибка. Пользователь уже существует в БД.\n");
                Console.ResetColor();
                return new User();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка при добавлении пользователя: {ex.Message}\n");
            Console.ResetColor();
            return new User();
        }
    }

    /// <summary>
    /// Вход пользователя в систему.
    /// </summary>
    /// <returns>Возвращает объект пользователя, если вход успешен, иначе пустой объект.</returns>
    public static User PerformLoginUser()
    {
        var userName = "";
        while (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Введите имя и фамилию через пробел и нажмите Enter:");
            userName = Console.ReadLine();
        }

        User user = UsersService.Get(userName);

        if (!string.IsNullOrEmpty(user?.FullName))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Пользователь '{user.FullName}' успешно вошел.\n");
            Console.ResetColor();
            return user;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Пользователь не найден, произведен выход на главную страницу.\n");
            Console.ResetColor();
            DisplayMainMenu();
            return new User();
        }
    }

    /// <summary>
    /// Удаление пользователя из БД.
    /// </summary>
    public static void PerformDeleteUser()
    {
        var userName = "";

        while (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Введите имя и фамилию через пробел и нажмите Enter:");
            userName = Console.ReadLine();
        }
        var newUser = new User
        {
            FullName = userName
        };

        bool isRemoveSuccessfu = UsersService.RemoveUser(userName);

        if (isRemoveSuccessfu)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Пользователь '{newUser.FullName}' успешно удален из БД.\n");
            Console.ResetColor(); ;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Произошла ошибка, произведен выход на главную страницу.\n");
            Console.ResetColor();
            DisplayMainMenu();
        }
    }

    /// <summary>
    /// Обработка меню пользователя после успешного входа.
    /// </summary>
    public static void UserMenu(User user)
    {
        while (true)
        {
            DisplayUserMenu(user);
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ProfileMenu(user);
                    break;
                case "2":
                    UserCoursesMenu(user);
                    break;
                case "3":
                    DisplayMainMenu();
                    return;
                default:
                    PrintWrongInputMessage();
                    break;
            }
        }
    }

    /// <summary>
    /// Отображение меню пользователя.
    /// </summary>
    public static void DisplayUserMenu(User user)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@$"
                            * {user.FullName} *

                            Выберите действие (введите число и нажмите Enter):

                            1. Посмотреть профиль
                            2. Посмотреть курсы
                            3. Выйти
                            ");
        Console.ResetColor();
    }

    /// <summary>
    /// Обработка меню профиля.
    /// </summary>
    public static void ProfileMenu(User user)
    {
        while (true)
        {
            DisplayProfileDetails(user);
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    return;
                default:
                    PrintWrongInputMessage();
                    break;
            }
        }
    }

    /// <summary>
    /// Отображение деталей профиля.
    /// </summary>
    public static void DisplayProfileDetails(User user)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(@$"
                    * {user.FullName} *

                    Выберите действие (введите число и нажмите Enter):

                    1. Назад

                    Профиль пользователя: {user.FullName}
                    Дата регистрации: {user.JoinDate}
                    Описание профиля: {user.Details ?? "Не заполнено"}
                    Фото профиля: {user.Avatar ?? "Не заполнено"}
                    {UsersService.FormatUserMetrics(user.FollowersCount)} подписчиков
                    {UsersService.FormatUserMetrics(user.Reputation)} репутация
                    {UsersService.FormatUserMetrics(user.Knowledge)} знания
                    ");
        Console.ResetColor();
    }

    /// <summary>
    /// Обработка меню курсов пользователя.
    /// </summary>
    public static void UserCoursesMenu(User user)
    {
        while (true)
        {
            var coursesId = DisplayUserCourses(user.FullName);
            string input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    return;
                default:
                    if (coursesId.Contains(input))
                    {
                        UserCommentsMenu(Convert.ToInt32(input), user);
                    }
                    else
                    {
                        PrintWrongInputMessage();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Отображение списка курсов пользователя.
    /// </summary>
    private static IEnumerable<string> DisplayUserCourses(string fullName)
    {
        List<Course> courses = CoursesService.Get(fullName);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n* Список курсов " + fullName + " *\n\n" +
                          "Выберите действие (введите число и нажмите Enter):\n" +
                          "0. Назад");

        if (courses.Count == 0)
        {
            Console.WriteLine("У пользователя еще нет курсов.");
        }
        else
        {
            Console.WriteLine("Для просмотра подробностей курса, введите его id.\n");
            foreach (var course in courses)
            {
                Console.WriteLine("______________________________________________\n" +
                                  "id: " + course.Id + "\n" +
                                  "Название: " + course.Title + "\n" +
                                  "Описание: " + (course.Summary ?? "Отсутствует") + "\n" +
                                  "Фото: " + (course.Photo ?? "Отсутствует") + "\n" +
                                  "______________________________________________");
            }
        }
        Console.ResetColor();
        return courses.Select(x => x.Id.ToString());
    }

    /// <summary>
    /// Обработка меню комментариев пользователя.
    /// </summary>
    public static void UserCommentsMenu(int id, User user)
    {
        while (true)
        {
            var commentsIds = DisplayUserComments(id, user);
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "0":
                    return;
                default:
                    if (commentsIds.Contains(choice))
                    {
                        var isCommentDeleted = CommentsService.Delete(Convert.ToInt32(choice));
                        if (isCommentDeleted)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Комментарий успешно удален");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ошибка удаления комментария");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        PrintWrongInputMessage();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Отображение комментариев к курсам пользователя.
    /// </summary>
    private static IEnumerable<string> DisplayUserComments(int id, User user)
    {
        List<Course> courses = CoursesService.Get(user.FullName);
        var currentCourse = courses.FirstOrDefault(x => x.Id == id);
        List<Comment> comments = CommentsService.Get(id);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\n* Комментарии к курсу " + currentCourse.Title + " *\n\n" +
                          "Выберите действие (введите число и нажмите Enter):\n" +
                          "0. Назад");

        if (comments.Count == 0)
        {
            Console.WriteLine("У курса еще нет комментариев.");
        }
        else
        {
            Console.WriteLine("Чтобы удалить комментарий, введите его id.");
            foreach (var comment in comments)
            {
                Console.WriteLine("______________________________________________\n" +
                                  comment.Id + "\n" +
                                  comment.Time + "\n" +
                                  comment.Text + "\n" +
                                  "______________________________________________");
            }
        }
        Console.ResetColor();
        return comments.Select(x => x.Id.ToString());
    }

    /// <summary>
    /// Отображение рейтинга пользователей.
    /// </summary>
    public static void DisplayUserRating()
    {
        Console.Clear(); // Очищаем консоль перед выводом рейтинга

        Console.ForegroundColor = ConsoleColor.Cyan;

        // Центрируем заголовок
        int windowWidth = Console.WindowWidth;
        string title = "* Рейтинг пользователей *";
        Console.WriteLine(new string(' ', (windowWidth - title.Length) / 2) + title + "\n");

        var dataSet = UsersService.GetUserRating();

        if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
        {
            Console.WriteLine("На платформе еще нет пользователей");
            Console.ResetColor();
            return;
        }

        // Настраиваем отступы и ширину колонок
        int nameWidth = 25;
        int knowledgeWidth = 15;
        int reputationWidth = 15;
        int totalWidth = nameWidth + knowledgeWidth + reputationWidth + 6; // +6 для разделителей

        // Центрируем таблицу
        int tableIndent = (windowWidth - totalWidth) / 2;
        string indent = new string(' ', tableIndent);

        // Выводим разделительную линию
        Console.WriteLine(indent + new string('═', totalWidth));

        // Выводим заголовки колонок с центрированием текста
        Console.WriteLine(indent + "║ " + CenterText("Пользователь", nameWidth) + " ║ " +
                          CenterText("Знания", knowledgeWidth) + " ║ " +
                          CenterText("Репутация", reputationWidth) + " ║");

        Console.WriteLine(indent + new string('═', totalWidth));

        // Выводим данные
        foreach (DataRow row in dataSet.Tables[0].Rows)
        {
            Console.WriteLine(indent + "║ " + (row["full_name"]?.ToString() ?? "").PadRight(nameWidth) + " ║ " +
                              CenterText(row["knowledge"]?.ToString() ?? "", knowledgeWidth) + " ║ " +
                              CenterText(row["reputation"]?.ToString() ?? "", reputationWidth) + " ║");
        }

        Console.WriteLine(indent + new string('═', totalWidth));
        Console.ResetColor();

        // Ждем действия пользователя перед возвратом в меню
        Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
        Console.Clear();
    }

    // Вспомогательный метод для центрирования текста в колонке
    private static string CenterText(string text, int width)
    {
        if (text.Length >= width)
        {
            return text;
        }

        int leftPadding = (width - text.Length) / 2;
        int rightPadding = width - text.Length - leftPadding;

        return new string(' ', leftPadding) + text + new string(' ', rightPadding);
    }
}


// ДЛЯ РАБОТЫ С МЕНЮ ПОЛЬЗОВАТЕЛЯ, НЕОБХОДИМО СОЗДАТЬ ХРАНИМУЮ ПРОЦЕДУРУ В MYSQL

/*
 * 
 DELIMITER //

CREATE FUNCTION format_number(number INT)
RETURNS VARCHAR(50)
DETERMINISTIC
BEGIN
    DECLARE formatted_number VARCHAR(50);

    IF number < 1000 THEN
        SET formatted_number = CAST(number AS CHAR);
    ELSE
        SET formatted_number = CONCAT(FORMAT(number / 1000, 1), 'K');        
        SET formatted_number = REPLACE(formatted_number, '.0K', 'K');
    END IF;

    RETURN formatted_number;
END //

DELIMITER ;

 */