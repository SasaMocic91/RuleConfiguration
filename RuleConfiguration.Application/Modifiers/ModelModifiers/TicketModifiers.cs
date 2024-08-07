using RuleConfiguration.Models;
using RuleConfiguration.Storage.Repositories.Base;

namespace RuleConfiguration.Modifiers.ModelModifiers;

public interface ITicketModifiers : IBaseModifierRepo<Ticket>
{
}

public class TicketModifiers(IBaseRulesCache<Ticket> baseRulesCache) : BaseModifierRepo<Ticket>(baseRulesCache), ITicketModifiers
{
    public new List<IBaseModifier<Ticket>> AllModifiers { get; set; } =
        [new BonusModifier(), new TaxModifier(), new JackpotModifier()];

    public new async Task<Ticket> ApplyModifiers(Ticket data)
    {
        base.AllModifiers = AllModifiers;
        var result = await base.ApplyModifiers(data);
        return result;
    }
    
    
}