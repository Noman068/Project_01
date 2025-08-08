using System;

public class Employee : IEmployee
{
    public string role;

    public bool AuthenticateUser()
    {
        Console.Write("Enter your role (1 for Receptionist & 2 for Admin): ");
        int roleInput = Validator.ChoiceValidator(1, 2);

        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        if (roleInput == 1 && username == "noman" && password == "curemd")
        {
            role = "receptionist";
            return true;
        }
        else if (roleInput == 2 && username == "noman" && password == "curemd123")
        {
            role = "admin";
            return true;
        }

        return false;
    }

}
