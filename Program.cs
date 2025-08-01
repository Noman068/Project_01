using System;



public class Program
{
    
    
    static void Main(string[] args)
    {

        Console.WriteLine("*************************************");
        Console.WriteLine("*   CureMD - Patient Visit Manager  *");
        Console.WriteLine("*         by Noman Nawaz Cheema    *");
        Console.WriteLine("*************************************\n");

        VisitManager manager = new VisitManager();
        //manager.GenerateData();
        UserRole RoleObj= new UserRole();
        if (!RoleObj.RoleManager())
        {
            Console.WriteLine("\nValidation failed...\n");
            return;

        }

            while (true)
            {



                Console.WriteLine("\n===== Patient Visit Manager =====");

                Console.WriteLine("Enter 0 to Exit Program.");
                Console.WriteLine("Enter 1 to Add Visit.");
                if(RoleObj.role == "admin")
                Console.WriteLine("Enter 2 to Update Visit.");
                if (RoleObj.role == "admin")
                Console.WriteLine("Enter 3 to Delete Visit.");
                Console.WriteLine("Enter 4 to Search Visit.");
                Console.WriteLine("Enter 5 to Show All Visits.");
                if (RoleObj.role == "admin")
                Console.WriteLine("Enter 6 for Report by Visit Type.");
                if (RoleObj.role == "admin")
                Console.WriteLine("Enter 7 for Weekly Summary.");
                if (RoleObj.role == "admin")
                Console.WriteLine("Enter 8 to Undo Last Action.");
                if (RoleObj.role == "admin")
                Console.WriteLine("Enter 9 to Redo Last Action.");
                if (RoleObj.role == "admin")
                Console.WriteLine("Enter 10 to View Visit Summary by Patient Name.");
                if (RoleObj.role == "admin")
                Console.WriteLine("Enter 11 to View Visit Fee by Visit type.\"");

                int choice = -1;

                while (true)
                {
                    Console.Write("\n\nEnter your choice: ");
                    string input = Console.ReadLine();

                    try
                    {
                        choice = Convert.ToInt32(input);
                        if (choice >= 0 && choice <= 11)
                            break;

                        Console.WriteLine("Please enter a valid choice between 0 and 11.");
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                    }
                }

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                    manager.AddVisit(RoleObj.role); break;
                    case 2:
                    if (RoleObj.role == "admin")
                        manager.UpdateVisit();
                    break;
                    case 3:
                    if (RoleObj.role == "admin")
                        manager.DeleteVisit(); 
                    break;
                    case 4:
                    manager.SearchVisit(RoleObj.role); break;
                    case 5:
                    manager.ShowAllVisits(RoleObj.role); break;
                    case 6:
                    if (RoleObj.role == "admin")
                        manager.ReportByVisitType();
                    break;
                    case 7:
                    if (RoleObj.role == "admin")
                        manager.ReportWeeklySummary();
                    break;
                    case 8:
                    if (RoleObj.role == "admin")
                        manager.Undo(); 
                    break;
                    case 9:
                    if (RoleObj.role == "admin")
                        manager.Redo(); 
                    break;
                    case 10:
                    if (RoleObj.role == "admin")
                        manager.ShowVisitSummaryByName(); 
                    break;
                    case 11:
                    if (RoleObj.role == "admin")
                        manager.FeeCalculator();
                    break;
                    case 0:
                        Console.WriteLine("\n\n......You have exited the program....\n");
                        return;
                }
            }
        }
    }
    
