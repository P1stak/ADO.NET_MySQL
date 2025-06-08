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

DELIMITER //
CREATE PROCEDURE get_user_social_info(IN user_name VARCHAR(50))
BEGIN
    SELECT sp.name, usp.connect_url        
    FROM users AS u    
    JOIN user_social_providers AS usp ON u.id = usp.user_id
    JOIN social_providers AS sp ON sp.id = usp.social_provider_id
    WHERE u.full_name = user_name
    ORDER BY sp.name;
END //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE get_user_social_info(IN user_name VARCHAR(50))
BEGIN
    SELECT sp.name, usp.connect_url        
    FROM users AS u    
    JOIN user_social_providers AS usp ON u.id = usp.user_id
    JOIN social_providers AS sp ON sp.id = usp.social_provider_id
    WHERE u.full_name = user_name
    ORDER BY sp.name;
END //
DELIMITER ;
 */