using ADO.NET_test.Models;
using ADO.NET_test.Services;
using MySql.Data.MySqlClient;
using Mysqlx.Prepare;
using Org.BouncyCastle.Asn1.Cmp;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main()
    {
        bool isRunnig = true;
        while (isRunnig)
        {
            Console.WriteLine(@"
************************************************
* Добро пожаловать на онлайн платформу Stepik! *
************************************************

Выберите действие (введите число и нажмите Enter):

1. Войти
2. Зарегистрироваться
3. Удаление пользователя из БД
4. Закрыть приложение

************************************************
");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    LoginUser();
                    break;
                case "2":
                    RegisterUser();
                    break;
                case "3":
                    DeleteUser();
                    break;
                case "4":
                    Console.WriteLine("До свидания!");
                    isRunnig = false;
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
    public static void RegisterUser()
    {
        Console.WriteLine("Введите имя и фамилию через пробел и нажмите Enter:");
        string fullName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(fullName))
        {
            Console.WriteLine("Произошла ошибка, произведен выход на главную страницу\n");
            return;
        }

        User newUser = new User()
        {
            FullName = fullName,
            JoinDate = DateTime.Now,
            IsActive = true
        };

        try
        {
            bool isAdded = UsersService.Add(newUser);

            if (isAdded)
            {
                Console.WriteLine($"Пользователь '{fullName}' успешно добавлен.\n");
            }
            else
            {
                Console.WriteLine("Произошла ошибка, произведен выход на главную страницу\n");
            }
        }
        catch
        {
            Console.WriteLine("Произошла ошибка, произведен выход на главную страницу\n");
        }
    }

    public static void LoginUser()
    {
        Console.WriteLine("Введите имя и фамилию через пробел и нажмите Enter:");
        string fullName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(fullName))
        {
            Console.WriteLine("Пользователь не найден, произведен выход на главную страницу\n");
            return;
        }

        User user = UsersService.Get(fullName.Trim());

        if (user != null)
        {
            Console.WriteLine($"Пользователь '{user.FullName}' успешно вошел\n");
        }
        else
        {
            Console.WriteLine("Пользователь не найден, произведен выход на главную страницу\n");
        }

    }

    public static void DeleteUser()
    {
        Console.WriteLine("Введите имя и фамилию через пробел удаляемого пользователя:");
        string fullName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(fullName))
        {
            Console.WriteLine("Имя не может быть пустым\n");
            return;
        }

        bool isDeleted = UsersService.RemoveUser(fullName.Trim());

        if (isDeleted)
        {
            Console.WriteLine($"Пользователь '{fullName}' успешно удален\n");
        }
        else
        {
            Console.WriteLine("Пользователь не найден или уже удален\n");
        }
    }
}