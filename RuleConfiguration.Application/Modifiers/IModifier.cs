using RuleConfigurator.Application.Models;

namespace RuleConfigurator.Application.Modifiers;

public interface IModifier
{
    Ticket Modify(Ticket ticket);

    string GetModifierName();
}