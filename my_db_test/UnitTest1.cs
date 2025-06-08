namespace my_db_test
{
    public class CalculatorTests
    {
        // Предположим, что данный метод располагается в реальном проекте и его нужно протестировать
        public class Calculator
        {
            public int Add(int a, int b)
            {
                return a + b;
            }
        }

        [Fact] // Атрибут, указывающий, что это тестовый метод
        public void Add_ShouldReturnCorrectSum()
        {
            // Arrange (Подготовка)
            var calculator = new Calculator();
            int a = 5;
            int b = 3;
            int expectedSum = 8;

            // Act (Действие)
            int actualSum = calculator.Add(a, b);

            // Assert (Проверка)
            Assert.Equal(expectedSum, actualSum);
        }
    }
}