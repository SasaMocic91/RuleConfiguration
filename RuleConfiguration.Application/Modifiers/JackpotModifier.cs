using RuleConfiguration.Models;
using RuleConfiguration.Storage.Repositories.Base;

namespace RuleConfiguration.Modifiers;

public class JackpotModifier : IBaseModifier<Ticket>
{
    public Ticket Modify(Ticket ticket)
    {
        Jackpot.Increase(ticket);
        var current = Jackpot.CurrentAmount();
        foreach (var item in current) Console.WriteLine($"{item.Key} : {item.Value}");

        return ticket;
    }

    public string GetModifierName()
    {
        return nameof(JackpotModifier);
    }
}