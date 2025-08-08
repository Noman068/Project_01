using System.Collections.Generic;

public interface IVisitRepository
{
    List<Visit> LoadVisits();
    void SaveVisits(List<Visit> visits);
    void AddVisit(Visit visit);
    void UpdateVisit(Visit visit);
    void DeleteVisit(string patientName);
    List<Visit> SearchVisits(string keyword, int searchType);
} 