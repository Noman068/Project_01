using System;

public interface IEmployee
{
    bool AuthenticateUser();
    
}


public interface IReceptionist : IEmployee
{
    void AddVisit();
    void SearchVisit();
    void ShowAllVisits();
}

public interface IAdmin : IEmployee , IReceptionist
{
	void DeleteVisit();
	void UpdateVisit();
	void ShowAllVisits();
	void ReportByVisitType();
	void ReportWeeklySummary();
	void UndoAction();
	void RedoAction();
	void ShowVisitSummaryByName();
	void FeeCalculator();

}