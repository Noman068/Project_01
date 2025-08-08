using System.Collections.Generic;

public interface IVisitService
{
    bool AddVisit(Visit visit, string role);
    bool UpdateVisit(string patientName, Visit updatedVisit, string role);
    bool DeleteVisit(string patientName, string role);
    List<Visit> GetAllVisits(string role);
    List<Visit> SearchVisits(string keyword, int searchType, string role);
    List<Visit> GetVisitsByPatient(string patientName);
    Dictionary<string, int> GetVisitTypeReport();
    List<Visit> GetWeeklySummary();
    void Undo(string role);
    void Redo(string role);
} 