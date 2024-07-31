namespace RuleConfiguration.Models;

public class RuleRecord
{
    public RuleRecord(string key, List<Modifier> modifiers, string type, string op)
    {
        Key = key;
        Modifiers = modifiers;
        Type = type;
        Operator = op;
    }

    public string Key { get; set; }

    public List<Func<Ticket, bool>> Expressions { get; set; } = new();

    public List<Modifier> Modifiers { get; set; } = new();

    public string Type { get; set; }

    public string Operator { get; set; }
}