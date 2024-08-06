using RuleConfiguration.Models;
using RuleConfiguration.Storage.Repositories.Base;

namespace RuleConfiguration.Modifiers;

public class BonusModifier : IBaseModifier<Ticket>
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