public class Program
{
    public static void Main()
    {
        var menu = new MainMenu();
        menu.Display();
        menu.HandleUserChoice();
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