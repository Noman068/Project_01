using System;

public class ActivityLog
{
	string filePath = "Activity.txt";
	public void SaveLog(string action, bool performed, string role, string date)
	{
		try
		{
			string text = $"\n{action},{performed},{role},{date}";
			File.AppendAllText(filePath, text);
		}catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}

	}
}
