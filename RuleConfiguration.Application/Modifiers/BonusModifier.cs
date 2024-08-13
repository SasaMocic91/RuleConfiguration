using RuleConfiguration.Models;
using RuleConfiguration.Modifiers;

namespace RuleConfigurator.Application.Modifiers;

public class BonusModifier : IModifier
{
    public Ticket Modify(Ticket ticket)
    {
        const decimal bonus = 0.2M;
        ticket.Bonus = ticket.WinAmount * bonus;
        ticket.WinAmount += bonus;
        return ticket;
    }

    public string GetModifierName()
    {
        return nameof(BonusModifier);
    }
}