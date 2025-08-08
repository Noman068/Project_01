using System;
using System.Collections.Generic;

public class VisitService : IVisitService
{
    private readonly IVisitRepository visitRepository;
    private readonly ICommandManager commandManager;
    private readonly IActivityLogger activityLogger;

    public VisitService(IVisitRepository visitRepository, ICommandManager commandManager, IActivityLogger activityLogger)
    {
        this.visitRepository = visitRepository;
        this.commandManager = commandManager;
        this.activityLogger = activityLogger;
    }

    public bool AddVisit(Visit visit, string role)
    {
        if (!ValidateVisit(visit))
        {
            activityLogger.LogActivity("add", false, role, Convert.ToString(DateTime.Now));
            return false;
        }

        if (!CheckTimeConflict(visit.PatientName, visit.VisitDate, visit.Time))
        {
            activityLogger.LogActivity("add", false, role, Convert.ToString(DateTime.Now));
            return false;
        }

        visitRepository.AddVisit(visit);
        commandManager.ExecuteCommand(new CommandDealer("Add", visit));
        activityLogger.LogActivity("add", true, role, Convert.ToString(DateTime.Now));
        return true;
    }

    public bool UpdateVisit(string patientName, Visit updatedVisit, string role)
    {
        if (!ValidateVisit(updatedVisit))
        {
            activityLogger.LogActivity("update", false, role, Convert.ToString(DateTime.Now));
            return false;
        }

        if (!CheckTimeConflict(updatedVisit.PatientName, updatedVisit.VisitDate, updatedVisit.Time))
        {
            activityLogger.LogActivity("update", false, role, Convert.ToString(DateTime.Now));
            return false;
        }

        List<Visit> visits = visitRepository.LoadVisits();
        Visit oldVisit = null;
        
        for (int i = 0; i < visits.Count; i++)
        {
            if (visits[i].PatientName.ToLower() == patientName.ToLower())
            {
                oldVisit = new Visit(visits[i].PatientName, visits[i].VisitDate, visits[i].Time, 
                                   visits[i].VisitType, visits[i].Notes, visits[i].DoctorName, visits[i].DurationInMinutes);
                visits[i] = updatedVisit;
                break;
            }
        }

        if (oldVisit != null)
        {
            visitRepository.SaveVisits(visits);
            commandManager.ExecuteCommand(new CommandDealer("Update", updatedVisit, oldVisit));
            activityLogger.LogActivity("update", true, role, Convert.ToString(DateTime.Now));
            return true;
        }

        activityLogger.LogActivity("update", false, role, Convert.ToString(DateTime.Now));
        return false;
    }

    public bool DeleteVisit(string patientName, string role)
    {
        List<Visit> visits = visitRepository.LoadVisits();
        Visit visitToDelete = null;

        for (int i = 0; i < visits.Count; i++)
        {
            if (visits[i].PatientName.ToLower() == patientName.ToLower())
            {
                visitToDelete = visits[i];
                visits.RemoveAt(i);
                break;
            }
        }

        if (visitToDelete != null)
        {
            visitRepository.SaveVisits(visits);
            commandManager.ExecuteCommand(new CommandDealer("Delete", visitToDelete));
            activityLogger.LogActivity("delete", true, role, Convert.ToString(DateTime.Now));
            return true;
        }

        activityLogger.LogActivity("delete", false, role, Convert.ToString(DateTime.Now));
        return false;
    }

    public List<Visit> GetAllVisits(string role)
    {
        return visitRepository.LoadVisits();
    }

    public List<Visit> SearchVisits(string keyword, int searchType, string role)
    {
        return visitRepository.SearchVisits(keyword, searchType);
    }

    public List<Visit> GetVisitsByPatient(string patientName)
    {
        return visitRepository.SearchVisits(patientName, 1);
    }

    public Dictionary<string, int> GetVisitTypeReport()
    {
        List<Visit> visits = visitRepository.LoadVisits();
        Dictionary<string, int> report = new Dictionary<string, int>();

        foreach (Visit visit in visits)
        {
            if (report.ContainsKey(visit.VisitType))
            {
                report[visit.VisitType]++;
            }
            else
            {
                report[visit.VisitType] = 1;
            }
        }

        return report;
    }

    public List<Visit> GetWeeklySummary()
    {
        List<Visit> visits = visitRepository.LoadVisits();
        List<Visit> weeklyVisits = new List<Visit>();
        DateTime currentDate = DateTime.Now;
        DateTime weekStart = currentDate.AddDays(-(int)currentDate.DayOfWeek);

        foreach (Visit visit in visits)
        {
            string[] dateParts = visit.VisitDate.Split('-');
            DateTime visitDate = new DateTime(Convert.ToInt32(dateParts[2]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[0]));

            if (visitDate >= weekStart && visitDate <= currentDate)
            {
                weeklyVisits.Add(visit);
            }
        }

        return weeklyVisits;
    }

    public void Undo(string role)
    {
        if (commandManager.CanUndo())
        {
            CommandDealer lastCommand = commandManager.GetLastCommand();
            List<Visit> visits = visitRepository.LoadVisits();

            if (lastCommand.Action == "Add")
            {
                for (int i = 0; i < visits.Count; i++)
                {
                    if (visits[i].PatientName.ToLower() == lastCommand.Visit.PatientName.ToLower())
                    {
                        visits.RemoveAt(i);
                        break;
                    }
                }
            }
            else if (lastCommand.Action == "Delete")
            {
                visits.Add(lastCommand.Visit);
            }
            else if (lastCommand.Action == "Update")
            {
                for (int i = 0; i < visits.Count; i++)
                {
                    if (visits[i].PatientName.ToLower() == lastCommand.Visit.PatientName.ToLower())
                    {
                        visits[i] = lastCommand.Previous;
                        break;
                    }
                }
            }

            visitRepository.SaveVisits(visits);
            commandManager.Undo();
            activityLogger.LogActivity("undo", true, role, Convert.ToString(DateTime.Now));
        }
    }

    public void Redo(string role)
    {
        if (commandManager.CanRedo())
        {
            CommandDealer lastCommand = commandManager.GetLastCommand();
            List<Visit> visits = visitRepository.LoadVisits();

            if (lastCommand.Action == "Add")
            {
                visits.Add(lastCommand.Visit);
            }
            else if (lastCommand.Action == "Delete")
            {
                for (int i = 0; i < visits.Count; i++)
                {
                    if (visits[i].PatientName.ToLower() == lastCommand.Visit.PatientName.ToLower())
                    {
                        visits.RemoveAt(i);
                        break;
                    }
                }
            }
            else if (lastCommand.Action == "Update")
            {
                for (int i = 0; i < visits.Count; i++)
                {
                    if (visits[i].PatientName.ToLower() == lastCommand.Visit.PatientName.ToLower())
                    {
                        visits[i] = lastCommand.Visit;
                        break;
                    }
                }
            }

            visitRepository.SaveVisits(visits);
            commandManager.Redo();
            activityLogger.LogActivity("redo", true, role, Convert.ToString(DateTime.Now));
        }
    }

    private bool ValidateVisit(Visit visit)
    {
        try
        {
            Validator.NameValidator(visit.PatientName);
            Validator.DateValidator(visit.VisitDate);
            Validator.TimeValidator(visit.Time);
            Validator.TypeValidator(visit.VisitType);
            Validator.NumberValidator(visit.DurationInMinutes);
            Validator.NameValidator(visit.DoctorName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool CheckTimeConflict(string patientName, string date, string time)
    {
        int flag = 0;
        List<Visit> visits = visitRepository.LoadVisits();
        foreach (Visit visit in visits)
        {
            if (patientName.ToLower() == visit.PatientName.ToLower() && date == visit.VisitDate)
            {
                string[] curData = time.Split(':');
                string[] fileData = visit.Time.Split(':');
                int[] cData = new int[curData.Length];
                int[] fData = new int[fileData.Length];
                for (int i = 0; i < curData.Length; i++)
                {
                    cData[i] = Convert.ToInt32(curData[i]);
                    fData[i] = Convert.ToInt32(fileData[i]);
                }

                if (cData[0] > fData[0] + 1 || fData[0] > cData[0] + 1)
                {
                    return true;
                }
                else
                {
                    int addHour = Math.Abs(cData[0] - fData[0]);
                    int addMin = Math.Abs(cData[1] - fData[1]);
                    int dif = Math.Abs((addHour * 60) - addMin);
                    if (dif <= 30)
                    {
                        flag++;
                        break;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            break;
        }

        if (flag > 0)
        {
            Console.Write("\n Warning: This patient has another visit within 30 minutes. Proceed? (Y/N) : ");
            string choice = Console.ReadLine();
            while (true)
            {
                if (choice.ToLower() == "y")
                {
                    break;
                }
                else if (choice.ToLower() == "n")
                {
                    return false;
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
} 