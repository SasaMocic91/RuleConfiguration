using RuleConfiguration.Models;
using RuleConfiguration.Modifiers;

namespace RuleConfigurator.Application.Modifiers;

public class JackpotModifier : IModifier
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