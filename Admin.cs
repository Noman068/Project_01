using System;
using System.Runtime.InteropServices.ComTypes;

public class Admin : Employee, IAdmin
{
    private IVisitService visitService;
    private string currentRole;


    public Admin(string currentRole)
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
            int maxChoice = 11;
            int choice = Validator.ChoiceValidator(0, maxChoice);

            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    AddVisit();
                    break;
                case 2:
                        UpdateVisit();
                    break;
                case 3:
                        DeleteVisit();
                    break;
                case 4:
                    SearchVisit();
                    break;
                case 5:
                    ShowAllVisits();
                    break;
                case 6:
                        ReportByVisitType();
                    break;
                case 7:
                        ReportWeeklySummary();
                    break;
                case 8:
                        UndoAction();
                    break;
                case 9:
                        RedoAction();
                    break;
                case 10:
                        ShowVisitSummaryByName();
                    break;
                case 11:
                        FeeCalculator();
                    break;
                case 0:
                    Console.WriteLine("\n\n......Admin have exited the program....\n");
                    return;
            }
        }
    }



    public void DisplayMenu()
    {
            Console.WriteLine("\n===== Patient Visit Manager =====");
            Console.WriteLine("Enter 0 to Exit Program.");
            Console.WriteLine("Enter 1 to Add Visit.");
            Console.WriteLine("Enter 2 to Update Visit.");
            Console.WriteLine("Enter 3 to Delete Visit.");
            Console.WriteLine("Enter 4 to Search Visit.");
            Console.WriteLine("Enter 5 to Show All Visits.");
            Console.WriteLine("Enter 6 for Report by Visit Type.");
            Console.WriteLine("Enter 7 for Weekly Summary.");
            Console.WriteLine("Enter 8 to Undo Last Action.");
            Console.WriteLine("Enter 9 to Redo Last Action.");
            Console.WriteLine("Enter 10 to View Visit Summary by Patient Name.");
            Console.WriteLine("Enter 11 to View Visit Fee by Visit type.");

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
            Console.WriteLine("\nVisit added successfully  by Admin!\n");
        }
        else
        {
            Console.WriteLine("\nFailed to add visit  by Admin. Please check your input.\n");
        }
    }



      public void UpdateVisit()
    {
        Console.Write("Enter patient name to update: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);

        Console.Write("Enter new visit date: ");
        string VisitDate = Validator.DateValidator(Console.ReadLine());

        Console.Write("Enter Time in format: ");
        string Time1 = Validator.TimeValidator(Console.ReadLine());

        Console.Write("Enter new visit type: ");
        string type = Console.ReadLine();
        type = Validator.TypeValidator(type);

        Console.Write("Enter new visit duration (in minutes) : ");
        string duration = Console.ReadLine();
        duration = Validator.NumberValidator(duration);

        Console.Write("Enter new notes: ");
        string notes = Console.ReadLine();

        Console.Write("Enter new doctor name: ");
        string doctor = Console.ReadLine();
        doctor = Validator.NameValidator(doctor);

        Visit updatedVisit = new Visit(name, VisitDate, Time1, type, notes, doctor, duration);

        if (visitService.UpdateVisit(name, updatedVisit, currentRole))
        {
            Console.WriteLine("\nVisit updated successfully by Admin!\n");
        }
        else
        {
            Console.WriteLine("\nFailed to update visit by Admin. Please check your input.\n");
        }
    }


    public void DeleteVisit()
    {
        Console.Write("Enter patient name to delete: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);

        if (visitService.DeleteVisit(name, currentRole))
        {
            Console.WriteLine("\nVisit deleted successfully by Admin!\n");
        }
        else
        {
            Console.WriteLine("\nVisit not found.\n");
        }
    }


    public void ReportByVisitType()
    {
        Dictionary<string, int> report = visitService.GetVisitTypeReport();
        Console.WriteLine("Visit Count by Type for Admin:\n");
        foreach (var kvp in report)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        Console.WriteLine();
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
            Console.WriteLine("\nSearch Results for Admin:\n");
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
            Console.WriteLine("All Visits for Admin:\n");
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




    public void ReportWeeklySummary()
    {
        List<Visit> weeklyVisits = visitService.GetWeeklySummary();

        if (weeklyVisits.Count > 0)
        {
            Console.WriteLine("Visits in the Last 7 Days for Admin:\n");
            foreach (Visit visit in weeklyVisits)
            {
                Console.WriteLine(visit.Display());
                Console.WriteLine("***************************");
            }
        }
        else
        {
            Console.WriteLine("No visits in the past 7 days.\n");
        }
    }




    public void UndoAction()
    {
        visitService.Undo(currentRole);
        Console.WriteLine("Undo completed by Admin.\n");
    }


    public void RedoAction()
    {
        visitService.Redo(currentRole);
        Console.WriteLine("Redo completed by Admin.\n");
    }


    public void ShowVisitSummaryByName()
    {
        Console.Write("Enter patient name to view summary: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);

        List<Visit> visits = visitService.GetVisitsByPatient(name);

        if (visits.Count > 0)
        {
            Console.WriteLine("\nVisit Summary for Admin:\n");
            foreach (Visit visit in visits)
            {
                Console.WriteLine(visit.Display());
                Console.WriteLine("***************************");
            }
            Console.WriteLine($"Total Visits by {name}: {visits.Count}");
        }
        else
        {
            Console.WriteLine("\nNo visit found for that patient.\n");
        }
    }



    public void FeeCalculator()
    {
        int Done = 0;
        Console.Write("Enter Type = ");
        string type = Console.ReadLine();
        type = Validator.TypeValidator(type);
        type=type.ToLower();
        try
        {
            string json = System.IO.File.ReadAllText("fees.json");
            var servicePrices = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(json);

            if (servicePrices.TryGetValue(type, out int price))
            {
                Done++;
                Console.WriteLine("***************************");
                Console.WriteLine($"\nPrice accessed by Admin: {price}");
                Console.WriteLine("***************************");
            }
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error reading fees: {ex.Message}");
        }
        if( Done > 0)
        {
            IActivityLogger activityLogger = new FileActivityLogger();
            activityLogger.LogActivity("fee check", true, currentRole, Convert.ToString(DateTime.Now));
        }

    }



}
