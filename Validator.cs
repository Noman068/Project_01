public class Validator
{



    public static int ChoiceValidator(string input, char minChoice , char maxChoice)
{
        while (true)
        {
            bool isValid = true;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (!(c >= minChoice && c <= maxChoice))
                {
                    isValid = false;
                    break;
                }

            }
            if (!isValid)
            {
                Console.WriteLine($"Invalid choice! Please enter a number between {minChoice} and {maxChoice}.");
                Console.Write("Enter your choice: ");
                input = Console.ReadLine();
                
            }
            else
            {
                break;
            }

        }
        return Convert.ToInt32(input);
    }




    public static string NameValidator(string name)
    {
        while (true)
        {


            bool isValid = true;

            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z' || c == ' ')))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid name. Please enter a valid name containing only letters.");
            }

            Console.Write("Enter patient name: ");
            name = Console.ReadLine();

        }
       

       return name;
            

    }

    public static string DateValidator(string date)
    {
        while (true)
        {

            bool isValid = true;

            for (int i = 0; i < date.Length; i++)
            {
                char c = date[i];
                if (!((c >= '0' && c <= '9') || c == '-'))
                {
                    isValid = false;
                    break;
                }

            }

            if (date.Length == 10 && date[2] == '-' && date[5] == '-' && isValid)
            {
                string[] part = date.Split('-');
            int[] parts = new int[3];
            for (int i = 0; i < part.Length; i++)
            {
                parts[i] = Convert.ToInt32(part[i]);
            }

            if ((parts[0] < 0 || parts[0] > 31 || parts[1] < 0 || parts[1] > 12 || parts[2] > DateTime.Now.Year + 1 || parts[2] < DateTime.Now.Year - 10 || (parts[0] == 31 && (parts[1] == 2 || parts[1] == 4 || parts[1] == 6 || parts[1] == 9 || parts[1] == 11)) || (parts[0] == 30 && parts[1] == 02) || (parts[0] == 29 && parts[1] == 02 && parts[2] % 4 != 0)))
            {
                Console.WriteLine("There was an issue in years or mothns or days ");
                isValid = false;

            }
            if (isValid)
            {
                return date;
            }
            }

            

            

            Console.WriteLine("Invalid date format! Please enter date in dd-MM-yyyy format.");
            Console.Write("Enter visit date (dd-MM-yyyy): ");
            date = Console.ReadLine();


        }        return date;
    }


   public static string TypeValidator(string type)
    {
        while (true)
        {


            if (type == "Consultation" || type == "consultation" || type == "Follow-up" || type == "follow-up" || type == "Emergency" || type == "emergency")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid visit type! Please enter 'Consultation' 'Follow-up' or 'Emergency'.");
                Console.Write("Enter visit type: ");
                type = Console.ReadLine();
            }
        }
        return type;
    }



}