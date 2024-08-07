namespace RuleConfiguration.Models;

public class Event
{
    public int Id { get; set; }

    public virtual Match Match { get; set; }

    public double Odd { get; set; }

    public string Outcome { get; set; }
    
    public bool IsLive { get; set; }
}