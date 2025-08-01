using System;
using System.Collections.Generic;
using System.IO;

public class VisitManager
{
    private string filePath = "visits.csv";
    private Stack<CommandDealer> undoStack = new Stack<CommandDealer>();
    private Stack<CommandDealer> redoStack = new Stack<CommandDealer>();
    private const int MaxUndo = 10;

    private List<Visit> LoadVisits()
    {
        List<Visit> visits = new List<Visit>();
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                Visit visit = Visit.CommaDel(line);
                if (visit != null)
                    visits.Add(visit);
            }
        }
        return visits;
    }

    private void SaveVisits(List<Visit> visits)
    {
        List<string> lines = new List<string>();
        foreach (Visit visit in visits)
        {
            lines.Add(visit.CommaAdd());
        }
        File.WriteAllLines(filePath, lines);
    }

    

    public void AddVisit()
    {

        Console.Write("Enter patient name: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);


        Console.Write("Enter visit date (dd-MM-yyyy): ");
        string date = Console.ReadLine();

        date = Validator.DateValidator(date);

        Console.Write("Enter visit type: ");
        string type = Console.ReadLine();
        type = Validator.TypeValidator(type);
        

        Console.Write("Enter notes: ");
        string notes = Console.ReadLine();

        Console.Write("Enter doctor name: ");
        string doctor = Console.ReadLine();
        doctor = Validator.NameValidator(doctor);


        Visit visit = new Visit(name, date, type, notes, doctor);
        List<Visit> visits = LoadVisits();
        visits.Add(visit);
        SaveVisits(visits);

        undoStack.Push(new CommandDealer("Add", visit));
        if (undoStack.Count > MaxUndo) undoStack = new Stack<CommandDealer>(undoStack.ToArray()[..MaxUndo]);
        redoStack.Clear();

        Console.WriteLine("\n Visit added successfully!\n");
    }

    public void ShowAllVisits()
    {
        List<Visit> visits = LoadVisits();
        Console.WriteLine("All Visits:\n");
        foreach (Visit visit in visits)
        {
            Console.WriteLine(visit.Display());
            Console.WriteLine("-----------------------------------");
        }
        if (visits.Count == 0)
            Console.WriteLine("No records available.\n");
    }

    public void SearchVisit()
    {
        Console.WriteLine("Search by: 1. Patient  2. Doctor  3. Date  4. Visit Type");
        Console.Write("Enter your choice: ");
        string input = Console.ReadLine();
        
        int choice = Validator.ChoiceValidator(input,'1','4');

        string keyword = "";

        switch (choice)
        {
            case 1:
                Console.Write("Enter Pateint name : ");
                keyword = Console.ReadLine();
                keyword = Validator.NameValidator(keyword);

                break;
            case 2:

                Console.Write("Enter doctor name: ");
                keyword = Validator.NameValidator(keyword);

                break;
            case 3:

                Console.Write("Enter date(dd-mm-yyyy): ");
                keyword = Console.ReadLine();
                keyword = Validator.DateValidator(keyword);

                break;
            case 4:

                Console.Write("Enter visit type: ");
                keyword = Console.ReadLine();
                keyword = Validator.TypeValidator(keyword);
                break;
        }

        List<Visit> visits = LoadVisits();
        bool found = false;
        Console.Write("\n\n");
        int i=0;
        foreach (Visit visit in visits)
        {
            if ((choice == 1 && visit.PatientName.ToLower() == (keyword.ToLower())) ||
                (choice == 2 && visit.DoctorName.ToLower() == (keyword.ToLower())) ||
                (choice == 3 && visit.VisitDate == (keyword)) ||
                (choice == 4 && visit.VisitType.ToLower() == (keyword.ToLower())))
            {

                Console.WriteLine(visit.Display());
                Console.WriteLine("***************************");
                found = true;
                i++;
            }
        }

        if (!found)
        {
            Console.WriteLine("No records found.\n");
        }else
        {
            Console.WriteLine($"Total visits: {i}");
            Console.WriteLine("***************************");
        }  

    }

    public void UpdateVisit()
    {
        Console.Write("Enter patient name to update: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);

        List<Visit> visits = LoadVisits();
        bool updated = false;

        for (int i = 0; i < visits.Count; i++)
        {
            if (visits[i].PatientName.ToLower() == name.ToLower())
            {
                Visit oldVisit = new Visit(visits[i].PatientName, visits[i].VisitDate, visits[i].VisitType, visits[i].Notes, visits[i].DoctorName);

                Console.Write("Enter new visit date: ");
                visits[i].VisitDate = Console.ReadLine();
                visits[i].VisitDate = Validator.DateValidator(visits[i].VisitDate);


                

                Console.Write("Enter new visit type: ");
                visits[i].VisitType = Console.ReadLine();
                visits[i].VisitType = Validator.TypeValidator(visits[i].VisitType);

                

                Console.Write("Enter new notes: ");
                visits[i].Notes = Console.ReadLine();

                Console.Write("Enter new doctor name: ");
                visits[i].DoctorName = Console.ReadLine();
                visits[i].DoctorName = Validator.NameValidator(visits[i].DoctorName);

                undoStack.Push(new CommandDealer("Update", visits[i], oldVisit));
                if (undoStack.Count > MaxUndo) undoStack = new Stack<CommandDealer>(undoStack.ToArray()[..MaxUndo]);
                redoStack.Clear();

                updated = true;
                break;
            }
        }

        if (updated)
        {
            SaveVisits(visits);
            Console.WriteLine("\n Visit updated successfully!\n");
        }
        else
        {
            Console.WriteLine("\n Visit not found.\n");
        }
    }

    public void DeleteVisit()
    {
        Console.Write("Enter patient name to delete: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);


        List<Visit> visits = LoadVisits();
        bool deleted = false;

        for (int i = 0; i < visits.Count; i++)
        {
            if (visits[i].PatientName.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                undoStack.Push(new CommandDealer("Delete", visits[i]));
                if (undoStack.Count > MaxUndo) undoStack = new Stack<CommandDealer>(undoStack.ToArray()[..MaxUndo]);
                redoStack.Clear();

                visits.RemoveAt(i);
                deleted = true;
                break;
            }
        }

        if (deleted)
        {
            SaveVisits(visits);
            Console.WriteLine("\n Visit deleted successfully!\n");
            Console.WriteLine("\n***************************\n");
        }
        else
        {
            Console.WriteLine("\n Visit not found.\n");
        }
    }


    public void ShowVisitSummaryByName()
    {
        Console.Write("Enter patient name to view summary: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);


        List<Visit> visits = LoadVisits();
        bool found = false;
        int count = 0;
        Console.WriteLine("\n Visit Summary:\n");
        foreach (Visit visit in visits)
        {
            if (visit.PatientName.ToLower() == name.ToLower())
            {
                
                Console.WriteLine(visit.Display());
                Console.WriteLine("***************************");
                found = true;
                count++;
            }
        }

        if (found)
        {
            Console.WriteLine($"Total Visits by {name} : {count}");
            Console.WriteLine("***************************");

        }

        if (!found)
        {
            Console.WriteLine("\n No visit found for that patient.\n");
        }
    }

    public void ReportByVisitType()
    {
        List<Visit> visits = LoadVisits();
        Dictionary<string, int> counts = new Dictionary<string, int>();

        foreach (Visit v in visits)
        {
            string type = v.VisitType.ToLower();
            if (!counts.ContainsKey(type)) counts[type] = 0;
            counts[type]++;
        }

        Console.WriteLine(" Visit Count by Type:\n");
        foreach (var kvp in counts)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        Console.WriteLine();
    }

    public void ReportWeeklySummary()
    {
        List<Visit> visits = LoadVisits();
        DateTime today = DateTime.Today;
        int count = 0;

        Console.WriteLine("Visits in the Last 7 Days:\n");
        foreach (Visit v in visits)
        {
            if (DateTime.TryParse(v.VisitDate, out DateTime visitDate))
            {
                if ((today - visitDate).TotalDays <= 7 && (today - visitDate).TotalDays >= 0)
                {
                    Console.WriteLine(v.Display());
                    Console.WriteLine("***************************");
                    count++;
                }
            }
        }

        if (count == 0)
        {
            Console.WriteLine("No visits in the past 7 days.\n");
        }
    }

    public void Undo()
    {
        if (undoStack.Count == 0)
        {
            Console.WriteLine("Nothing to undo.\n");
            return;
        }

        CommandDealer cmd = undoStack.Pop();
        List<Visit> visits = LoadVisits();

        if (cmd.Action == "Add")
        {
            visits.RemoveAll(v => v.PatientName == cmd.Visit.PatientName);
        }
        else if (cmd.Action == "Delete")
        {
            visits.Add(cmd.Visit);
        }
        else if (cmd.Action == "Update")
        {
            for (int i = 0; i < visits.Count; i++)
            {
                if (visits[i].PatientName == cmd.Visit.PatientName)
                {
                    visits[i] = cmd.Previous;
                    break;
                }
            }
        }

        SaveVisits(visits);
        redoStack.Push(cmd);
        Console.WriteLine(" Undo completed.\n");
    }

    public void Redo()
    {
        if (redoStack.Count == 0)
        {
            Console.WriteLine(" Nothing to redo.\n");
            return;
        }

        CommandDealer cmd = redoStack.Pop();
        List<Visit> visits = LoadVisits();

        if (cmd.Action == "Add")
        {
            visits.Add(cmd.Visit);
        }
        else if (cmd.Action == "Delete")
        {
            visits.RemoveAll(v => v.PatientName == cmd.Visit.PatientName);
        }
        else if (cmd.Action == "Update")
        {
            for (int i = 0; i < visits.Count; i++)
            {
                if (visits[i].PatientName == cmd.Previous.PatientName)
                {
                    visits[i] = cmd.Visit;
                    break;
                }
            }
        }

        SaveVisits(visits);
        undoStack.Push(cmd);
        Console.WriteLine("Redo completed.\n");
    }


}