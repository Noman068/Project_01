using System;

public class Receptionist : Employee , IReceptionist
{


    private  IVisitService visitService;
    private  string currentRole;
 

    public Receptionist(string currentRole)
    {
        IVisitRepository visitRepository = new CsvVisitRepository();
        ICommandManager commandManager = new CommandManager();
        IActivityLogger activityLogger = new FileActivityLogger();
        this.visitService = new VisitService(visitRepository, commandManager, activityLogger);
        this.currentRole = currentRole;
        RunApplication();
    }



    public void RunApplication()
    {
        while (true)
        {
            DisplayMenu();
            int maxChoice = 5;
            int choice = Validator.ChoiceValidator(0, maxChoice);

            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    AddVisit();
                    break;
                case 2:
                    Console.WriteLine("\n\n......You have exited the program.... Because 2 was not in option for receptionist....\n");
                    return;
                case 3:
                    Console.WriteLine("\n\n......You have exited the program....Because 3 was not in option for receptionist....\n");
                    return;
                case 4:
                    SearchVisit();
                    break;
                case 5:
                    ShowAllVisits();
                    break;
                case 0:
                    Console.WriteLine("\n\n......Receptionist have exited the program....\n");
                    return;
            }
        }
    }



    public void DisplayMenu()
    {
        Console.WriteLine("\n===== Patient Visit Manager =====");
        Console.WriteLine("Enter 0 to Exit Program.");
        Console.WriteLine("Enter 1 to Add Visit.");
        Console.WriteLine("Enter 4 to Search Visit.");
        Console.WriteLine("Enter 5 to Show All Visits.");

    }


    public void AddVisit()
    {
        Console.Write("Enter patient name: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);

        Console.Write("Enter visit date (dd-MM-yyyy): ");
        string date = Console.ReadLine();
        date = Validator.DateValidator(date);

        Console.Write("Enter Time in format hh:mm : ");
        string time = Console.ReadLine();
        time = Validator.TimeValidator(time);

        Console.Write("Enter visit type: ");
        string type = Console.ReadLine();
        type = Validator.TypeValidator(type);

        Console.Write("Enter visit duration (in minutes): ");
        string duration = Console.ReadLine();
        duration = Validator.NumberValidator(duration);

        Console.Write("Enter notes: ");
        string notes = Console.ReadLine();

        Console.Write("Enter doctor name: ");
        string doctor = Console.ReadLine();
        doctor = Validator.NameValidator(doctor);

        Visit visit = new Visit(name, date, time, type, notes, doctor, duration);

        if (visitService.AddVisit(visit, currentRole))
        {
            Console.WriteLine("\nVisit added successfully by Receptionist!\n");
        }
        else
        {
            Console.WriteLine("\nFailed to add visit by Receptionist . Please check your input.\n");
        }
    }



    public void SearchVisit()
    {
        Console.WriteLine("Search by: 1. Patient  2. Doctor  3. Date  4. Visit Type");
        int choice = Validator.ChoiceValidator(1, 4);

        string keyword = "";
        switch (choice)
        {
            case 1:
                Console.Write("Enter Patient name: ");
                keyword = Console.ReadLine();
                keyword = Validator.NameValidator(keyword);
                break;
            case 2:
                Console.Write("Enter doctor name: ");
                keyword = Console.ReadLine();
                keyword = Validator.NameValidator(keyword);
                break;
            case 3:
                Console.Write("Enter date (dd-MM-yyyy): ");
                keyword = Console.ReadLine();
                keyword = Validator.DateValidator(keyword);
                break;
            case 4:
                Console.Write("Enter visit type: ");
                keyword = Console.ReadLine();
                keyword = Validator.TypeValidator(keyword);
                break;
        }

        List<Visit> results = visitService.SearchVisits(keyword, choice, currentRole);

        if (results.Count > 0)
        {
            Console.WriteLine("\nSearch Results for receptionist:\n");
            foreach (Visit visit in results)
            {
                Console.WriteLine(visit.Display());
                Console.WriteLine("***************************");
            }
            Console.WriteLine($"Total visits found: {results.Count}");
        }
        else
        {
            Console.WriteLine("No records found.\n");
        }
    }


       public void ShowAllVisits()
        {
            List<Visit> visits = visitService.GetAllVisits(currentRole);

            if (visits.Count > 0)
            {
                Console.WriteLine("All Visits for receptionist:\n");
                foreach (Visit visit in visits)
                {
                    Console.WriteLine(visit.Display());
                    Console.WriteLine("-----------------------------------");
                }
            }
            else
            {
                Console.WriteLine("No records available.\n");
            }
        }


    }
