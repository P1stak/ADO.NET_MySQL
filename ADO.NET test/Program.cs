using ADO.NET_test.Models;
using ADO.NET_test.Services;
using MySql.Data.MySqlClient;
using System.Data;

public class Program
{
    private const int ConsoleWidth = 80;
    private const char BorderChar = '=';
    private const char LineChar = '-';

    /// <summary>
    /// Обработка начального меню
    /// </summary>
    public static void Main()
    {
        Console.Title = "myDB";
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
                    PrintCentered("До свидания!");
                    Console.ResetColor();
                    return;
                default:
                    PrintWrongInputMessage();
                    break;
            }
        }
    }

    /// <summary>
    /// Печатает текст по центру консоли
    /// </summary>
    private static void PrintCentered(string text, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        int padding = (ConsoleWidth - text.Length) / 2;
        Console.WriteLine(text.PadLeft(padding + text.Length));
        Console.ResetColor();
    }

    /// <summary>
    /// Печатает разделительную линию
    /// </summary>
    private static void PrintLine(int length = ConsoleWidth, char lineChar = LineChar)
    {
        Console.WriteLine(new string(lineChar, length));
    }

    /// <summary>
    /// Отображение главного меню приложения.
    /// </summary>
    public static void DisplayMainMenu()
    {
        Console.Clear();
        using var connect = new MySqlConnection(Constant.ConnectionString);

        var totalCoursesCount = CoursesService.GetTotalCount();
        var totalUsersCount = UsersService.GetTotalCount();

        PrintLine(ConsoleWidth, BorderChar);
        PrintCentered($"Добро пожаловать в БД {connect.Database}!", ConsoleColor.Cyan);
        PrintLine(ConsoleWidth, BorderChar);

        Console.WriteLine($"\n{"Количество курсов на платформе:",-30} {totalCoursesCount}");
        Console.WriteLine($"{"Количество пользователей:",-30} {totalUsersCount}\n");

        PrintLine();
        Console.WriteLine("Выберите действие (введите число и нажмите Enter):\n");
        Console.WriteLine("1. Войти");
        Console.WriteLine("2. Зарегистрироваться");
        Console.WriteLine("3. Удалить пользователя");
        Console.WriteLine("4. Рейтинг пользователей");
        Console.WriteLine("5. Закрыть приложение");
        PrintLine(ConsoleWidth, BorderChar);
        Console.ResetColor();
    }

    /// <summary>
    /// Вывод сообщения об ошибке при неверном выборе.
    /// </summary>
    public static void PrintWrongInputMessage()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nНеверный выбор. Попробуйте снова.\n");
        Console.ResetColor();
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    public static User PerformRegistrationUser()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered("РЕГИСТРАЦИЯ НОВОГО ПОЛЬЗОВАТЕЛЯ");
        PrintLine();
        Console.ResetColor();

        var userName = "";
        while (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Введите имя и фамилию через пробел:");
            userName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Имя не может быть пустым!\n");
                Console.ResetColor();
                continue;
            }

            if (userName.Split(' ').Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Пожалуйста, введите и имя, и фамилию!\n");
                Console.ResetColor();
                userName = "";
            }
        }

        var newUser = new User { FullName = userName };

        try
        {
            bool isAdditionSuccessful = UsersService.Add(newUser);

            if (isAdditionSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nПользователь '{newUser.FullName}' успешно добавлен.\n");
                Console.ResetColor();
                return newUser;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nОшибка: Пользователь уже существует.\n");
                Console.ResetColor();
                return new User();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nОшибка при добавлении: {ex.Message}\n");
            Console.ResetColor();
            return new User();
        }
    }

    /// <summary>
    /// Вход пользователя в систему.
    /// </summary>
    public static User PerformLoginUser()
    {
        while (true) // Бесконечный цикл, пока пользователь не войдет или не выйдет
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintCentered("АВТОРИЗАЦИЯ");
            PrintLine();
            Console.ResetColor();

            var userName = "";
            while (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Введите имя и фамилию (или 0 для выхода):");
                userName = Console.ReadLine()?.Trim();

                // Проверка на команду выхода
                if (userName == "0")
                {
                    DisplayMainMenu();
                    return new User(); // Возвращаем пустого пользователя
                }

                if (string.IsNullOrEmpty(userName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Имя не может быть пустым!\n");
                    Console.ResetColor();
                    continue;
                }

                if (userName.Split(' ').Length < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пожалуйста, введите и имя, и фамилию!\n");
                    Console.ResetColor();
                    userName = "";
                }
            }

            try
            {
                User user = UsersService.Get(userName);

                if (!string.IsNullOrEmpty(user?.FullName))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nДобро пожаловать, {user.FullName}!\n");
                    Console.ResetColor();
                    Thread.Sleep(1000); // Задержка для отображения сообщения
                    return user;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nПользователь не найден.\n");

                    // Предложение выбора действия
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1. Попробовать снова");
                    Console.WriteLine("2. Вернуться в главное меню");
                    Console.ResetColor();

                    string choice = Console.ReadLine();
                    if (choice == "2")
                    {
                        DisplayMainMenu();
                        return new User();
                    }
                    // Если выбран 1 или другой вариант - цикл продолжится
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nОшибка при авторизации: {ex.Message}\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Попробовать снова");
                Console.WriteLine("2. Вернуться в главное меню");
                Console.ResetColor();

                string choice = Console.ReadLine();
                if (choice == "2")
                {
                    DisplayMainMenu();
                    return new User();
                }
            }
        }
    }

    /// <summary>
    /// Удаление пользователя из БД.
    /// </summary>
    public static void PerformDeleteUser()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered("УДАЛЕНИЕ ПОЛЬЗОВАТЕЛЯ");
        PrintLine();
        Console.ResetColor();

        var userName = "";
        while (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Введите имя и фамилию пользователя для удаления:");
            userName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Имя не может быть пустым!\n");
                Console.ResetColor();
                continue;
            }

            if (userName.Split(' ').Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Пожалуйста, введите и имя, и фамилию!\n");
                Console.ResetColor();
                userName = "";
            }
        }

        try
        {
            bool isRemoveSuccessful = UsersService.RemoveUser(userName);

            if (isRemoveSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nПользователь '{userName}' успешно удален.\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nПользователь '{userName}' не найден.\n");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nОшибка при удалении: {ex.Message}\n");
        }

        Console.ResetColor();
        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
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
                    DisplayCertificates(user);
                    break;
                case "4":
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
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        PrintLine(ConsoleWidth, BorderChar);
        PrintCentered($"ЛИЧНЫЙ КАБИНЕТ: {user.FullName.ToUpper()}");
        PrintLine(ConsoleWidth, BorderChar);

        Console.WriteLine("\nВыберите действие:\n");
        Console.WriteLine("1. Посмотреть профиль");
        Console.WriteLine("2. Посмотреть курсы");
        Console.WriteLine("3. Мои сертификаты");
        Console.WriteLine("4. Выйти\n");  // Добавленная строка
        PrintLine();
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
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        PrintLine(ConsoleWidth, BorderChar);
        PrintCentered($"ПРОФИЛЬ: {user.FullName.ToUpper()}");
        PrintLine(ConsoleWidth, BorderChar);

        Console.WriteLine($"\n{"Дата регистрации:",-20} {user.JoinDate}");
        Console.WriteLine($"{"Описание:",-20} {user.Details ?? "Не заполнено"}");
        Console.WriteLine($"{"Фото профиля:",-20} {user.Avatar ?? "Не заполнено"}");
        Console.WriteLine($"{"Подписчики:",-20} {UsersService.FormatUserMetrics(user.FollowersCount)}");
        Console.WriteLine($"{"Репутация:",-20} {UsersService.FormatUserMetrics(user.Reputation)}");
        Console.WriteLine($"{"Знания:",-20} {UsersService.FormatUserMetrics(user.Knowledge)}\n");

        PrintLine();
        Console.WriteLine("1. Назад\n");
        PrintLine();
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
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;

        PrintLine(ConsoleWidth, BorderChar);
        PrintCentered("РЕЙТИНГ ПОЛЬЗОВАТЕЛЕЙ");
        PrintLine(ConsoleWidth, BorderChar);

        Console.WriteLine("\nВыберите действие:\n");
        Console.WriteLine("1. Назад\n");

        try
        {
            var dataSet = UsersService.GetUserRating();

            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("На платформе еще нет пользователей");
                Console.ResetColor();
                return;
            }

            const int nameWidth = 25;
            const int statWidth = 12;

            PrintLine(ConsoleWidth);
            Console.WriteLine(
                $"{"Пользователь".PadRight(nameWidth)} " +
                $"{"Знания".PadLeft(statWidth)} " +
                $"{"Репутация".PadLeft(statWidth)}");
            PrintLine(ConsoleWidth);

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Console.WriteLine(
                    $"{row["full_name"]?.ToString()?.PadRight(nameWidth)} " +
                    $"{row["knowledge"]?.ToString()?.PadLeft(statWidth)} " +
                    $"{row["reputation"]?.ToString()?.PadLeft(statWidth)}");
            }

            PrintLine(ConsoleWidth);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nОшибка при загрузке рейтинга: {ex.Message}\n");
        }

        Console.ResetColor();
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    /// <summary>
    /// Отображение сертификатов пользователя
    /// </summary>
    public static void DisplayCertificates(User user)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        PrintLine(ConsoleWidth, BorderChar);
        PrintCentered($"СЕРТИФИКАТЫ: {user.FullName.ToUpper()}");
        PrintLine(ConsoleWidth, BorderChar);

        try
        {
            var certificates = CertificatesService.Get(user.FullName);

            if (certificates.Tables.Count == 0 || certificates.Tables[0].Rows.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("У пользователя нет сертификатов.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\nСписок сертификатов:\n");
                foreach (DataRow row in certificates.Tables[0].Rows)
                {
                    Console.WriteLine($"Курс: {row["title"]}");
                    Console.WriteLine($"Дата выдачи: {((DateTime)row["issue_date"]).ToString("dd.MM.yyyy")}");
                    Console.WriteLine($"Оценка: {row["grade"]}\n");
                    PrintLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка при загрузке сертификатов: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
            }
            Console.ResetColor();
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
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