using ADO.NET_test.Models;
using ADO.NET_test.Services;

public class UsersProcessing
{
    private readonly UsersService _usersService = new();

    public User PerformRegistration()
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

        bool isAdditionSuccessful = _usersService.Add(newUser);

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
            Console.WriteLine($"Произошла ошибка, произведен выход на главную страницу.\n");
            Console.ResetColor();
            return new User();
        }
    }

    public User PerformLogin()
    {
        var userName = "";
        while (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Введите имя и фамилию через пробел и нажмите Enter:");
            userName = Console.ReadLine();
        }

        User? user = _usersService.Get(userName);

        if (user != null)
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
            return new User();
        }
    }

    public void PerformDeletion()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Введите имя и фамилию через пробел для удаления профиля:");
        Console.ResetColor();

        while (true)
        {
            string? userName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Имя не может быть пустым. Попробуйте снова или введите '0' для отмены:");
                Console.ResetColor();
                continue;
            }

            if (userName == "0")
            {
                return; // Возврат в предыдущее меню
            }

            // Проверка формата "Имя Фамилия"
            if (!userName.Contains(' ') || userName.Split(' ').Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверный формат. Введите имя и фамилию через один пробел или '0' для отмены:");
                Console.ResetColor();
                continue;
            }

            bool isDeletionSuccessful = _usersService.RemoveUser(userName);

            if (isDeletionSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Пользователь '{userName}' успешно удален.\n");
                Console.ResetColor();
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Не удалось удалить пользователя '{userName}' или пользователь не найден.\n");
                Console.ResetColor();
                return;
            }
        }
    }
}