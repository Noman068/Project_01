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

        while (true)
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

            int choice = -1;

            while (true)
            {
                Console.Write("\n\nEnter your choice: ");
                string input = Console.ReadLine();

                try
                {
                    choice = Convert.ToInt32(input);
                    if (choice >= 0 && choice <= 10)
                        break;

                    Console.WriteLine("Please enter a valid choice between 0 and 10.");
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }

            Console.WriteLine();

            switch (choice)
            {
                case 1: manager.AddVisit(); break;
                case 2: manager.UpdateVisit(); break;
                case 3: manager.DeleteVisit(); break;
                case 4: manager.SearchVisit(); break;
                case 5: manager.ShowAllVisits(); break;
                case 6: manager.ReportByVisitType(); break;
                case 7: manager.ReportWeeklySummary(); break;
                case 8: manager.Undo(); break;
                case 9: manager.Redo(); break;
                case 10: manager.ShowVisitSummaryByName(); break;
                case 0:
                    Console.WriteLine("\n\n......You have exited the program....\n");
                    return;
            }
        }
    }
}
