using System;
using System.Collections.Generic;

public class Program
{
   
    static void Main(string[] args)
    {
        Console.WriteLine("*************************************");
        Console.WriteLine("*   CureMD - Patient Visit Manager  *");
        Console.WriteLine("*         by Noman Nawaz Cheema    *");
        Console.WriteLine("*************************************\n");

        Employee EmployeeInstance=new Employee();


        if (!EmployeeInstance.AuthenticateUser())
        {
            Console.WriteLine("\nAuthentication failed. Exiting...\n");
            return;
        }
        else
        {
            if(EmployeeInstance.role=="admin") {
                Admin AdminInstance = new Admin(EmployeeInstance.role);
            }
            else
            {
                Receptionist ReceptionistInstance = new Receptionist(EmployeeInstance.role);
            }
        }

       
        //GenerateMockData();
    }

    
    public static void GenerateMockData()
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

            Visit obj = new Visit(patientName, visitDate, time, visitType, note, doctorName, duration);
            visits.Add(obj);
        }
        CsvVisitRepository csvVisitRepository = new CsvVisitRepository();
        csvVisitRepository.SaveVisits(visits);
        
    }
}
    
