using ADO.NET_test.Models;
using ADO.NET_test.Services;
using System.Data;

public class CoursesServiceTests
{
    private readonly CoursesService _coursesService = new();

    [Fact]
    public void Get_ShouldReturnListOfCourses_WhenUserExists() // тест на получение курсов для существующего пользователя в БД
    {
        // список курсов
        var expectedCourses = new List<Course>
            {
            new Course { Id = 3, Title = "PHP для начинающих", Summary = "Курс по созданию динамических сайтов", Photo = "https://example.com/course6.jpg" },
            new Course { Id = 2, Title = "Введение в HTML и CSS", Summary = "Курс по верстке сайтов", Photo = "https://example.com/course5.jpg" },
            new Course { Id = 1, Title = "JavaScript для начинающих", Summary = "Курс для начинающих веб-разработчиков", Photo = "https://example.com/course4.jpg" }
            };

        // вызов метода Get сервиса с именем пользователя из БД
        var resultCourses = _coursesService.Get("Петр Васильев");

        // проверка соответствий
        for (int i = 0; i < expectedCourses.Count; i++)
        {
            Assert.Equal(expectedCourses[i].Id, resultCourses[i].Id);
            Assert.Equal(expectedCourses[i].Title, resultCourses[i].Title);
            Assert.Equal(expectedCourses[i].Summary, resultCourses[i].Summary);
            Assert.Equal(expectedCourses[i].Photo, resultCourses[i].Photo);
        }
    }

    [Fact]
    public void Get_ShouldReturnEmptyList_WhenUserDoesNotExist() // тест на получение курсов для несуществующего пользователя из БД
    {
        // ожидаемый пустой список при отсутствии пользователя в БД
        var expectedCourses = new List<Course>();

        // вызываем метод Get() сервиса с именем несуществующего пользователя
        var resultCourses = _coursesService.Get("aboba test");

        // чек на возвращенный пустой список
        Assert.Equal(expectedCourses.Count, resultCourses.Count);
    }

    [Fact]
    public void GetTotalCount_ShouldReturnCorrectCount() // тест на получение общего количества курсов
    {
        // ожидаемое количество курсов
        var expectedTotalCount = 32;

        // вызывается метод GetTotalCount сервиса курсов
        var resultTotalCount = _coursesService.GetTotalCount();

        // чек соответствия возвращаемого кол-ва курсов 
        Assert.Equal(expectedTotalCount, resultTotalCount);
    }
}