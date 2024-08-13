using RuleConfiguration.Models;
using RuleConfiguration.Modifiers;

namespace RuleConfigurator.Application.Modifiers;

public class TaxModifier : IModifier
{
    public Ticket Modify(Ticket ticket)
    {
        ticket.Tax = ticket.WinAmount * 0.1M;
        ticket.WinAmount -= ticket.Tax;
        return ticket;
    }

    public string GetModifierName()
    {
        return nameof(TaxModifier);
    }
}