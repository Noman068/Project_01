using System;
using System.Collections.Generic;
using System.IO;

public class CsvVisitRepository : IVisitRepository
{
    private readonly string filePath;

    public CsvVisitRepository(string filePath = "visits.csv")
    {
        this.filePath = filePath;
    }

    public List<Visit> LoadVisits()
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

    public void SaveVisits(List<Visit> visits)
    {
        List<string> lines = new List<string>();
        foreach (Visit visit in visits)
        {
            lines.Add(visit.CommaAdd());
        }
        File.WriteAllLines(filePath, lines);
    }

    public void AddVisit(Visit visit)
    {
        List<Visit> visits = LoadVisits();
        visits.Add(visit);
        SaveVisits(visits);
    }

    public void UpdateVisit(Visit updatedVisit)
    {
        List<Visit> visits = LoadVisits();
        for (int i = 0; i < visits.Count; i++)
        {
            if (visits[i].PatientName.Equals(updatedVisit.PatientName, StringComparison.OrdinalIgnoreCase))
            {
                visits[i] = updatedVisit;
                break;
            }
        }
        SaveVisits(visits);
    }

    public void DeleteVisit(string patientName)
    {
        List<Visit> visits = LoadVisits();
        visits.RemoveAll(v => v.PatientName.Equals(patientName, StringComparison.OrdinalIgnoreCase));
        SaveVisits(visits);
    }

    public List<Visit> SearchVisits(string keyword, int searchType)
    {
        List<Visit> visits = LoadVisits();
        List<Visit> results = new List<Visit>();

        foreach (Visit visit in visits)
        {
            bool match = false;
            switch (searchType)
            {
                case 1:
                    match = visit.PatientName.ToLower() == keyword.ToLower();
                    break;
                case 2:
                    match = visit.DoctorName.ToLower() == keyword.ToLower();
                    break;
                case 3:
                    match = visit.VisitDate == keyword;
                    break;
                case 4:
                    match = visit.VisitType.ToLower() == keyword.ToLower();
                    break;
            }
            if (match)
                results.Add(visit);
        }
        return results;
    }
} 