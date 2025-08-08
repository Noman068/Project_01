public class Visit
{
    public string PatientName { get; set; }
    public string VisitDate { get; set; }
    public string VisitType { get; set; }
    public string Notes { get; set; }
    public string DoctorName { get; set; }
    public string DurationInMinutes {  get; set; }

    public string Time {  get; set; }

    public Visit(string patientName, string visitDate, string time, string visitType, string notes, string doctorName, string durationInMinutes)
    {
        PatientName = patientName;
        VisitDate = visitDate;
        VisitType = visitType;
        Notes = notes;
        DoctorName = doctorName;
        DurationInMinutes = durationInMinutes;
        Time = time;
    }

    public string CommaAdd()
    {
        return $"{PatientName},{VisitDate},{Time},{VisitType},{Notes},{DoctorName},{DurationInMinutes}";
    }

    public static Visit CommaDel(string line)
    {
        var parts = line.Split(',');
        if (parts.Length < 7)
            return null;

        return new Visit(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6]);
    }

    public string Display()
    {
        return $"Patient: {PatientName}\nDate: {VisitDate}\n Time : {Time}\nType: {VisitType}\n Duration: {DurationInMinutes}\nDoctor: {DoctorName}\nNotes: {Notes}";
    }
}
