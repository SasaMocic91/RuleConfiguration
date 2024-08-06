using RuleConfiguration.Modifiers.ModelModifiers;
using RuleConfiguration.RuleCaches;

namespace RuleConfiguration.Repos;

public interface IRepoWrapper
{
    ITicketRulesCache TicketRules { get; }
    ITicketModifiers TicketModifiers { get; }
}