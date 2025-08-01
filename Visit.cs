public class Visit
{
    public string PatientName { get; set; }
    public string VisitDate { get; set; }
    public string VisitType { get; set; }
    public string Notes { get; set; }
    public string DoctorName { get; set; }

    public Visit(string patientName, string visitDate, string visitType, string notes, string doctorName)
    {
        PatientName = patientName;
        VisitDate = visitDate;
        VisitType = visitType;
        Notes = notes;
        DoctorName = doctorName;
    }

    public string CommaAdd()
    {
        return $"{PatientName},{VisitDate},{VisitType},{Notes},{DoctorName}";
    }

    public static Visit CommaDel(string line)
    {
        var parts = line.Split(',');
        if (parts.Length < 5)
            return null;

        return new Visit(parts[0], parts[1], parts[2], parts[3], parts[4]);
    }

    public string Display()
    {
        return $"Patient: {PatientName}\nDate: {VisitDate}\nType: {VisitType}\nDoctor: {DoctorName}\nNotes: {Notes}";
    }
}
