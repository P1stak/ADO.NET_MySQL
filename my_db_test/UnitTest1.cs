namespace my_db_test
{
    public class CalculatorTests
    {
        // �����������, ��� ������ ����� ������������� � �������� ������� � ��� ����� ��������������
        public class Calculator
        {
            public int Add(int a, int b)
            {
                return a + b;
            }
        }

        [Fact] // �������, �����������, ��� ��� �������� �����
        public void Add_ShouldReturnCorrectSum()
        {
            // Arrange (����������)
            var calculator = new Calculator();
            int a = 5;
            int b = 3;
            int expectedSum = 8;

            // Act (��������)
            int actualSum = calculator.Add(a, b);

            // Assert (��������)
            Assert.Equal(expectedSum, actualSum);
        }
    }
}