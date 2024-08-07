using RuleConfiguration.Modifiers.ModelModifiers;
using RuleConfiguration.RuleCaches;

namespace RuleConfiguration.Repos;

public interface IRepositoryWrapper
{
    ITicketRulesCache TicketRules { get; }
    ITicketModifiers TicketModifiers { get; }
}