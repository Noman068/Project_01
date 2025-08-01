public class CommandDealer
{
    public string Action { get; set; }
    public Visit Visit { get; set; }
    public Visit Previous { get; set; }

    public CommandDealer(string action, Visit visit, Visit previous = null)
    {
        Action = action;
        Visit = visit;
        Previous = previous;
    }
}
