using RuleConfiguration.Models;

namespace RuleConfiguration.Modifiers;

public interface IModifier
{
    Ticket Modify(Ticket ticket);

    string GetModifierName();
}