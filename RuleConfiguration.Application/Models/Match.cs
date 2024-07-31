namespace RuleConfigurator.Application.Models;

public class Match
{
    public int Id { get; set; }

    public string Home { get; set; }

    public string Away { get; set; }

    public virtual Tournament Tournament { get; set; }
}