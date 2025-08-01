using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class VisitManager
{

    private string filePath = "visits.csv";
    private Stack<CommandDealer> undoStack = new Stack<CommandDealer>();
    private Stack<CommandDealer> redoStack = new Stack<CommandDealer>();
    private const int MaxUndo = 10;
    public ActivityLog log= new ActivityLog();

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

    public void GenerateData()
    {
        string[] visitTypes = { "Consultation", "Emergency", "Follow-up" };
        string[] names = { "Alice", "Bob", "Charlie", "David", "Emma" };
        string[] visitDates = { "01-08-2025", "02-08-2025", "03-08-2025", "04-08-2025", "05-08-2025" };
        string[] notes = { "No issues", "Monitor BP", "Discussed symptoms", "Critical case", "Improving" };
        string[] doctorNames = { "Dr. Khan", "Dr. Ali", "Dr. Sara", "Dr. Usman", "Dr. Zainab" };
        string[] durationInMinutes = { "30", "20", "45", "60", "25" };
        string[] times = { "09:00", "10:30", "14:15", "16:45", "11:00" };

        Random rand = new Random();

        
            
            
          List<Visit> visits = new List<Visit>();

            for (int i = 0; i < 400; i++)
            {
                string patientName = names[rand.Next(names.Length)];
                string visitDate = visitDates[rand.Next(visitDates.Length)];
                string time = times[rand.Next(times.Length)];
                string visitType = visitTypes[rand.Next(visitTypes.Length)];
                string note = notes[rand.Next(notes.Length)];
                string doctorName = doctorNames[rand.Next(doctorNames.Length)];
                string duration = durationInMinutes[rand.Next(durationInMinutes.Length)];

                string record = $"{patientName},{visitDate},{time},{visitType},{note},{doctorName},{duration}";

                Visit obj = new Visit(patientName,visitDate,time,visitType,note,doctorName,duration);
                visits.Add(obj);
            }
            SaveVisits(visits);
        

        
    }

    public static bool Conflict(string name, string date, string time)
    {
        bool flag = true;
        VisitManager manager = new VisitManager();
        List<Visit> visits = manager.LoadVisits();
        foreach (Visit visit in visits)
        {
            if (name == visit.PatientName && date==visit.VisitDate )
            {
                string[] curData=time.Split(':');
                string[] fileData = visit.Time.Split(':');
                int[] cData=new int[fileData.Length];
                int[] fData=new int[fileData.Length];
                for(int i = 0; i < curData.Length; i++)
                {
                    cData[i] = Convert.ToInt32(curData[i]);
                    fData[i] = Convert.ToInt32(fileData[i]);
                }

                if (cData[0] > fData[0] + 1 || fData[0] > cData[0]+1)
                {
                    flag = true;
                    break;
                }
                else
                {
                    int addHour = Math.Abs(cData[0]-fData[0]);
                    int addMin = Math.Abs(cData[1] - fData[1]);
                    int dif = (addHour * 60) + addMin;
                    if (dif >= 30)
                    {
                        flag = false;
                        break;
                    }
                }


            }
        }

        if (flag)
        {
            Console.Write("\n Warning: This patient has another visit within 30 minutes. Proceed? (Y/N) : ");
            string choice=Console.ReadLine();
            while (true)
            {
                if(choice.ToLower() == "y" || choice.ToLower() == "n")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\n Write correct input(Y/N): \n");
                    Console.Write("\n Warning: This patient has another visit within 30 minutes. Proceed? (Y/N) : ");
                     choice = Console.ReadLine();
                    if (choice.ToLower() == "y")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            
        }


        return true;

    }


    public void AddVisit(string role)
    {

        Console.Write("Enter patient name: ");
        string name = Console.ReadLine();
        name = Validator.NameValidator(name);


        Console.Write("Enter visit date (dd-MM-yyyy): ");
        string date = Console.ReadLine();
        date = Validator.DateValidator(date);

        Console.Write("Enter Time in format hh:mm : ");
        string time= Console.ReadLine();
        time = Validator.TimeValidator(time);
        bool toAdd=Conflict(name, date, time);
        if (!toAdd)
        {
            Console.WriteLine("\n Visit was not added due to time conflict. \n");
            log.SaveLog("add", toAdd,role,Convert.ToString(DateTime.Now));
            return;
        }


        Console.Write("Enter visit type: ");
        string type = Console.ReadLine();
        type = Validator.TypeValidator(type);


        Console.Write("Enter visit duration (in minutes): ");
        string duration = Console.ReadLine();
        duration=Validator.NumberValidator(duration);
        

        Console.Write("Enter notes: ");
        string notes = Console.ReadLine();

        Console.Write("Enter doctor name: ");
        string doctor = Console.ReadLine();
        doctor = Validator.NameValidator(doctor);


            Visit visit = new Visit(name, date, time, type, notes, doctor, duration);
            List<Visit> visits = LoadVisits();
            visits.Add(visit);
            SaveVisits(visits);

            undoStack.Push(new CommandDealer("Add", visit));
            if (undoStack.Count > MaxUndo) undoStack = new Stack<CommandDealer>(undoStack.ToArray()[..MaxUndo]);
            redoStack.Clear();

            Console.WriteLine("\n Visit added successfully!\n");
        log.SaveLog("add", toAdd, role, Convert.ToString(DateTime.Now));

    }

    public void ShowAllVisits(string role)
    {
        List<Visit> visits = LoadVisits();
        Console.WriteLine("All Visits:\n");
        foreach (Visit visit in visits)
        {
            Console.WriteLine(visit.Display());
            Console.WriteLine("-----------------------------------");
        }
        if(visits.Count> 0)
        {
            log.SaveLog("display", true, role, Convert.ToString(DateTime.Now));
        }
        if (visits.Count == 0)
        {
            Console.WriteLine("No records available.\n");
            log.SaveLog("display", false, role, Convert.ToString(DateTime.Now));
        }
    }

    public void SearchVisit(string role)
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
                log.SaveLog("display", true, role, Convert.ToString(DateTime.Now));
                i++;
            }
        }

        

        if (!found)
        {
            Console.WriteLine("No records found.\n");
            log.SaveLog("display", false, role, Convert.ToString(DateTime.Now));
        }
        else
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
        bool toUpdate = false;

        for (int i = 0; i < visits.Count; i++)
        {
            if (visits[i].PatientName.ToLower() == name.ToLower())
            {
                Visit oldVisit = new Visit(visits[i].PatientName, visits[i].VisitDate, visits[i].Time, visits[i].VisitType, visits[i].Notes, visits[i].DoctorName, visits[i].DurationInMinutes);

                Console.Write("Enter new visit date: ");
                string VisitDate = Validator.DateValidator(Console.ReadLine());
                visits[i].VisitDate = VisitDate;

                Console.Write("Enter Time in format: ");
                string Time1 =Validator.TimeValidator(Console.ReadLine());
                visits[i].Time = Time1;
                toUpdate = Conflict(visits[i].PatientName, VisitDate, Time1);
                if(!toUpdate)
                {
                    Console.WriteLine("\n Update function was not performed dut to time conflict\n");
                    return;
                }


                Console.Write("Enter new visit type: ");
                visits[i].VisitType = Console.ReadLine();
                visits[i].VisitType = Validator.TypeValidator(visits[i].VisitType);


                Console.Write("Enter new visit duration: ");
                visits[i].DurationInMinutes = Console.ReadLine();
                visits[i].DurationInMinutes = Validator.NumberValidator(visits[i].DurationInMinutes);


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
            log.SaveLog("display", true, "admin", Convert.ToString(DateTime.Now));
        }
        else
        {
            log.SaveLog("display", false, "admin", Convert.ToString(DateTime.Now));
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
            log.SaveLog("display", true, "admin", Convert.ToString(DateTime.Now));
            Console.WriteLine("\n***************************\n");
        }
        else
        {
            log.SaveLog("display", false, "admin", Convert.ToString(DateTime.Now));
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
            log.SaveLog("display", true, "admin", Convert.ToString(DateTime.Now));
            Console.WriteLine($"Total Visits by {name} : {count}");
            Console.WriteLine("***************************");

        }

        if (!found)
        {
            log.SaveLog("display", false, "admin", Convert.ToString(DateTime.Now));
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
        log.SaveLog("display", true, "admin", Convert.ToString(DateTime.Now));
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

        if (count > 0)
        {
            log.SaveLog("display", true, "admin", Convert.ToString(DateTime.Now));
        }

        if (count == 0)
        {
            log.SaveLog("display", false, "admin", Convert.ToString(DateTime.Now));
            Console.WriteLine("No visits in the past 7 days.\n");
        }
    }

    public void Undo()
    {
        if (undoStack.Count == 0)
        {
            Console.WriteLine("Nothing to undo.\n");
            log.SaveLog("display", false, "admin", Convert.ToString(DateTime.Now));
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
        log.SaveLog("display", true, "admin", Convert.ToString(DateTime.Now));
    }

    public void Redo()
    {
        if (redoStack.Count == 0)
        {
            Console.WriteLine(" Nothing to redo.\n");
            log.SaveLog("display", false, "admin", Convert.ToString(DateTime.Now));
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
        log.SaveLog("display", true, "admin", Convert.ToString(DateTime.Now));
    }


    public void FeeCalculator()
    {
        Console.Write("Enter Type = ");
        string type=Console.ReadLine();
        type = Validator.TypeValidator(type);

        string json = File.ReadAllText("Enum.json");
        var servicePrices = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
        if (servicePrices.TryGetValue(type, out int price))
        {
            Console.WriteLine("***************************");
            Console.WriteLine("\nPrice: " + price);
            Console.WriteLine("***************************");
        }
        log.SaveLog("display", true, "admin", Convert.ToString(DateTime.Now));

    }

}