public class Validator
{


    public static string TimeValidator(string input)
    {
        while (true)
        {
            int isValid = 0;
            string check = "0123456789:";
            for (int i = 0; i < input.Length; i++)
            {
                if (!(check.Contains(input[i])))
                {
                    isValid++;
                    break;
                }
            }

            if (input[2] != ':' || input.Length != 5)
            {
                isValid++;
            }

            if (isValid == 0) {
                string[] part = input.Split(':');
                int[] ints = new int[part.Length];
                for (int i = 0; i < part.Length; i++)
                {
                    ints[i] = Convert.ToInt32(part[i]);
                }
                if (ints[0] < 0 || ints[0] > 23 || ints[1] < 0 || ints[1] > 59)
                {
                    isValid++;
                }
            }


            if (isValid != 0)
            {
                Console.Write("Enter correct time format hh:mm : ");
                input = Console.ReadLine();
            } else
            {
                break;
            }
        }
        return input;
    }

    public static string NumberValidator(string input)
    {

        while(true)
        {
            int isValid = 0;
            string test = "0123456789";
            for(int i = 0; i < input.Length; i++)
            {
                if (!(test.Contains(input[i])))
                {
                    isValid++;
                    break;
                }
            }
            if (isValid != 0)
            {
                Console.Write("\nInvalid Duration.Please enter a Number = ");
                input = Console.ReadLine();
            }
            else
            {
                break;
            }
        }

        return  input;

    }

    public static int ChoiceValidator(int minChoice, int maxChoice)
    {
        int choice = -1;

        while (true)
        {
            Console.Write("\n\nEnter your choice: ");
            string input = Console.ReadLine();

            try
            {
                choice = Convert.ToInt32(input);
                if (choice >= minChoice && choice <= maxChoice)
                    break;

                Console.Write($"Please enter a valid choice between {minChoice} and {maxChoice} :");
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
        return choice;
    }



    public static string NameValidator(string name)
    {
        while (true)
        {


            int isValid = 0;



            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
                {
                    isValid++;
                    break;
                }
            }
            if (isValid == 0)
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

            int isValid = 0;

            for (int i = 0; i < date.Length; i++)
            {
                char c = date[i];
                if (!((c >= '0' && c <= '9') || c == '-'))
                {
                    isValid++;
                    break;
                }

            }

            if (date.Length == 10 && date[2] == '-' && date[5] == '-' && isValid == 0)
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
                isValid++;

            }
            if (isValid == 0)
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