using System;

public enum Role
{
    admin,
    receptionist
}

public class UserRole
{
    public  string role ;

    public  bool RoleManager()
    {
        
        Console.Write("Enter your role (1 for recepionist & 2 for Admin) : ");
        int roleinput = Validator.ChoiceValidator(Console.ReadLine(), '1', '2');

        Console.Write("Enter UserName : ");
        string username = Console.ReadLine();
        Console.Write("Enter Password : ");
        string password = Console.ReadLine();
        
        if (roleinput == 1 && username=="noman" && password=="curemd")
        {
            role = Convert.ToString(Role.receptionist);
            return true;

        }else if(roleinput == 2 && username == "noman" && password == "curemd123")
        {
            role=Convert.ToString(Role.admin);
            return true;
        }

        
        return false;
    }
}
