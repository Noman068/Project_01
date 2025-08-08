public interface IActivityLogger
{
    void LogActivity(string action, bool performed, string role, string timestamp);
} 