using RuleConfiguration.Models;
using RuleConfiguration.Storage.Repositories.Base;

namespace RuleConfiguration.Modifiers;

public class TaxModifier : IBaseModifier<Ticket>
{
    public Ticket Modify(Ticket ticket)
    {
        ticket.Tax = ticket.WinAmount * 0.1;
        ticket.WinAmount -= ticket.Tax;
        return ticket;
    }

    public string GetModifierName()
    {
        return nameof(TaxModifier);
    }
}